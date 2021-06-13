using System.Diagnostics;

namespace Quze.Infrastruture.Extensions
{
    public static class StringExtensions
    {
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this string me)
        {
            return string.IsNullOrEmpty(me);
        }

        [DebuggerStepThrough]
        public static bool IsNotNullOrEmpty(this string me)
        {
            return !string.IsNullOrEmpty(me);
        }

        [DebuggerStepThrough]
        public static bool IsNullOrWiteSpaces(this string me)
        {
            return string.IsNullOrWhiteSpace(me);
        }

        [DebuggerStepThrough]
        public static bool IsNotNullOrWiteSpaces(this string me)
        {
            return !me.IsNullOrWiteSpaces();
        }

        [DebuggerStepThrough]
        public static string ValueOrEmpty(this string me)
        {
            return me.IsNull() ? string.Empty : me;
        }
    }
}
