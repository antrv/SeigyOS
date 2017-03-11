using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;
using System.__Helpers;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Char: IComparable, IConvertible, IComparable<char>, IEquatable<char>
    {
        public const char MinValue = (char)0x00;
        public const char MaxValue = (char)0xFFFF;

        private readonly char _value;

        public override int GetHashCode()
        {
            return _value | (_value << 16);
        }

        public override bool Equals(object obj)
        {
            return obj is char && _value == (char)obj;
        }

        public bool Equals(char obj)
        {
            return _value == obj;
        }

        [Pure]
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (!(value is char))
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_MustBeChar));
            return _value - (char)value;
        }

        [Pure]
        public int CompareTo(char value)
        {
            return _value - value;
        }

        [Pure]
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return ToString(_value);
        }

        [Pure]
        public string ToString(IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return char.ToString(_value);
        }

        [Pure]
        public static string ToString(char c)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return new string(c, 1);
        }

        public static char Parse(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            Contract.EndContractBlock();
            if (s.Length != 1)
                throw new FormatException(__Resources.GetResourceString(__Resources.Format_NeedSingleChar));
            return s[0];
        }

        public static bool TryParse(string s, out char result)
        {
            result = '\0';
            if (s == null || s.Length != 1)
                return false;
            result = s[0];
            return true;
        }

        [Pure]
        public static bool IsDigit(char c)
        {
            if (CharHelper.IsLatin1(c))
                return c >= '0' && c <= '9';
            return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.DecimalDigitNumber;
        }

        private static bool CheckLetter(UnicodeCategory uc)
        {
            switch (uc)
            {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.OtherLetter:
                    return true;
                default:
                    return false;
            }
        }

        [Pure]
        public static bool IsLetter(char c)
        {
            if (CharHelper.IsLatin1(c))
            {
                if (CharHelper.IsAscii(c))
                {
                    c |= (char)0x20;
                    return c >= 'a' && c <= 'z';
                }
                return CheckLetter(CharHelper.GetLatin1UnicodeCategory(c));
            }
            return CheckLetter(CharUnicodeInfo.GetUnicodeCategory(c));
        }

        private static bool IsWhiteSpaceLatin1(char c)
        {
            return c == ' ' || c >= '\x0009' && c <= '\x000d' || c == '\x00a0' || c == '\x0085';
        }

        [Pure]
        public static bool IsWhiteSpace(char c)
        {
            if (CharHelper.IsLatin1(c))
                return IsWhiteSpaceLatin1(c);
            return CharUnicodeInfo.IsWhiteSpace(c);
        }

        [Pure]
        public static bool IsUpper(char c)
        {
            if (CharHelper.IsLatin1(c))
            {
                if (CharHelper.IsAscii(c))
                    return c >= 'A' && c <= 'Z';
                return CharHelper.GetLatin1UnicodeCategory(c) == UnicodeCategory.UppercaseLetter;
            }
            return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.UppercaseLetter;
        }

        [Pure]
        public static bool IsLower(char c)
        {
            if (CharHelper.IsLatin1(c))
            {
                if (CharHelper.IsAscii(c))
                    return c >= 'a' && c <= 'z';
                return CharHelper.GetLatin1UnicodeCategory(c) == UnicodeCategory.LowercaseLetter;
            }
            return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.LowercaseLetter;
        }

        private static bool CheckPunctuation(UnicodeCategory uc)
        {
            switch (uc)
            {
                case UnicodeCategory.ConnectorPunctuation:
                case UnicodeCategory.DashPunctuation:
                case UnicodeCategory.OpenPunctuation:
                case UnicodeCategory.ClosePunctuation:
                case UnicodeCategory.InitialQuotePunctuation:
                case UnicodeCategory.FinalQuotePunctuation:
                case UnicodeCategory.OtherPunctuation:
                    return true;
                default:
                    return false;
            }
        }

        [Pure]
        public static bool IsPunctuation(char c)
        {
            if (CharHelper.IsLatin1(c))
                return CheckPunctuation(CharHelper.GetLatin1UnicodeCategory(c));
            return CheckPunctuation(CharUnicodeInfo.GetUnicodeCategory(c));
        }

        private static bool CheckLetterOrDigit(UnicodeCategory uc)
        {
            switch (uc)
            {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.OtherLetter:
                case UnicodeCategory.DecimalDigitNumber:
                    return true;
                default:
                    return false;
            }
        }

        [Pure]
        public static bool IsLetterOrDigit(char c)
        {
            if (CharHelper.IsLatin1(c))
                return CheckLetterOrDigit(CharHelper.GetLatin1UnicodeCategory(c));
            return CheckLetterOrDigit(CharUnicodeInfo.GetUnicodeCategory(c));
        }

        public static char ToUpper(char c, CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));
            Contract.EndContractBlock();
            return culture.TextInfo.ToUpper(c);
        }

        public static char ToUpper(char c)
        {
            return ToUpper(c, CultureInfo.CurrentCulture);
        }

        public static char ToUpperInvariant(char c)
        {
            return ToUpper(c, CultureInfo.InvariantCulture);
        }

        public static char ToLower(char c, CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));
            Contract.EndContractBlock();
            return culture.TextInfo.ToLower(c);
        }

        public static char ToLower(char c)
        {
            return ToLower(c, CultureInfo.CurrentCulture);
        }

        public static char ToLowerInvariant(char c)
        {
            return ToLower(c, CultureInfo.InvariantCulture);
        }

        [Pure]
        public TypeCode GetTypeCode()
        {
            return TypeCode.Char;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Char", "Boolean"));
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return _value;
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(_value);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(_value);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(_value);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(_value);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(_value);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(_value);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(_value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(_value);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Char", "Single"));
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Char", "Double"));
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Char", "Decimal"));
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Char", "DateTime"));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType(this, type, provider);
        }

        public static bool IsControl(char c)
        {
            if (CharHelper.IsLatin1(c))
                return CharHelper.GetLatin1UnicodeCategory(c) == UnicodeCategory.Control;
            return CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.Control;
        }

        public static bool IsControl(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsControl(s[index]);
        }

        public static bool IsDigit(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsDigit(s[index]);
        }

        public static bool IsLetter(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsLetter(s[index]);
        }

        public static bool IsLetterOrDigit(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsLetterOrDigit(s[index]);
        }

        public static bool IsLower(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsLower(s[index]);
        }

        private static bool CheckNumber(UnicodeCategory uc)
        {
            switch (uc)
            {
                case UnicodeCategory.DecimalDigitNumber:
                case UnicodeCategory.LetterNumber:
                case UnicodeCategory.OtherNumber:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNumber(char c)
        {
            if (CharHelper.IsLatin1(c))
            {
                if (CharHelper.IsAscii(c))
                    return c >= '0' && c <= '9';
                return CheckNumber(CharHelper.GetLatin1UnicodeCategory(c));
            }
            return CheckNumber(CharUnicodeInfo.GetUnicodeCategory(c));
        }

        public static bool IsNumber(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsNumber(s[index]);
        }

        public static bool IsPunctuation(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsPunctuation(s[index]);
        }

        internal static bool CheckSeparator(UnicodeCategory uc)
        {
            switch (uc)
            {
                case UnicodeCategory.SpaceSeparator:
                case UnicodeCategory.LineSeparator:
                case UnicodeCategory.ParagraphSeparator:
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsSeparatorLatin1(char c)
        {
            return c == '\x0020' || c == '\x00a0';
        }

        public static bool IsSeparator(char c)
        {
            if (CharHelper.IsLatin1(c))
                return IsSeparatorLatin1(c);
            return CheckSeparator(CharUnicodeInfo.GetUnicodeCategory(c));
        }

        public static bool IsSeparator(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsSeparator(s[index]);
        }

        [Pure]
        public static bool IsSurrogate(char c)
        {
            return c >= CharHelper.HighSurrogateStart && c <= CharHelper.LowSurrogateEnd;
        }

        [Pure]
        public static bool IsSurrogate(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsSurrogate(s[index]);
        }

        private static bool CheckSymbol(UnicodeCategory uc)
        {
            switch (uc)
            {
                case UnicodeCategory.MathSymbol:
                case UnicodeCategory.CurrencySymbol:
                case UnicodeCategory.ModifierSymbol:
                case UnicodeCategory.OtherSymbol:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsSymbol(char c)
        {
            if (CharHelper.IsLatin1(c))
                return CheckSymbol(CharHelper.GetLatin1UnicodeCategory(c));
            return CheckSymbol(CharUnicodeInfo.GetUnicodeCategory(c));
        }

        public static bool IsSymbol(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsSymbol(s[index]);
        }

        public static bool IsUpper(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsUpper(s[index]);
        }

        public static bool IsWhiteSpace(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsWhiteSpace(s[index]);
        }

        public static UnicodeCategory GetUnicodeCategory(char c)
        {
            if (CharHelper.IsLatin1(c))
                return CharHelper.GetLatin1UnicodeCategory(c);
            return CharUnicodeInfo.InternalGetUnicodeCategory(c);
        }

        public static UnicodeCategory GetUnicodeCategory(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return GetUnicodeCategory(s[index]);
        }

        public static double GetNumericValue(char c)
        {
            return CharUnicodeInfo.GetNumericValue(c);
        }

        public static double GetNumericValue(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return CharUnicodeInfo.GetNumericValue(s, index);
        }

        [Pure]
        public static bool IsHighSurrogate(char c)
        {
            return c >= CharHelper.HighSurrogateStart && c <= CharHelper.HighSurrogateEnd;
        }

        [Pure]
        public static bool IsHighSurrogate(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsHighSurrogate(s[index]);
        }

        [Pure]
        public static bool IsLowSurrogate(char c)
        {
            return c >= CharHelper.LowSurrogateStart && c <= CharHelper.LowSurrogateEnd;
        }

        [Pure]
        public static bool IsLowSurrogate(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            return IsLowSurrogate(s[index]);
        }

        [Pure]
        public static bool IsSurrogatePair(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if ((uint)index >= (uint)s.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            Contract.EndContractBlock();
            if (index + 1 < s.Length)
                return IsSurrogatePair(s[index], s[index + 1]);
            return false;
        }

        [Pure]
        public static bool IsSurrogatePair(char highSurrogate, char lowSurrogate)
        {
            return highSurrogate >= CharHelper.HighSurrogateStart && highSurrogate <= CharHelper.HighSurrogateEnd &&
                lowSurrogate >= CharHelper.LowSurrogateStart && lowSurrogate <= CharHelper.LowSurrogateEnd;
        }

        public static string ConvertFromUtf32(int utf32)
        {
            if (utf32 < 0 || utf32 > CharHelper.UnicodePlane16End || utf32 >= CharHelper.HighSurrogateStart && utf32 <= CharHelper.LowSurrogateEnd)
                throw new ArgumentOutOfRangeException(nameof(utf32), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_InvalidUTF32));
            Contract.EndContractBlock();

            if (utf32 < CharHelper.UnicodePlane01Start)
                return ToString((char)utf32);

            utf32 -= CharHelper.UnicodePlane01Start;
            char[] surrogate = new char[2];
            surrogate[0] = (char)(utf32 / 0x400 + (int)CharHelper.HighSurrogateStart);
            surrogate[1] = (char)(utf32 % 0x400 + (int)CharHelper.LowSurrogateStart);
            return new string(surrogate);
        }

        public static int ConvertToUtf32(char highSurrogate, char lowSurrogate)
        {
            if (!IsHighSurrogate(highSurrogate))
                throw new ArgumentOutOfRangeException(nameof(highSurrogate), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_InvalidHighSurrogate));
            if (!IsLowSurrogate(lowSurrogate))
                throw new ArgumentOutOfRangeException(nameof(lowSurrogate), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_InvalidLowSurrogate));
            Contract.EndContractBlock();
            return (highSurrogate - CharHelper.HighSurrogateStart) * 0x400 + (lowSurrogate - CharHelper.LowSurrogateStart) + CharHelper.UnicodePlane01Start;
        }

        public static int ConvertToUtf32(string s, int index)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (index < 0 || index >= s.Length)
                throw new ArgumentOutOfRangeException(nameof(index), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Index));
            Contract.EndContractBlock();
            int temp1 = s[index] - CharHelper.HighSurrogateStart;
            if (temp1 >= 0 && temp1 <= 0x7ff)
            {
                if (temp1 <= 0x3ff)
                {
                    if (index < s.Length - 1)
                    {
                        int temp2 = s[index + 1] - CharHelper.LowSurrogateStart;
                        if (temp2 >= 0 && temp2 <= 0x3ff)
                            return temp1 * 0x400 + temp2 + CharHelper.UnicodePlane01Start;
                        throw new ArgumentException(__Resources.GetResourceString(__Resources.Argument_InvalidHighSurrogate, index), nameof(s));
                    }
                    throw new ArgumentException(__Resources.GetResourceString(__Resources.Argument_InvalidHighSurrogate, index), nameof(s));
                }
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Argument_InvalidLowSurrogate, index), nameof(s));
            }
            return s[index];
        }
    }
}