using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace RocketLib
{
    /// <summary>
    /// Generic key-value pair for XML serialization of dictionaries
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary key</typeparam>
    /// <typeparam name="TValue">The type of the dictionary value</typeparam>
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue>
    {
        [XmlElement("Key")]
        public TKey Key { get; set; }

        [XmlElement("Value")]
        public TValue Value { get; set; }

        public SerializableKeyValuePair() { }

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}

/// <summary>
/// Extension methods for easy dictionary serialization with Unity Mod Manager's XML settings
/// </summary>
public static class DictionarySerializationExtensions
{
    /// <summary>
    /// Converts a dictionary to an array of SerializableKeyValuePair for XML serialization
    /// </summary>
    public static RocketLib.SerializableKeyValuePair<TKey, TValue>[] ToSerializableArray<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary)
    {
        if (dictionary == null)
            return new RocketLib.SerializableKeyValuePair<TKey, TValue>[0];

        return dictionary.Select(kvp => new RocketLib.SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value)).ToArray();
    }

    /// <summary>
    /// Converts an array of SerializableKeyValuePair back to a dictionary
    /// </summary>
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
        this RocketLib.SerializableKeyValuePair<TKey, TValue>[] array)
    {
        if (array == null)
            return new Dictionary<TKey, TValue>();

        return System.Linq.Enumerable.ToDictionary(array, kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    /// Converts a dictionary to an array using a custom converter function
    /// </summary>
    public static TWrapper[] ToSerializableArray<TKey, TValue, TWrapper>(
        this Dictionary<TKey, TValue> dictionary,
        Func<KeyValuePair<TKey, TValue>, TWrapper> converter)
    {
        if (dictionary == null)
            return new TWrapper[0];

        return dictionary.Select(converter).ToArray();
    }

    /// <summary>
    /// Converts an array back to a dictionary using custom converter functions
    /// </summary>
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue, TWrapper>(
        this TWrapper[] array,
        Func<TWrapper, TKey> keySelector,
        Func<TWrapper, TValue> valueSelector)
    {
        if (array == null)
            return new Dictionary<TKey, TValue>();

        return System.Linq.Enumerable.ToDictionary(array, keySelector, valueSelector);
    }
}