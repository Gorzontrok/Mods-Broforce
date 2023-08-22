using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
