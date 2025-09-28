using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using UnityModManagerNet;

namespace RocketLib.Utils
{
    /// <summary>
    /// Provides recovery functionality for corrupted or incompatible XML settings files.
    /// When UnityModManager fails to deserialize a settings file due to XML errors or schema changes,
    /// this class attempts to recover as much data as possible from the invalid XML.
    /// </summary>
    /// <example>
    /// <code>
    /// // Settings.cs
    /// public int SettingsVersion = 0;
    /// 
    /// public override void Save(UnityModManager.ModEntry modEntry)
    /// {
    ///     if (SettingsVersion == 0) SettingsVersion = 1;
    ///     Save(this, modEntry);
    /// }
    /// 
    /// // Main.cs Load method
    /// settings = Settings.Load&lt;Settings&gt;(modEntry);
    /// if (settings.SettingsVersion == 0)
    /// {
    ///     var settingsPath = Path.Combine(modEntry.Path, "Settings.xml");
    ///     settings = SettingsRecovery.TryRecoverSettings&lt;Settings&gt;(settingsPath, settings);
    /// }
    /// if (settings.SettingsVersion == 0) settings.SettingsVersion = 1;
    /// </code>
    /// </example>
    public static class SettingsRecovery
    {
        /// <summary>
        /// Attempts to recover settings from a potentially corrupted XML file.
        /// </summary>
        /// <typeparam name="T">The settings class type, must inherit from UnityModManager.ModSettings</typeparam>
        /// <param name="xmlPath">Path to the XML settings file to recover from</param>
        /// <param name="defaultSettings">Optional default settings object to use as base, or null to create new</param>
        /// <returns>A settings object with recovered values where possible, defaults for unrecoverable fields</returns>
        /// <remarks>
        /// Recovery process:
        /// - Simple types (bool, int, float, string, enum) are recovered individually
        /// - Arrays and Lists are recovered element by element, skipping invalid items
        /// - Complex objects are recovered if their XML structure is valid
        /// - Fields with XML attribute mappings ([XmlArray], [XmlElement]) are properly handled
        /// - Invalid or missing fields are left at their default values
        /// </remarks>
        public static T TryRecoverSettings<T>(string xmlPath, T defaultSettings = null) where T : UnityModManager.ModSettings, new()
        {
            if (!File.Exists(xmlPath))
                return defaultSettings ?? new T();

            var result = defaultSettings ?? new T();

            try
            {
                var doc = new XmlDocument();
                doc.Load(xmlPath);

                var root = doc.DocumentElement;
                if (root == null)
                    return result;

                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in properties)
                {
                    if (!prop.CanWrite)
                        continue;

                    RecoverProperty(root, prop, result);
                }

                var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

                foreach (var field in fields)
                {
                    RecoverField(root, field, result);
                }
            }
            catch
            {
                // If even the recovery fails, just return the default
            }

            return result;
        }

        private static void RecoverProperty(XmlElement root, PropertyInfo prop, object target)
        {
            try
            {
                // Check for XML attributes that might rename the element
                string elementName = prop.Name;
                var xmlArrayAttr = prop.GetCustomAttributes(typeof(XmlArrayAttribute), false).FirstOrDefault() as XmlArrayAttribute;
                if (xmlArrayAttr != null && !string.IsNullOrEmpty(xmlArrayAttr.ElementName))
                    elementName = xmlArrayAttr.ElementName;

                var xmlElementAttr = prop.GetCustomAttributes(typeof(XmlElementAttribute), false).FirstOrDefault() as XmlElementAttribute;
                if (xmlElementAttr != null && !string.IsNullOrEmpty(xmlElementAttr.ElementName))
                    elementName = xmlElementAttr.ElementName;

                var element = root[elementName];
                if (element == null)
                    return;

                // Try to deserialize the property - complex types will be handled by XmlSerializer
                var value = DeserializeValue(element, prop.PropertyType);
                if (value != null)
                {
                    prop.SetValue(target, value, null);
                }
            }
            catch
            {
                // Skip this property if recovery fails
            }
        }

