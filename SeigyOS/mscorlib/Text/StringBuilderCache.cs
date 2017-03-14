namespace System.Text
{
    internal static class StringBuilderCache
    {
        private const int cMaxBuilderSize = 360;

        [ThreadStatic]
        private static StringBuilder _cachedInstance;

        public static StringBuilder Acquire(int capacity = StringBuilder.DefaultCapacity)
        {
            if (capacity <= cMaxBuilderSize)
            {
                StringBuilder sb = _cachedInstance;
                if (sb != null && capacity <= sb.Capacity)
                {
                    _cachedInstance = null;
                    sb.Clear();
                    return sb;
                }
            }
            return new StringBuilder(capacity);
        }

        public static void Release(StringBuilder sb)
        {
            if (sb.Capacity <= cMaxBuilderSize)
                _cachedInstance = sb;
        }

        public static string GetStringAndRelease(StringBuilder sb)
        {
            string result = sb.ToString();
            Release(sb);
            return result;
        }
    }
}