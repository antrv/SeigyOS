namespace System
{
    // ReSharper disable once InconsistentNaming
    internal static class __ThrowHelper
    {
        internal static void ThrowNotSupportedException(__ResourceName message)
        {
            string messageString = __Resources.GetResourceString(message);
            throw new NotSupportedException(messageString);
        }

        internal static void ThrowArgumentException(__ResourceName message)
        {
            string messageString = __Resources.GetResourceString(message);
            throw new ArgumentException(messageString);
        }

        internal static void ThrowArgumentOutOfRangeException()
        {
            throw new ArgumentOutOfRangeException();
        }

        internal static void ThrowArgumentOutOfRangeException(__ResourceName paramName, __ResourceName message)
        {
            string paramNameString = __Resources.GetResourceString(paramName);
            string messageString = __Resources.GetResourceString(message);
            throw new ArgumentOutOfRangeException(paramNameString, messageString);
        }

        internal static void ThrowArgumentNullException(__ResourceName paramName)
        {
            string paramNameString = __Resources.GetResourceString(paramName);
            throw new ArgumentNullException(paramNameString);
        }
    }
}