        private static void RecoverField(XmlElement root, FieldInfo field, object target)
        {
            try
            {
                // Check for XML attributes that might rename the element
                string elementName = field.Name;
                var xmlArrayAttr = field.GetCustomAttributes(typeof(XmlArrayAttribute), false).FirstOrDefault() as XmlArrayAttribute;
                if (xmlArrayAttr != null && !string.IsNullOrEmpty(xmlArrayAttr.ElementName))
                    elementName = xmlArrayAttr.ElementName;

                var xmlElementAttr = field.GetCustomAttributes(typeof(XmlElementAttribute), false).FirstOrDefault() as XmlElementAttribute;
                if (xmlElementAttr != null && !string.IsNullOrEmpty(xmlElementAttr.ElementName))
                    elementName = xmlElementAttr.ElementName;

                var element = root[elementName];
                if (element == null)
                    return;

                // Try to deserialize the field - complex types will be handled by XmlSerializer
                var value = DeserializeValue(element, field.FieldType);
                if (value != null)
                {
                    field.SetValue(target, value);
                }
            }
            catch
            {
                // Skip this field if recovery fails
            }
        }

        private static object DeserializeValue(XmlElement element, Type targetType)
        {
            try
            {
                // Handle primitive types and strings
                if (targetType == typeof(string))
                    return element.InnerText;

                if (targetType == typeof(bool))
                    return bool.Parse(element.InnerText);

                if (targetType == typeof(int))
                    return int.Parse(element.InnerText);

                if (targetType == typeof(float))
                    return float.Parse(element.InnerText);

                if (targetType == typeof(double))
                    return double.Parse(element.InnerText);

                if (targetType == typeof(long))
                    return long.Parse(element.InnerText);

                if (targetType == typeof(short))
                    return short.Parse(element.InnerText);

                if (targetType == typeof(byte))
                    return byte.Parse(element.InnerText);

                if (targetType == typeof(char))
                    return char.Parse(element.InnerText);

                // Handle enums
                if (targetType.IsEnum)
                {
                    return Enum.Parse(targetType, element.InnerText);
                }

                // For other types, try using XmlSerializer
                try
                {
                    // Special handling for arrays - XmlSerializer expects the array elements directly
                    if (targetType.IsArray)
                    {
                        var elementType = targetType.GetElementType();
                        var items = new System.Collections.ArrayList();

                        foreach (XmlNode child in element.ChildNodes)
                        {
                            if (child.NodeType == XmlNodeType.Element)
                            {
                                var childElement = child as XmlElement;
                                var item = DeserializeValue(childElement, elementType);
                                if (item != null)
                                    items.Add(item);
                            }
                        }

                        var array = Array.CreateInstance(elementType, items.Count);
                        items.CopyTo(array, 0);
                        return array;
                    }

                    // Special handling for generic lists
                    if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>))
                    {
                        var elementType = targetType.GetGenericArguments()[0];
                        var listType = typeof(System.Collections.Generic.List<>).MakeGenericType(elementType);
                        var list = Activator.CreateInstance(listType);
                        var addMethod = listType.GetMethod("Add");

                        foreach (XmlNode child in element.ChildNodes)
                        {
                            if (child.NodeType == XmlNodeType.Element)
                            {
                                var childElement = child as XmlElement;
                                var item = DeserializeValue(childElement, elementType);
                                if (item != null)
                                    addMethod.Invoke(list, new object[] { item });
                            }
                        }

                        return list;
                    }

                    // For non-array types, use standard XmlSerializer
                    // Check if the type expects a different root element name
                    var xmlRootAttr = targetType.GetCustomAttributes(typeof(XmlRootAttribute), false).FirstOrDefault() as XmlRootAttribute;
                    XmlSerializer serializer;

                    if (xmlRootAttr != null && !string.IsNullOrEmpty(xmlRootAttr.ElementName))
                    {
                        // If the element name in XML doesn't match the expected root name, wrap it
                        if (element.Name != xmlRootAttr.ElementName)
                        {
                            serializer = new XmlSerializer(targetType, new XmlRootAttribute(element.Name));
                        }
                        else
                        {
                            serializer = new XmlSerializer(targetType);
                        }
                    }
                    else if (element.Name != targetType.Name)
                    {
                        // Element name doesn't match type name, tell XmlSerializer to expect this element name
                        serializer = new XmlSerializer(targetType, new XmlRootAttribute(element.Name));
                    }
                    else
                    {
                        serializer = new XmlSerializer(targetType);
                    }

                    using (var reader = new StringReader(element.OuterXml))
                    {
                        return serializer.Deserialize(reader);
                    }
                }
                catch
                {
                    // XmlSerializer failed for this type
                    return null;
                }
            }
            catch
            {
                // Failed to deserialize this value
            }

            return null;
        }
    }
}