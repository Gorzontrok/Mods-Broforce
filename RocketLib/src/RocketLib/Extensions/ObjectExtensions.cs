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
}
