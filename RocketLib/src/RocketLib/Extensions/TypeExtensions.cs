using System;

namespace RocketLib
{
    public static class TypeExtensions
    {
        public static bool CanBeNull(this Type type)
        {
            return !type.IsValueType || (Nullable.GetUnderlyingType(type) != null);
        }
    }
}
