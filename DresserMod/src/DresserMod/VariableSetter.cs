using HarmonyLib;
using RocketLib.Loggers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DresserMod
{
    public static class VariableSetter
    {
        public const char PATH_SEPARATOR = '.';

        public static void Dynamic(object obj, Dictionary<string, object> map, Action<Traverse, string, object> setter)
        {
            if (setter == null)
            {
                setter = (t, s, o) => t.SetValue(o);
            }
            Traverse traverse = obj.GetTraverse();
            var field = traverse;
            foreach (KeyValuePair<string, object> kvp in map)
            {
                try
                {
                    field = FindFieldWithPath(traverse, kvp.Key);
                    setter(field, kvp.Key, kvp.Value);
                }
#pragma warning disable 0168
                catch (NullReferenceException e)
#pragma warning restore 0168
                { }
                catch (Exception e)
                {
                    Main.Log($"Key: {kvp.Key} ; Value: {kvp.Value}\nAt type field {field.GetValueType()}\n" + e);
                }
            }
        }

        public static Traverse FindFieldWithPath(Traverse traverse, string fieldPath)
        {
            if (fieldPath.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(fieldPath));

            var field = traverse;
            string[] path = fieldPath.Split(PATH_SEPARATOR);

            foreach (string variable in path)
            {
                if (variable.StartsWith("__C_"))
                {
                    object current = field.GetValue();
                    string componentName = variable.Substring(3, variable.Length - 3);
                    Component component = null;
                    if (current.As<Component>())
                    {
                        component = current.As<Component>().GetComponent(componentName);
                    }
                    if (current.As<GameObject>())
                    {
                        component = current.As<GameObject>().GetComponent(componentName);
                    }
                    if (component != null)
                    {
                        field = Traverse.Create(component);
                    }
                    else
                    {
                        throw new ArgumentException($"Can't get Component {componentName} from {field.ToString()}");
                    }
                }
                field = field.Field(variable);
            }
            return field;
        }
    }
}
