using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security;
using System.__Helpers;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    [ComVisible(true)]
    public struct Guid: IFormattable, IComparable, IComparable<Guid>, IEquatable<Guid>
    {
        public static readonly Guid Empty;

        private int _a;
        private short _b;
        private short _c;
        private byte _d;
        private byte _e;
        private byte _f;
        private byte _g;
        private byte _h;
        private byte _i;
        private byte _j;
        private byte _k;

        public Guid(byte[] b)
        {
            if (b == null)
                throw new ArgumentNullException(nameof(b));
            if (b.Length != 16)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_GuidArrayCtor, "16"));
            Contract.EndContractBlock();

            _a = (b[3] << 24) | (b[2] << 16) | (b[1] << 8) | b[0];
            _b = (short)((b[5] << 8) | b[4]);
            _c = (short)((b[7] << 8) | b[6]);
            _d = b[8];
            _e = b[9];
            _f = b[10];
            _g = b[11];
            _h = b[12];
            _i = b[13];
            _j = b[14];
            _k = b[15];
        }

        [CLSCompliant(false)]
        public Guid(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
        {
            _a = (int)a;
            _b = (short)b;
            _c = (short)c;
            _d = d;
            _e = e;
            _f = f;
            _g = g;
            _h = h;
            _i = i;
            _j = j;
            _k = k;
        }

        public Guid(int a, short b, short c, byte[] d)
        {
            if (d == null)
                throw new ArgumentNullException(nameof(d));
            if (d.Length != 8)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_GuidArrayCtor, "8"));
            Contract.EndContractBlock();

            _a = a;
            _b = b;
            _c = c;
            _d = d[0];
            _e = d[1];
            _f = d[2];
            _g = d[3];
            _h = d[4];
            _i = d[5];
            _j = d[6];
            _k = d[7];
        }

        public Guid(int a, short b, short c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
        {
            _a = a;
            _b = b;
            _c = c;
            _d = d;
            _e = e;
            _f = f;
            _g = g;
            _h = h;
            _i = i;
            _j = j;
            _k = k;
        }


        public Guid(string g)
        {
            if (g == null)
                throw new ArgumentNullException(nameof(g));
            Contract.EndContractBlock();
            this = Empty;
            GuidResult result = new GuidResult();
            result.Init(GuidParseThrowStyle.All);
            if (TryParseGuid(g, GuidStyles.Any, ref result))
                this = result.parsedGuid;
            else
                throw result.GetGuidParseException();
        }

        public static Guid Parse(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            Contract.EndContractBlock();
            GuidResult result = new GuidResult();
            result.Init(GuidParseThrowStyle.AllButOverflow);
            if (TryParseGuid(input, GuidStyles.Any, ref result))
                return result.parsedGuid;
            throw result.GetGuidParseException();
        }

        public static bool TryParse(string input, out Guid result)
        {
            GuidResult parseResult = new GuidResult();
            parseResult.Init(GuidParseThrowStyle.None);
            if (TryParseGuid(input, GuidStyles.Any, ref parseResult))
            {
                result = parseResult.parsedGuid;
                return true;
            }
            result = Empty;
            return false;
        }

        public static Guid ParseExact(string input, string format)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (format == null)
                throw new ArgumentNullException(nameof(format));
            if (format.Length != 1)
                throw new FormatException(__Resources.GetResourceString(__Resources.Format_InvalidGuidFormatSpecification));

            GuidStyles style;
            char formatCh = format[0];
            if (formatCh == 'D' || formatCh == 'd')
                style = GuidStyles.DigitFormat;
            else if (formatCh == 'N' || formatCh == 'n')
                style = GuidStyles.NumberFormat;
            else if (formatCh == 'B' || formatCh == 'b')
                style = GuidStyles.BraceFormat;
            else if (formatCh == 'P' || formatCh == 'p')
                style = GuidStyles.ParenthesisFormat;
            else if (formatCh == 'X' || formatCh == 'x')
                style = GuidStyles.HexFormat;
            else
                throw new FormatException(__Resources.GetResourceString(__Resources.Format_InvalidGuidFormatSpecification));

            GuidResult result = new GuidResult();
            result.Init(GuidParseThrowStyle.AllButOverflow);
            if (TryParseGuid(input, style, ref result))
                return result.parsedGuid;
            throw result.GetGuidParseException();
        }

        public static bool TryParseExact(string input, string format, out Guid result)
        {
            if (format == null || format.Length != 1)
            {
                result = Empty;
                return false;
            }

            GuidStyles style;
            char formatCh = format[0];
            if (formatCh == 'D' || formatCh == 'd')
                style = GuidStyles.DigitFormat;
            else if (formatCh == 'N' || formatCh == 'n')
                style = GuidStyles.NumberFormat;
            else if (formatCh == 'B' || formatCh == 'b')
                style = GuidStyles.BraceFormat;
            else if (formatCh == 'P' || formatCh == 'p')
                style = GuidStyles.ParenthesisFormat;
            else if (formatCh == 'X' || formatCh == 'x')
                style = GuidStyles.HexFormat;
            else
            {
                result = Empty;
                return false;
            }

            GuidResult parseResult = new GuidResult();
            parseResult.Init(GuidParseThrowStyle.None);
            if (TryParseGuid(input, style, ref parseResult))
            {
                result = parseResult.parsedGuid;
                return true;
            }
            result = Empty;
            return false;
        }

        public byte[] ToByteArray()
        {
            byte[] g = new byte[16];

            g[0] = (byte)_a;
            g[1] = (byte)(_a >> 8);
            g[2] = (byte)(_a >> 16);
            g[3] = (byte)(_a >> 24);
            g[4] = (byte)_b;
            g[5] = (byte)(_b >> 8);
            g[6] = (byte)_c;
            g[7] = (byte)(_c >> 8);
            g[8] = _d;
            g[9] = _e;
            g[10] = _f;
            g[11] = _g;
            g[12] = _h;
            g[13] = _i;
            g[14] = _j;
            g[15] = _k;

            return g;
        }

        public override string ToString()
        {
            return ToString("D", null);
        }

        public override int GetHashCode()
        {
            return _a ^ ((_b << 16) | (ushort)_c) ^ ((_f << 24) | _k);
        }

        public override bool Equals(object o)
        {
            if (!(o is Guid))
                return false;
            Guid g = (Guid)o;
            return g._a == _a && g._b == _b && g._c == _c && g._d == _d && g._e == _e && g._f == _f &&
                   g._g == _g && g._h == _h && g._i == _i && g._j == _j && g._k == _k;
        }

        public bool Equals(Guid g)
        {
            return g._a == _a && g._b == _b && g._c == _c && g._d == _d && g._e == _e && g._f == _f &&
                   g._g == _g && g._h == _h && g._i == _i && g._j == _j && g._k == _k;
        }

        private static int GetResult(uint me, uint them)
        {
            return me < them ? -1 : 1;
        }

        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (!(value is Guid))
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_MustBeGuid));

            Guid g = (Guid)value;
            if (g._a != _a)
                return GetResult((uint)_a, (uint)g._a);
            if (g._b != _b)
                return GetResult((uint)_b, (uint)g._b);
            if (g._c != _c)
                return GetResult((uint)_c, (uint)g._c);
            if (g._d != _d)
                return GetResult(_d, g._d);
            if (g._e != _e)
                return GetResult(_e, g._e);
            if (g._f != _f)
                return GetResult(_f, g._f);
            if (g._g != _g)
                return GetResult(_g, g._g);
            if (g._h != _h)
                return GetResult(_h, g._h);
            if (g._i != _i)
                return GetResult(_i, g._i);
            if (g._j != _j)
                return GetResult(_j, g._j);
            if (g._k != _k)
                return GetResult(_k, g._k);
            return 0;
        }

        public int CompareTo(Guid value)
        {
            if (value._a != _a)
                return GetResult((uint)_a, (uint)value._a);
            if (value._b != _b)
                return GetResult((uint)_b, (uint)value._b);
            if (value._c != _c)
                return GetResult((uint)_c, (uint)value._c);
            if (value._d != _d)
                return GetResult(_d, value._d);
            if (value._e != _e)
                return GetResult(_e, value._e);
            if (value._f != _f)
                return GetResult(_f, value._f);
            if (value._g != _g)
                return GetResult(_g, value._g);
            if (value._h != _h)
                return GetResult(_h, value._h);
            if (value._i != _i)
                return GetResult(_i, value._i);
            if (value._j != _j)
                return GetResult(_j, value._j);
            if (value._k != _k)
                return GetResult(_k, value._k);
            return 0;
        }

        public static bool operator ==(Guid a, Guid b)
        {
            if (a._a != b._a)
                return false;
            if (a._b != b._b)
                return false;
            if (a._c != b._c)
                return false;
            if (a._d != b._d)
                return false;
            if (a._e != b._e)
                return false;
            if (a._f != b._f)
                return false;
            if (a._g != b._g)
                return false;
            if (a._h != b._h)
                return false;
            if (a._i != b._i)
                return false;
            if (a._j != b._j)
                return false;
            if (a._k != b._k)
                return false;

            return true;
        }

        public static bool operator !=(Guid a, Guid b)
        {
            return !(a == b);
        }

        [SecuritySafeCritical]
        public static Guid NewGuid()
        {
            Contract.Ensures(Contract.Result<Guid>() != Empty);

            Guid guid;
            Marshal.ThrowExceptionForHR(Win32Native.CoCreateGuid(out guid), new IntPtr(-1));
            return guid;
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        private static char HexToChar(int a)
        {
            a = a & 0xf;
            return (char)(a > 9 ? a - 10 + 0x61 : a + 0x30);
        }

        [SecurityCritical]
        private static unsafe int HexsToChars(char* guidChars, int offset, int a, int b, bool hex = false)
        {
            if (hex)
            {
                guidChars[offset++] = '0';
                guidChars[offset++] = 'x';
            }
            guidChars[offset++] = HexToChar(a >> 4);
            guidChars[offset++] = HexToChar(a);
            if (hex)
            {
                guidChars[offset++] = ',';
                guidChars[offset++] = '0';
                guidChars[offset++] = 'x';
            }
            guidChars[offset++] = HexToChar(b >> 4);
            guidChars[offset++] = HexToChar(b);
            return offset;
        }

        [SecuritySafeCritical]
        public string ToString(string format, IFormatProvider provider)
        {
            if (format == null || format.Length == 0)
                format = "D";

            int offset = 0;
            bool dash = true;
            bool hex = false;

            if (format.Length != 1)
                throw new FormatException(__Resources.GetResourceString(__Resources.Format_InvalidGuidFormatSpecification));

            string guidString;
            char formatCh = format[0];
            if (formatCh == 'D' || formatCh == 'd')
                guidString = string.FastAllocateString(36);
            else if (formatCh == 'N' || formatCh == 'n')
            {
                guidString = string.FastAllocateString(32);
                dash = false;
            }
            else if (formatCh == 'B' || formatCh == 'b')
            {
                guidString = string.FastAllocateString(38);
                unsafe
                {
                    fixed (char* guidChars = guidString)
                    {
                        guidChars[offset++] = '{';
                        guidChars[37] = '}';
                    }
                }
            }
            else if (formatCh == 'P' || formatCh == 'p')
            {
                guidString = string.FastAllocateString(38);
                unsafe
                {
                    fixed (char* guidChars = guidString)
                    {
                        guidChars[offset++] = '(';
                        guidChars[37] = ')';
                    }
                }
            }
            else if (formatCh == 'X' || formatCh == 'x')
            {
                guidString = string.FastAllocateString(68);
                unsafe
                {
                    fixed (char* guidChars = guidString)
                    {
                        guidChars[offset++] = '{';
                        guidChars[67] = '}';
                    }
                }
                dash = false;
                hex = true;
            }
            else
            {
                throw new FormatException(__Resources.GetResourceString(__Resources.Format_InvalidGuidFormatSpecification));
            }

            unsafe
            {
                fixed (char* guidChars = guidString)
                {
                    if (hex)
                    {
                        guidChars[offset++] = '0';
                        guidChars[offset++] = 'x';
                        offset = HexsToChars(guidChars, offset, _a >> 24, _a >> 16);
                        offset = HexsToChars(guidChars, offset, _a >> 8, _a);
                        guidChars[offset++] = ',';
                        guidChars[offset++] = '0';
                        guidChars[offset++] = 'x';
                        offset = HexsToChars(guidChars, offset, _b >> 8, _b);
                        guidChars[offset++] = ',';
                        guidChars[offset++] = '0';
                        guidChars[offset++] = 'x';
                        offset = HexsToChars(guidChars, offset, _c >> 8, _c);
                        guidChars[offset++] = ',';
                        guidChars[offset++] = '{';
                        offset = HexsToChars(guidChars, offset, _d, _e, true);
                        guidChars[offset++] = ',';
                        offset = HexsToChars(guidChars, offset, _f, _g, true);
                        guidChars[offset++] = ',';
                        offset = HexsToChars(guidChars, offset, _h, _i, true);
                        guidChars[offset++] = ',';
                        offset = HexsToChars(guidChars, offset, _j, _k, true);
                        guidChars[offset++] = '}';
                    }
                    else
                    {
                        offset = HexsToChars(guidChars, offset, _a >> 24, _a >> 16);
                        offset = HexsToChars(guidChars, offset, _a >> 8, _a);
                        if (dash)
                            guidChars[offset++] = '-';
                        offset = HexsToChars(guidChars, offset, _b >> 8, _b);
                        if (dash)
                            guidChars[offset++] = '-';
                        offset = HexsToChars(guidChars, offset, _c >> 8, _c);
                        if (dash)
                            guidChars[offset++] = '-';
                        offset = HexsToChars(guidChars, offset, _d, _e);
                        if (dash)
                            guidChars[offset++] = '-';
                        offset = HexsToChars(guidChars, offset, _f, _g);
                        offset = HexsToChars(guidChars, offset, _h, _i);
                        offset = HexsToChars(guidChars, offset, _j, _k);
                    }
                }
            }
            return guidString;
        }

        private static bool TryParseGuid(string g, GuidStyles flags, ref GuidResult result)
        {
            if (g == null)
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidUnrecognized);
                return false;
            }

            string guidString = g.Trim();
            if (guidString.Length == 0)
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidUnrecognized);
                return false;
            }

            bool dashesExistInString = (guidString.IndexOf('-', 0) >= 0);
            if (dashesExistInString)
            {
                if ((flags & (GuidStyles.AllowDashes | GuidStyles.RequireDashes)) == 0)
                {
                    result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidUnrecognized);
                    return false;
                }
            }
            else
            {
                if ((flags & GuidStyles.RequireDashes) != 0)
                {
                    result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidUnrecognized);
                    return false;
                }
            }

            bool bracesExistInString = (guidString.IndexOf('{', 0) >= 0);
            if (bracesExistInString)
            {
                if ((flags & (GuidStyles.AllowBraces | GuidStyles.RequireBraces)) == 0)
                {
                    result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidUnrecognized);
                    return false;
                }
            }
            else
            {
                if ((flags & GuidStyles.RequireBraces) != 0)
                {
                    result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidUnrecognized);
                    return false;
                }
            }

            bool parenthesisExistInString = (guidString.IndexOf('(', 0) >= 0);
            if (parenthesisExistInString)
            {
                if ((flags & (GuidStyles.AllowParenthesis | GuidStyles.RequireParenthesis)) == 0)
                {
                    result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidUnrecognized);
                    return false;
                }
            }
            else
            {
                if ((flags & GuidStyles.RequireParenthesis) != 0)
                {
                    result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidUnrecognized);
                    return false;
                }
            }

            try
            {
                if (dashesExistInString)
                    return TryParseGuidWithDashes(guidString, ref result);
                if (bracesExistInString)
                    return TryParseGuidWithHexPrefix(guidString, ref result);
                return TryParseGuidWithNoStyle(guidString, ref result);
            }
            catch (IndexOutOfRangeException ex)
            {
                result.SetFailure(ParseFailureKind.FormatWithInnerException, __Resources.Format_GuidUnrecognized, null, null, ex);
                return false;
            }
            catch (ArgumentException ex)
            {
                result.SetFailure(ParseFailureKind.FormatWithInnerException, __Resources.Format_GuidUnrecognized, null, null, ex);
                return false;
            }
        }

        private static bool TryParseGuidWithHexPrefix(string guidString, ref GuidResult result)
        {
            guidString = EatAllWhitespace(guidString);
            if (string.IsNullOrEmpty(guidString) || guidString[0] != '{')
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidBrace);
                return false;
            }

            if (!IsHexPrefix(guidString, 1))
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidHexPrefix, "{0xdddddddd, etc}");
                return false;
            }

            int numStart = 3;
            int numLen = guidString.IndexOf(',', numStart) - numStart;
            if (numLen <= 0)
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidComma);
                return false;
            }

            int parsePos = 0;
            if (!StringToInt(guidString.Substring(numStart, numLen), ref parsePos, -1, ParseNumbers.IsTight, out result.parsedGuid._a, ref result))
                return false;

            if (!IsHexPrefix(guidString, numStart + numLen + 1))
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidHexPrefix, "{0xdddddddd, 0xdddd, etc}");
                return false;
            }

            numStart = numStart + numLen + 3;
            numLen = guidString.IndexOf(',', numStart) - numStart;
            if (numLen <= 0)
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidComma);
                return false;
            }

            parsePos = 0;
            if (!StringToShort(guidString.Substring(numStart, numLen), ref parsePos, -1, ParseNumbers.IsTight, out result.parsedGuid._b, ref result))
                return false;

            if (!IsHexPrefix(guidString, numStart + numLen + 1))
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidHexPrefix, "{0xdddddddd, 0xdddd, 0xdddd, etc}");
                return false;
            }

            numStart = numStart + numLen + 3;
            numLen = guidString.IndexOf(',', numStart) - numStart;
            if (numLen <= 0)
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidComma);
                return false;
            }

            parsePos = 0;
            if (!StringToShort(guidString.Substring(numStart, numLen), ref parsePos, -1, ParseNumbers.IsTight, out result.parsedGuid._c, ref result))
                return false;

            if (guidString.Length <= numStart + numLen + 1 || guidString[numStart + numLen + 1] != '{')
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidBrace);
                return false;
            }

            numLen++;
            byte[] bytes = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                if (!IsHexPrefix(guidString, numStart + numLen + 1))
                {
                    result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidHexPrefix, "{... { ... 0xdd, ...}}");
                    return false;
                }

                numStart = numStart + numLen + 3;
                if (i < 7)
                {
                    numLen = guidString.IndexOf(',', numStart) - numStart;
                    if (numLen <= 0)
                    {
                        result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidComma);
                        return false;
                    }
                }
                else
                {
                    numLen = guidString.IndexOf('}', numStart) - numStart;
                    if (numLen <= 0)
                    {
                        result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidBraceAfterLastNumber);
                        return false;
                    }
                }

                uint number = (uint)Convert.ToInt32(guidString.Substring(numStart, numLen), 16);
                if (number > 255)
                {
                    result.SetFailure(ParseFailureKind.Format, __Resources.Overflow_Byte);
                    return false;
                }
                bytes[i] = (byte)number;
            }

            result.parsedGuid._d = bytes[0];
            result.parsedGuid._e = bytes[1];
            result.parsedGuid._f = bytes[2];
            result.parsedGuid._g = bytes[3];
            result.parsedGuid._h = bytes[4];
            result.parsedGuid._i = bytes[5];
            result.parsedGuid._j = bytes[6];
            result.parsedGuid._k = bytes[7];

            if (numStart + numLen + 1 >= guidString.Length || guidString[numStart + numLen + 1] != '}')
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidEndBrace);
                return false;
            }

            if (numStart + numLen + 1 != guidString.Length - 1)
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_ExtraJunkAtEnd);
                return false;
            }

            return true;
        }

        private static bool TryParseGuidWithNoStyle(string guidString, ref GuidResult result)
        {
            if (guidString.Length != 32)
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidInvLen);
                return false;
            }

            for (int i = 0; i < guidString.Length; i++)
            {
                char ch = guidString[i];
                if ((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f'))
                    continue;
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidInvalidChar);
                return false;
            }

            int startPos = 0;
            int parsePos = 0;
            if (!StringToInt(guidString.Substring(startPos, 8), ref parsePos, -1, ParseNumbers.IsTight, out result.parsedGuid._a, ref result))
                return false;

            startPos += 8;
            parsePos = 0;
            if (!StringToShort(guidString.Substring(startPos, 4), ref parsePos, -1, ParseNumbers.IsTight, out result.parsedGuid._b, ref result))
                return false;

            startPos += 4;
            parsePos = 0;
            if (!StringToShort(guidString.Substring(startPos, 4), ref parsePos, -1, ParseNumbers.IsTight, out result.parsedGuid._c, ref result))
                return false;

            startPos += 4;
            parsePos = 0;
            int temp;
            if (!StringToInt(guidString.Substring(startPos, 4), ref parsePos, -1, ParseNumbers.IsTight, out temp, ref result))
                return false;

            startPos += 4;
            int currentPos = startPos;
            long templ;
            if (!StringToLong(guidString, ref currentPos, startPos, out templ, ref result))
                return false;

            if (currentPos - startPos != 12)
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidInvLen);
                return false;
            }

            result.parsedGuid._d = (byte)(temp >> 8);
            result.parsedGuid._e = (byte)(temp);
            temp = (int)(templ >> 32);
            result.parsedGuid._f = (byte)(temp >> 8);
            result.parsedGuid._g = (byte)temp;
            temp = (int)templ;
            result.parsedGuid._h = (byte)(temp >> 24);
            result.parsedGuid._i = (byte)(temp >> 16);
            result.parsedGuid._j = (byte)(temp >> 8);
            result.parsedGuid._k = (byte)temp;

            return true;
        }

        private static bool TryParseGuidWithDashes(string guidString, ref GuidResult result)
        {
            int startPos = 0;
            if (guidString[0] == '{')
            {
                if (guidString.Length != 38 || guidString[37] != '}')
                {
                    result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidInvLen);
                    return false;
                }
                startPos = 1;
            }
            else if (guidString[0] == '(')
            {
                if (guidString.Length != 38 || guidString[37] != ')')
                {
                    result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidInvLen);
                    return false;
                }
                startPos = 1;
            }
            else if (guidString.Length != 36)
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidInvLen);
                return false;
            }

            if (guidString[8 + startPos] != '-' ||
                guidString[13 + startPos] != '-' ||
                guidString[18 + startPos] != '-' ||
                guidString[23 + startPos] != '-')
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidDashes);
                return false;
            }

            int temp;
            int currentPos = startPos;
            if (!StringToInt(guidString, ref currentPos, 8, ParseNumbers.NoSpace, out temp, ref result))
                return false;
            result.parsedGuid._a = temp;
            ++currentPos;

            if (!StringToInt(guidString, ref currentPos, 4, ParseNumbers.NoSpace, out temp, ref result))
                return false;
            result.parsedGuid._b = (short)temp;
            ++currentPos;

            if (!StringToInt(guidString, ref currentPos, 4, ParseNumbers.NoSpace, out temp, ref result))
                return false;
            result.parsedGuid._c = (short)temp;
            ++currentPos;

            if (!StringToInt(guidString, ref currentPos, 4, ParseNumbers.NoSpace, out temp, ref result))
                return false;
            ++currentPos;
            startPos = currentPos;

            long templ;
            if (!StringToLong(guidString, ref currentPos, ParseNumbers.NoSpace, out templ, ref result))
                return false;

            if (currentPos - startPos != 12)
            {
                result.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidInvLen);
                return false;
            }
            result.parsedGuid._d = (byte)(temp >> 8);
            result.parsedGuid._e = (byte)temp;
            temp = (int)(templ >> 32);
            result.parsedGuid._f = (byte)(temp >> 8);
            result.parsedGuid._g = (byte)temp;
            temp = (int)templ;
            result.parsedGuid._h = (byte)(temp >> 24);
            result.parsedGuid._i = (byte)(temp >> 16);
            result.parsedGuid._j = (byte)(temp >> 8);
            result.parsedGuid._k = (byte)temp;

            return true;
        }

        [SecurityCritical]
        private static unsafe bool StringToShort(string str, ref int parsePos, int requiredLength, int flags, out short result, ref GuidResult parseResult)
        {
            result = 0;
            int x;
            bool retValue = StringToInt(str, ref parsePos, requiredLength, flags, out x, ref parseResult);
            result = (short)x;
            return retValue;
        }

        [SecurityCritical]
        private static bool StringToInt(string str, ref int parsePos, int requiredLength, int flags, out int result, ref GuidResult parseResult)
        {
            int currStart = parsePos;
            result = 0;
            try
            {
                result = ParseNumbers.StringToInt(str, 16, flags, ref parsePos);
            }
            catch (OverflowException ex)
            {
                if (parseResult.throwStyle == GuidParseThrowStyle.All)
                    throw;
                if (parseResult.throwStyle == GuidParseThrowStyle.AllButOverflow)
                    throw new FormatException(__Resources.GetResourceString(__Resources.Format_GuidUnrecognized), ex);
                parseResult.SetFailure(ex);
                return false;
            }
            catch (Exception ex)
            {
                if (parseResult.throwStyle == GuidParseThrowStyle.None)
                {
                    parseResult.SetFailure(ex);
                    return false;
                }
                throw;
            }

            if (requiredLength != -1 && parsePos - currStart != requiredLength)
            {
                parseResult.SetFailure(ParseFailureKind.Format, __Resources.Format_GuidInvalidChar);
                return false;
            }
            return true;
        }

        [SecuritySafeCritical]
        private static bool StringToLong(string str, ref int parsePos, int flags, out long result, ref GuidResult parseResult)
        {
            result = 0;
            try
            {
                result = ParseNumbers.StringToLong(str, 16, flags, ref parsePos);
            }
            catch (OverflowException ex)
            {
                if (parseResult.throwStyle == GuidParseThrowStyle.All)
                    throw;
                if (parseResult.throwStyle == GuidParseThrowStyle.AllButOverflow)
                    throw new FormatException(__Resources.GetResourceString(__Resources.Format_GuidUnrecognized), ex);
                parseResult.SetFailure(ex);
                return false;
            }
            catch (Exception ex)
            {
                if (parseResult.throwStyle == GuidParseThrowStyle.None)
                {
                    parseResult.SetFailure(ex);
                    return false;
                }
                throw;
            }
            return true;
        }

        private static string EatAllWhitespace(string str)
        {
            int newLength = 0;
            char[] chArr = new char[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                char curChar = str[i];
                if (!char.IsWhiteSpace(curChar))
                    chArr[newLength++] = curChar;
            }
            return new string(chArr, 0, newLength);
        }

        private static bool IsHexPrefix(string str, int i)
        {
            return str.Length > i + 1 && str[i] == '0' &&
                   (str[i + 1] == 'x' || str[i + 1] == 'X');
        }
    }
}