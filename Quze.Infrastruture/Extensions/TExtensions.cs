using System;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace Quze.Infrastruture.Extensions
{
    public static class TExtensions
    {
        [DebuggerStepThrough]
        public static bool IsNull<T>(this T me)
        {
            if (me is INullable && (me as INullable).IsNull) return true;
            var type = typeof(T);
            if (type.IsValueType)
            {
                if (!ReferenceEquals(Nullable.GetUnderlyingType(type), null) && me.GetHashCode() == 0) return true;
            }
            else
            {
                if (ReferenceEquals(me, null)) return true;
                if (Convert.IsDBNull(me)) return true;
            }
            return false;
        }

        public static bool IsNotNull<T>(this T me)
        {
            return !IsNull(me);
        }

        [DebuggerStepThrough]
        public static bool IsNull<T>
        (this T? me)
        where T : struct
        {
            return !me.HasValue;
        }

    }

 

}
