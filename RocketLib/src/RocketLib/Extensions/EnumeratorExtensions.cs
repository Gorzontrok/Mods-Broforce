using System;

public static class EnumeratorExtensions
{
    public static T[] GetValues<T>(this T enumerator) where T : Enum
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("T must be an enum type");
        }
        return (T[])Enum.GetValues(typeof(T));
    }
    public static string[] GetValuesAsStrings<T>(this T enumerator) where T : Enum
    {
        T[] values = enumerator.GetValues();
        string[] array = new string[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            array[i] = values[i].ToString();
        }
        return array;
    }
    public static string[] AsStrings<T>(this T[] enumerators) where T : Enum
    {
        string[] array = new string[enumerators.Length];
        for (int i = 0; i < enumerators.Length; i++)
        {
            array[i] = enumerators[i].ToString();
        }
        return array;
    }
}
