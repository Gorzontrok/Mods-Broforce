using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class ObjectExtensions
{
    public static void DestroyMe(this UnityEngine.Object target)
    {
        UnityEngine.Object.Destroy(target);
    }

    // Credits: Deadcows/MyBox | Licence: MIT
    /// <summary>
    /// Check if this is a particular type.
    /// </summary>
    public static bool Is<T>(this object source)
    {
        return source is T;
    }

    // Credits: Deadcows/MyBox | Licence: MIT
    /// <summary>
    /// Cast to a different type, exception-safe.
    /// </summary>
    public static T As<T>(this object source) where T : class
    {
        return source as T;
    }

    public static bool NotAs<T>(this object source) where T : class
    {
        return source as T == null;
    }

    public static bool IsTypeOf(this  object obj, Type type)
    {
        return obj.GetType() == type;
    }

    /// <summary>
    /// Not great
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="baseType"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    /// <exception cref="MissingMethodException"></exception>
    /// <exception cref="Exception"></exception>
    public static void InvokeBaseMethod(this object obj, Type baseType, string methodName)
    {
        obj.InvokeBaseMethod<object>(baseType, methodName);
    }
    /// <summary>
    /// Not great
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="baseType"></param>
    /// <param name="methodName"></param>
    /// <returns></returns>
    /// <exception cref="MissingMethodException"></exception>
    /// <exception cref="Exception"></exception>
    public static T InvokeBaseMethod<T>(this object obj, Type baseType, string methodName) where T : class
    {
        var type = obj.GetType();

        var method = baseType.GetMethod(methodName);
        if (method == null)
            throw new MissingMethodException(methodName);
        if (type == baseType)
        {
            return method.Invoke(obj, null) as T;
        }
        if (!type.IsSubclassOf(baseType))
            throw new Exception($"{type} is not a subclass of {baseType}");

        var ptr = method.MethodHandle.GetFunctionPointer();
        var baseMethod = (Func<T>)Activator.CreateInstance(typeof(Func<T>), obj, ptr);
        return baseMethod.Invoke() as T;
    }
}
