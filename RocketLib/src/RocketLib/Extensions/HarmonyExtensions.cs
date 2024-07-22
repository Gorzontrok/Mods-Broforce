using HarmonyLib;
using RocketLib.Loggers;
using System;
using System.Collections.Generic;

public static class HarmonyExtensions
{
    public const char PATH_SEPARATOR = '.';
    public static Traverse GetTraverse(this object obj)
    {
        if (obj as Traverse != null)
            return (Traverse)obj;
        return Traverse.Create(obj);
    }

    #region Fields
    /// <summary>
    /// Get the value of a field as <typeparamref name="T"/>
    /// </summary>
    /// <seealso cref="HarmonyLib.Traverse.Field(string).GetValue{T}()"/>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static T GetFieldValue<T>(this object obj, string fieldName)
    {
        return obj.GetTraverse().Field(fieldName).GetValue<T>();
    }

    /// <summary>
    /// Get the value of a field
    /// </summary>
    /// <seealso cref="HarmonyLib.Traverse.Field(string).GetValue()"/>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static object GetFieldValue(this object obj, string fieldName)
    {
        return obj.GetTraverse().Field(fieldName).GetValue();
    }

    /// <summary>
    /// Set the value of the field <paramref name="fieldName"/> with <paramref name="value"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    public static void SetFieldValue(this object obj, string fieldName, object value)
    {
        obj.GetTraverse().Field(fieldName).SetValue(value);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Call a method then return value as <typeparamref name="T"/>
    /// </summary>
    /// <seealso cref="HarmonyLib.Traverse.Method(string, object[]).GetValue{T}()"/>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="methodName"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public static T CallMethod<T>(this object obj, string methodName, params object[] arguments)
    {
        return obj.GetTraverse().Method(methodName, arguments).GetValue<T>();
    }
    /// <summary>
    /// Call a method then return value
    /// </summary>
    /// <seealso cref="HarmonyLib.Traverse.Method(string, object[]).GetValue()"/>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="methodName"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public static object CallMethod(this object obj, string methodName, params object[] arguments)
    {
        return obj.GetTraverse().Method(methodName, arguments).GetValue();
    }
    #endregion

    #region Specific Fields
    /// <summary>
    /// Get bool field value
    /// </summary>
    /// <seealso cref="GetFieldValue{bool}(object, string)"/>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static bool GetBool(this object obj, string fieldName)
    {
        return obj.GetFieldValue<bool>(fieldName);
    }
    /// <summary>
    /// Get float field value
    /// </summary>
    /// <seealso cref="GetFieldValue{float}(object, string)"/>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static float GetFloat(this object obj, string fieldName)
    {
        return obj.GetFieldValue<float>(fieldName);
    }
    /// <summary>
    /// Get int field value
    /// </summary>
    /// <seealso cref="GetFieldValue{float}(object, string)"/>
    /// <param name="obj"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public static int GetInt(this object obj, string fieldName)
    {
        return obj.GetFieldValue<int>(fieldName);
    }
    #endregion

    /// <summary>
    /// Set multiple fields on an object from a <see cref="Dictionary{string, object}"/>
    /// Can navigate through instances from fields using the separator <see cref="PATH_SEPARATOR"/> in the <see cref="Dictionary{string, object}"/> key.
    /// <seealso cref="FindFieldWithPath(object, string)"/>
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="map">Key: Field Name | Value: the value to give to the field</param>
    /// <param name="skipTheseFields">Fields to ignore</param>
    /// <param name="setter">Custom action to set fields</param>
    public static void DynamicFieldsValueSetter(this object obj, Dictionary<string, object> map, string[] skipTheseFields = null, Action<Traverse, string, object> setter = null)
    {
        if (setter == null)
        {
            setter = (t, s, o) => t.SetValue(o);
        }

        if (skipTheseFields == null)
        {
            skipTheseFields = new string[0];
        }

        Traverse traverse = obj.GetTraverse();
        var field = traverse;
        foreach (KeyValuePair<string, object> kvp in map)
        {
            try
            {
                if (!skipTheseFields.Contains(kvp.Key))
                {
                    field = traverse.FindFieldWithPath(kvp.Key);
                    setter(field, kvp.Key, kvp.Value);
                }
            }
            catch (NullReferenceException)
            { }
            catch (Exception e)
            {
                ScreenLogger.Instance.ExceptionLog($"Key: {kvp.Key} ; Value: {kvp.Value}\nAt type field {field.GetValueType()}", e);
            }
        }
    }

    /// <summary>
    /// Use <see cref="PATH_SEPARATOR"/> to navigate through the objects.
    /// </summary>
    /// <example>
    /// We have 2 classes
    /// <code>
    /// class A
    /// {
    ///     B b;
    /// }
    /// class B
    /// {
    ///     int number = 8;
    /// }
    /// </code>
    /// If we want to get 'B.Number' from A, <paramref name="fieldPath"/> should be 'b.number'
    /// </example>
    /// <param name="obj"></param>
    /// <param name="fieldPath"></param>
    /// <returns></returns>
    public static Traverse FindFieldWithPath(this object obj, string fieldPath)
    {
        return FindFieldWithPath(obj.GetTraverse(), fieldPath);
    }

    /// <summary>
    /// Use <see cref="PATH_SEPARATOR"/> to navigate through the objects.
    /// </summary>
    /// <example>
    /// We have 2 classes
    /// <code>
    /// class A
    /// {
    ///     B b;
    /// }
    /// class B
    /// {
    ///     int number = 8;
    /// }
    /// </code>
    /// If we want to get 'B.Number' from A, <paramref name="fieldPath"/> should be 'b.number'
    /// </example>
    /// <param name="traverse"></param>
    /// <param name="fieldPath"></param>
    /// <returns></returns>
    public static Traverse FindFieldWithPath(this Traverse traverse, string fieldPath)
    {
        var field = traverse;
        string[] path = fieldPath.Split(PATH_SEPARATOR);
        foreach (string variable in path)
        {
            field = field.Field(variable);
        }
        return field;
    }
}