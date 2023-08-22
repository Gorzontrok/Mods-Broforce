using HarmonyLib;
using RocketLib.Loggers;
using System;
using System.Collections.Generic;

public static class HarmonyExtensions
{
    public const char PATH_SEPARATOR = '.';
    public static Traverse GetTraverse(this object obj)
    {
        return Traverse.Create(obj);
    }

    #region Fields
    public static T GetFieldValue<T>(this object obj, string fieldName)
    {
        if (obj is Traverse)
            return (obj as Traverse).Field(fieldName).GetValue<T>();
        return obj.GetTraverse().Field(fieldName).GetValue<T>();
    }

    public static object GetFieldValue(this object obj, string fieldName)
    {
        if (obj is Traverse)
            return (obj as Traverse).Field(fieldName).GetValue();
        return obj.GetTraverse().Field(fieldName).GetValue();
    }

    public static Traverse SetFieldValue<T>(this object obj, string fieldName, T value)
    {
        if (obj is Traverse)
            return (obj as Traverse).Field(fieldName).SetValue(value);
        return obj.GetTraverse().Field(fieldName).SetValue(value);
    }
    public static Traverse SetFieldValue(this object obj, string fieldName, object value)
    {
        if (obj is Traverse)
            return (obj as Traverse).Field(fieldName).SetValue(value);
        return obj.GetTraverse().Field(fieldName).SetValue(value);
    }
    #endregion

    #region Methods
    public static T CallMethod<T>(this object obj, string methodName, params object[] arguments)
    {
        if (obj is Traverse)
            return (obj as Traverse).Method(methodName, arguments).GetValue<T>();
        return obj.GetTraverse().Method(methodName, arguments).GetValue<T>();
    }
    public static object CallMethod(this object obj, string methodName, params object[] arguments)
    {
        if (obj is Traverse)
            return (obj as Traverse).Method(methodName, arguments).GetValue();
        return obj.GetTraverse().Method(methodName, arguments).GetValue();
    }
    #endregion

    #region Specific Fields
    public static bool GetBool(this object obj, string fieldName)
    {
        return obj.GetFieldValue<bool>(fieldName);
    }
    public static float GetFloat(this object obj, string fieldName)
    {
        return obj.GetFieldValue<float>(fieldName);
    }
    public static int GetInt(this object obj, string fieldName)
    {
        return obj.GetFieldValue<int>(fieldName);
    }

    #endregion
    public static void DynamicFieldsValueSetter(this object obj, Dictionary<string, object> map, string[] skipTheseFields = null, Action<Traverse, string, object> setter = null)
    {
        if(setter == null)
        {
            setter = (t, s, o) => t.SetValue(o);
        }

        if(skipTheseFields == null)
        {
            skipTheseFields= new string[0];
        }

        Traverse traverse = obj.GetTraverse();
        var field = traverse;
        foreach(KeyValuePair<string, object> kvp in map)
        {
            try
            {
                if (!skipTheseFields.Contains(kvp.Key))
                {
                    field = traverse.FindFieldWithPath(kvp.Key);
                    setter(field, kvp.Key, kvp.Value);
                }
            }
#pragma warning disable 0168
            catch (NullReferenceException e)
#pragma warning restore 0168
            { }
            catch (Exception e)
            {
                ScreenLogger.Instance.ExceptionLog($"Key: {kvp.Key} ; Value: {kvp.Value}\nAt type field {field.GetValueType()}",e);
            }
        }
    }

    public static Traverse FindFieldWithPath(this object obj, string fieldPath)
    {
        return FindFieldWithPath(obj.GetTraverse(), fieldPath);
    }
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