using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RocketLib.Utils
{
    /// <summary>
    /// Provides functionality to compare two objects and find all differences between them
    /// </summary>
    public static class ObjectComparer
    {
        /// <summary>
        /// Maximum recursion depth to prevent stack overflow from circular references
        /// </summary>
        private const int MAX_DEPTH = 10;

        /// <summary>
        /// Internal class used to track pairs of objects that have already been compared
        /// to prevent infinite recursion on circular references
        /// </summary>
        private class ObjectPair
        {
            public object Obj1 { get; set; }
            public object Obj2 { get; set; }

            public override bool Equals(object obj)
            {
                var other = obj as ObjectPair;
                if (other == null) return false;
                return ReferenceEquals(Obj1, other.Obj1) && ReferenceEquals(Obj2, other.Obj2);
            }

            public override int GetHashCode()
            {
                var hash1 = Obj1 != null ? Obj1.GetHashCode() : 0;
                var hash2 = Obj2 != null ? Obj2.GetHashCode() : 0;
                return hash1 ^ hash2;
            }
        }

        /// <summary>
        /// Properties that should be skipped during comparison to avoid reflection metadata
        /// and circular references common in Unity objects
        /// </summary>
        private static readonly HashSet<string> SkipProperties = new HashSet<string>
        {
            // Reflection properties
            "Props", "ReflectedType", "DeclaringType", "Module", "Assembly",
            "TypeHandle", "MetadataToken", "Attributes", "CustomAttributes",
            "UnderlyingSystemType", "BaseType", "StructLayoutAttribute",
            "GenericTypeArguments", "GenericParameterAttributes", "GenericParameterPosition",
            "DeclaringMethod", "TypeId",
            
            // Unity circular reference properties
            "gameObject", "transform", "parent", "root",
            "normalized", "right", "up", "forward",
            
            // Other problematic properties
            "Item", "SyncRoot", "IsReadOnly", "IsFixedSize"
        };

        /// <summary>
        /// Type names that should be treated as simple types and compared directly
        /// without recursing into their properties
        /// </summary>
        private static readonly HashSet<string> SimpleTypeNames = new HashSet<string>
        {
            "Vector2", "Vector3", "Vector4", "Quaternion", "Color", "Color32",
            "Rect", "Bounds", "Matrix4x4", "AnimationCurve", "Gradient"
        };

        /// <summary>
        /// Compares two objects of the same type and returns a list of all differences found
        /// </summary>
        /// <typeparam name="T">The type of objects being compared</typeparam>
        /// <param name="obj1">The first object to compare</param>
        /// <param name="obj2">The second object to compare</param>
        /// <returns>A list of all differences found between the two objects</returns>
        public static List<Difference> Compare<T>(T obj1, T obj2)
        {
            var visitedPairs = new HashSet<ObjectPair>();
            return CompareInternal(obj1, obj2, "", 0, visitedPairs);
        }

        /// <summary>
        /// Internal recursive comparison method that tracks depth and visited pairs
        /// </summary>
        private static List<Difference> CompareInternal<T>(T obj1, T obj2, string path, int depth, HashSet<ObjectPair> visitedPairs)
        {
            var differences = new List<Difference>();

            if (depth > MAX_DEPTH)
                return differences;

            try
            {
                if (ReferenceEquals(obj1, obj2))
                    return differences;

                if (obj1 == null || obj2 == null)
                {
                    differences.Add(new Difference
                    {
                        PropertyPath = path,
                        Value1 = obj1 != null ? obj1.ToString() : "null",
                        Value2 = obj2 != null ? obj2.ToString() : "null"
                    });
                    return differences;
                }

                var type = typeof(T);

                // Handle simple types
                if (IsSimpleType(type))
                {
                    if (!obj1.Equals(obj2))
                    {
                        differences.Add(new Difference
                        {
                            PropertyPath = path,
                            Value1 = obj1.ToString(),
                            Value2 = obj2.ToString()
                        });
                    }
                    return differences;
                }

                // Check for circular references
                var pair = new ObjectPair { Obj1 = obj1, Obj2 = obj2 };
                if (!type.IsValueType && visitedPairs.Contains(pair))
                    return differences;

                if (!type.IsValueType)
                    visitedPairs.Add(pair);

                // Handle arrays
                if (type.IsArray)
                {
                    var array1 = obj1 as Array;
                    var array2 = obj2 as Array;

                    if (array1.Length != array2.Length)
                    {
                        differences.Add(new Difference
                        {
                            PropertyPath = $"{path}.Length",
                            Value1 = array1.Length.ToString(),
                            Value2 = array2.Length.ToString()
                        });
                    }

                    var minLength = array1.Length < array2.Length ? array1.Length : array2.Length;
                    for (int i = 0; i < minLength; i++)
                    {
                        var itemDiffs = CompareObjectsNonGeneric(
                            array1.GetValue(i),
                            array2.GetValue(i),
                            $"{path}[{i}]",
                            depth + 1,
                            visitedPairs);
                        differences.AddRange(itemDiffs);
                    }

                    return differences;
                }

                // Handle other collections (but not strings)
                if (obj1 is IEnumerable && !(obj1 is string))
                {
                    var enum1 = obj1 as IEnumerable;
                    var enum2 = obj2 as IEnumerable;

                    var list1 = new List<object>();
                    var list2 = new List<object>();

                    foreach (var item in enum1) list1.Add(item);
                    foreach (var item in enum2) list2.Add(item);

                    if (list1.Count != list2.Count)
                    {
                        differences.Add(new Difference
                        {
                            PropertyPath = $"{path}.Count",
                            Value1 = list1.Count.ToString(),
                            Value2 = list2.Count.ToString()
                        });
                    }

                    var minCount = list1.Count < list2.Count ? list1.Count : list2.Count;
                    for (int i = 0; i < minCount; i++)
                    {
                        var itemDiffs = CompareObjectsNonGeneric(
                            list1[i],
                            list2[i],
                            $"{path}[{i}]",
                            depth + 1,
                            visitedPairs);
                        differences.AddRange(itemDiffs);
                    }

                    return differences;
                }

                // Handle regular objects - get all properties
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in properties)
                {
                    try
                    {
                        // Skip properties we can't read
                        if (!prop.CanRead)
                            continue;

                        // Skip indexer properties
                        if (prop.GetIndexParameters().Length > 0)
                            continue;

                        // Skip blacklisted properties
                        if (SkipProperties.Contains(prop.Name))
                            continue;

                        var value1 = prop.GetValue(obj1, null);
                        var value2 = prop.GetValue(obj2, null);
                        var propertyPath = string.IsNullOrEmpty(path) ? prop.Name : $"{path}.{prop.Name}";

                        if (value1 == null && value2 == null)
                            continue;

                        if (value1 == null || value2 == null)
                        {
                            differences.Add(new Difference
                            {
                                PropertyPath = propertyPath,
                                Value1 = value1 != null ? value1.ToString() : "null",
                                Value2 = value2 != null ? value2.ToString() : "null"
                            });
                            continue;
                        }

                        // For simple types, compare directly
                        if (IsSimpleType(prop.PropertyType))
                        {
                            if (!value1.Equals(value2))
                            {
                                differences.Add(new Difference
                                {
                                    PropertyPath = propertyPath,
                                    Value1 = value1.ToString(),
                                    Value2 = value2.ToString()
                                });
                            }
                        }
                        else
                        {
                            // For complex types, recurse
                            var nestedDiffs = CompareObjectsNonGeneric(value1, value2, propertyPath, depth + 1, visitedPairs);
                            differences.AddRange(nestedDiffs);
                        }
                    }
                    catch (Exception)
                    {
                        // Skip properties that throw exceptions
                    }
                }

                // Also check fields if requested
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    try
                    {
                        if (SkipProperties.Contains(field.Name))
                            continue;

                        var value1 = field.GetValue(obj1);
                        var value2 = field.GetValue(obj2);
                        var fieldPath = string.IsNullOrEmpty(path) ? field.Name : $"{path}.{field.Name}";

                        if (value1 == null && value2 == null)
                            continue;

                        if (value1 == null || value2 == null)
                        {
                            differences.Add(new Difference
                            {
                                PropertyPath = fieldPath,
                                Value1 = value1 != null ? value1.ToString() : "null",
                                Value2 = value2 != null ? value2.ToString() : "null"
                            });
                            continue;
                        }

                        if (IsSimpleType(field.FieldType))
                        {
                            if (!value1.Equals(value2))
                            {
                                differences.Add(new Difference
                                {
                                    PropertyPath = fieldPath,
                                    Value1 = value1.ToString(),
                                    Value2 = value2.ToString()
                                });
                            }
                        }
                        else
                        {
                            var nestedDiffs = CompareObjectsNonGeneric(value1, value2, fieldPath, depth + 1, visitedPairs);
                            differences.AddRange(nestedDiffs);
                        }
                    }
                    catch (Exception)
                    {
                        // Skip fields that throw exceptions
                    }
                }
            }
            catch (Exception)
            {
                // Silently ignore comparison errors
            }

            return differences;
        }

        /// <summary>
        /// Compares two objects when their type is not known at compile time
        /// </summary>
        private static List<Difference> CompareObjectsNonGeneric(object obj1, object obj2, string path, int depth, HashSet<ObjectPair> visitedPairs)
        {
            if (depth > MAX_DEPTH)
                return new List<Difference>();

            if (obj1 == null || obj2 == null)
            {
                return new List<Difference>
                {
                    new Difference
                    {
                        PropertyPath = path,
                        Value1 = obj1 != null ? obj1.ToString() : "null",
                        Value2 = obj2 != null ? obj2.ToString() : "null"
                    }
                };
            }

            var type = obj1.GetType();
            var method = typeof(ObjectComparer)
                .GetMethod(nameof(CompareInternal), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(type);

            return (List<Difference>)method.Invoke(null, new object[] { obj1, obj2, path, depth, visitedPairs });
        }

        /// <summary>
        /// Determines if a type should be treated as simple (compared directly without recursion)
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if the type is simple, false otherwise</returns>
        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                || type == typeof(TimeSpan)
                || type == typeof(Guid)
                || type.IsEnum
                || SimpleTypeNames.Contains(type.Name)
                || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                    && IsSimpleType(Nullable.GetUnderlyingType(type)));
        }

        /// <summary>
        /// Generates a single assignment statement for a property path and value.
        /// Uses 'this.' prefix as the code is meant to be placed inside the class.
        /// </summary>
        /// <param name="propertyPath">The property path (e.g. "sprite.width")</param>
        /// <param name="value">The value to assign</param>
        /// <returns>A C# assignment statement, or empty string if unable to generate</returns>
        internal static string GenerateMatchingStatement(string propertyPath, string value)
        {
            // Skip complex object properties that we can't handle
            if (value.Contains("Center:") || value.Contains("Extents:") ||
                propertyPath.Contains(".Count") || propertyPath.Contains(".Length"))
            {
                return "";
            }

            // Skip object types (format: "Name (Type)" or array types like "System.Byte[]")
            if (IsObjectType(value))
            {
                return "";
            }

            // Handle vector types
            if (IsVectorValue(value))
            {
                return GenerateVectorAssignment(propertyPath, value);
            }

            // Handle boolean values (with proper casing)
            if (IsBool(value))
            {
                var boolValue = value.ToLowerInvariant();
                return $"this.{propertyPath} = {boolValue};";
            }

            // Handle numeric values
            if (IsNumeric(value))
            {
                // Check if it's a float (has decimal point)
                if (value.Contains("."))
                {
                    return $"this.{propertyPath} = {value}f;";
                }
                else
                {
                    return $"this.{propertyPath} = {value};";
                }
            }

            // Handle string values
            // Escape quotes in string values
            var escapedValue = value.Replace("\"", "\\\"");
            return $"this.{propertyPath} = \"{escapedValue}\";";
        }

        /// <summary>
        /// Determines if a value represents a vector type
        /// </summary>
        internal static bool IsVectorValue(string value)
        {
            // Check for vector format: (x, y) or (x, y, z) or (x, y, z, w)
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^\s*\(\s*-?\d+\.?\d*\s*(,\s*-?\d+\.?\d*\s*){1,3}\)\s*$");
        }

        /// <summary>
        /// Generates an assignment statement for vector types using 'this.' prefix
        /// </summary>
        internal static string GenerateVectorAssignment(string propertyPath, string value)
        {
            // Extract numbers from the vector string
            var numbers = System.Text.RegularExpressions.Regex.Matches(value, @"-?\d+\.?\d*")
                .Cast<System.Text.RegularExpressions.Match>()
                .Select(m => m.Value)
                .ToList();

            // Add 'f' suffix to each number
            var formattedNumbers = numbers.Select(n => n.Contains(".") ? n + "f" : n + "f").ToList();

            if (formattedNumbers.Count == 2)
            {
                return $"this.{propertyPath} = new Vector2({formattedNumbers[0]}, {formattedNumbers[1]});";
            }
            else if (formattedNumbers.Count == 3)
            {
                return $"this.{propertyPath} = new Vector3({formattedNumbers[0]}, {formattedNumbers[1]}, {formattedNumbers[2]});";
            }
            else if (formattedNumbers.Count == 4)
            {
                return $"this.{propertyPath} = new Vector4({formattedNumbers[0]}, {formattedNumbers[1]}, {formattedNumbers[2]}, {formattedNumbers[3]});";
            }

            return "";
        }

        /// <summary>
        /// Determines if a value is numeric or boolean
        /// </summary>
        internal static bool IsNumericOrBool(string value)
        {
            return IsBool(value) || IsNumeric(value);
        }

        /// <summary>
        /// Determines if a value is boolean
        /// </summary>
        internal static bool IsBool(string value)
        {
            return string.Equals(value, "True", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(value, "False", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines if a value is numeric
        /// </summary>
        internal static bool IsNumeric(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^-?\d+\.?\d*$");
        }

        /// <summary>
        /// Determines if a value represents an object type that should be skipped
        /// </summary>
        internal static bool IsObjectType(string value)
        {
            // Check for "Name (Type)" pattern
            if (System.Text.RegularExpressions.Regex.IsMatch(value, @".+\s*\(.+\)$"))
                return true;

            // Check for array types like "System.Byte[]"
            if (value.EndsWith("[]") && value.Contains("."))
                return true;

            // Check for null
            if (value == "null")
                return true;

            return false;
        }
    }

    /// <summary>
    /// Represents a single difference between two objects
    /// </summary>
    public class Difference
    {
        /// <summary>
        /// The path to the property or field that differs (e.g. "Person.Address.Street")
        /// </summary>
        public string PropertyPath { get; set; }

        /// <summary>
        /// The string representation of the value from the first object
        /// </summary>
        public string Value1 { get; set; }

        /// <summary>
        /// The string representation of the value from the second object
        /// </summary>
        public string Value2 { get; set; }

        /// <summary>
        /// Returns a formatted string representation of the difference
        /// </summary>
        public override string ToString()
        {
            return $"{PropertyPath}: '{Value1}' → '{Value2}'";
        }
    }
}