namespace System
{
    public static class ThrowIfNullExtension
    {
        public static void ThrowIfNull<T>(this T value, string paramName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}

