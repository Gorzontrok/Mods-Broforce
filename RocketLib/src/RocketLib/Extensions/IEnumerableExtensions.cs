using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class IEnumerableExtensions
{
    /// <summary>
    /// Returns random element from collection
    /// </summary>
    public static T RandomElement<T>(this IEnumerable<T> self)
    {
        return self.ElementAt(UnityEngine.Random.Range(0, self.Count()));
    }

    /// <summary>
    /// Returns random index from collection
    /// </summary>
    public static int RandomIndex<T>(this IEnumerable<T> self)
    {
        return UnityEngine.Random.Range(0, self.Count());
    }

    /// <summary>
    /// Is list null or empty
    /// </summary>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> self)
    {
        return self == null || self.Count() == 0;
    }
    /// <summary>
    /// Is list NOT null or empty
    /// </summary>
    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> self)
    {
        return !self.IsNullOrEmpty();
    }
}