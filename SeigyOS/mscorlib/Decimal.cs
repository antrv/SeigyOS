using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using System.Security;
using System.__Helpers;

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    [ComVisible(true)]
    public struct Decimal: IFormattable, IComparable, IConvertible, IDeserializationCallback, IComparable<decimal>, IEquatable<decimal>
    {
        private const int cSignMask = unchecked((int)0x80000000);
        private const byte cDecimalNeg = 0x80;
        private const byte cDecimalAdd = 0x00;
        private const int cScaleMask = 0x00FF0000;
        private const int cScaleShift = 16;
        private const int cMaxInt32Scale = 9;
        private static readonly uint[] _powers10 =
        {
            1,
            10,
            100,
            1000,
            10000,
            100000,
            1000000,
            10000000,
            100000000,
            1000000000
        };
        private const decimal cNearNegativeZero = -0.000000000000000000000000001m;
        private const decimal cNearPositiveZero = +0.000000000000000000000000001m;

        public const decimal Zero = 0m;
        public const decimal One = 1m;
        public const decimal MinusOne = -1m;
        public const decimal MaxValue = 79228162514264337593543950335m;
        public const decimal MinValue = -79228162514264337593543950335m;

        private int flags;
        private int hi;
        private int lo;
        private int mid;

        public Decimal(int value)
        {
            int copy = value;
            if (copy >= 0)
            {
                flags = 0;
            }
            else
            {
                flags = cSignMask;
                copy = -copy;
            }
            lo = copy;
            mid = 0;
            hi = 0;
        }

        [CLSCompliant(false)]
        public Decimal(uint value)
        {
            flags = 0;
            lo = (int)value;
            mid = 0;
            hi = 0;
        }

        public Decimal(long value)
        {
            long copy = value;
            if (copy >= 0)
            {
                flags = 0;
            }
            else
            {
                flags = cSignMask;
                copy = -copy;
            }
            lo = (int)copy;
            mid = (int)(copy >> 32);
            hi = 0;
        }

        [CLSCompliant(false)]
        public Decimal(ulong value)
        {
            flags = 0;
            lo = (int)value;
            mid = (int)(value >> 32);
            hi = 0;
        }

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Decimal(float value);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Decimal(double value);

        internal Decimal(Currency value)
        {
            decimal temp = Currency.ToDecimal(value);
            lo = temp.lo;
            mid = temp.mid;
            hi = temp.hi;
            flags = temp.flags;
        }

        public static long ToOACurrency(decimal value)
        {
            return new Currency(value).ToOACurrency();
        }

        public static decimal FromOACurrency(long cy)
        {
            return Currency.ToDecimal(Currency.FromOACurrency(cy));
        }

        public Decimal(int[] bits)
        {
            lo = 0;
            mid = 0;
            hi = 0;
            flags = 0;
            SetBits(bits);
        }

        private void SetBits(int[] bits)
        {
            if (bits == null)
                throw new ArgumentNullException(nameof(bits));
            Contract.EndContractBlock();
            if (bits.Length == 4)
            {
                int f = bits[3];
                if ((f & ~(cSignMask | cScaleMask)) == 0 && (f & cScaleMask) <= (28 << 16))
                {
                    lo = bits[0];
                    mid = bits[1];
                    hi = bits[2];
                    flags = f;
                    return;
                }
            }
            throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_DecBitCtor));
        }

        public Decimal(int lo, int mid, int hi, bool isNegative, byte scale)
        {
            if (scale > 28)
                throw new ArgumentOutOfRangeException(nameof(scale), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_DecimalScale));
            Contract.EndContractBlock();
            this.lo = lo;
            this.mid = mid;
            this.hi = hi;
            flags = scale << 16;
            if (isNegative)
                flags |= cSignMask;
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            try
            {
                SetBits(GetBits(this));
            }
            catch (ArgumentException e)
            {
                throw new SerializationException(__Resources.GetResourceString(__Resources.Overflow_Decimal), e);
            }
        }

        void IDeserializationCallback.OnDeserialization(object sender)
        {
            try
            {
                SetBits(GetBits(this));
            }
            catch (ArgumentException e)
            {
                throw new SerializationException(__Resources.GetResourceString(__Resources.Overflow_Decimal), e);
            }
        }

        private Decimal(int lo, int mid, int hi, int flags)
        {
            if ((flags & ~(cSignMask | cScaleMask)) == 0 && (flags & cScaleMask) <= (28 << 16))
            {
                this.lo = lo;
                this.mid = mid;
                this.hi = hi;
                this.flags = flags;
                return;
            }
            throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_DecBitCtor));
        }

        internal static decimal Abs(decimal d)
        {
            return new decimal(d.lo, d.mid, d.hi, d.flags & ~cSignMask);
        }

        [SecuritySafeCritical]
        public static decimal Add(decimal d1, decimal d2)
        {
            FCallAddSub(ref d1, ref d2, cDecimalAdd);
            return d1;
        }

        // FCallAddSub adds or subtracts two decimal values.  On return, d1 contains the result
        // of the operation.  Passing in DECIMAL_ADD or DECIMAL_NEG for bSign indicates
        // addition or subtraction, respectively.
        //
        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static extern void FCallAddSub(ref decimal d1, ref decimal d2, byte bSign);

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static extern void FCallAddSubOverflowed(ref decimal d1, ref decimal d2, byte bSign, ref bool overflowed);

        // Rounds a Decimal to an integer value. The Decimal argument is rounded
        // towards positive infinity.
        public static decimal Ceiling(decimal d)
        {
            return (-(decimal.Floor(-d)));
        }

        // Compares two Decimal values, returning an integer that indicates their
        // relationship.
        //
        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int Compare(decimal d1, decimal d2)
        {
            return FCallCompare(ref d1, ref d2);
        }

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        private static extern int FCallCompare(ref decimal d1, ref decimal d2);

        // Compares this object to another object, returning an integer that
        // indicates the relationship.
        // Returns a value less than zero if this  object
        // null is considered to be less than any instance.
        // If object is not of type Decimal, this method throws an ArgumentException.
        //
        [SecuritySafeCritical]
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (!(value is decimal))
                throw new ArgumentException(__Resources.GetResourceString("Arg_MustBeDecimal"));

            decimal other = (decimal)value;
            return FCallCompare(ref this, ref other);
        }

        [SecuritySafeCritical]
        public int CompareTo(decimal value)
        {
            return FCallCompare(ref this, ref value);
        }

        // Divides two Decimal values.
        //
        [SecuritySafeCritical]
        public static decimal Divide(decimal d1, decimal d2)
        {
            FCallDivide(ref d1, ref d2);
            return d1;
        }

        // FCallDivide divides two decimal values.  On return, d1 contains the result
        // of the operation.
        //
        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static extern void FCallDivide(ref decimal d1, ref decimal d2);

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static extern void FCallDivideOverflowed(ref decimal d1, ref decimal d2, ref bool overflowed);


        // Checks if this Decimal is equal to a given object. Returns true
        // if the given object is a boxed Decimal and its value is equal to the
        // value of this Decimal. Returns false otherwise.
        //
        [SecuritySafeCritical]
        public override bool Equals(object value)
        {
            if (value is decimal)
            {
                decimal other = (decimal)value;
                return FCallCompare(ref this, ref other) == 0;
            }
            return false;
        }

        [SecuritySafeCritical]
        public bool Equals(decimal value)
        {
            return FCallCompare(ref this, ref value) == 0;
        }

        // Returns the hash code for this Decimal.
        //
        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern override int GetHashCode();

        // Compares two Decimal values for equality. Returns true if the two
        // Decimal values are equal, or false if they are not equal.
        //
        [SecuritySafeCritical]
        public static bool Equals(decimal d1, decimal d2)
        {
            return FCallCompare(ref d1, ref d2) == 0;
        }

        // Rounds a Decimal to an integer value. The Decimal argument is rounded
        // towards negative infinity.
        //
        [SecuritySafeCritical]
        public static decimal Floor(decimal d)
        {
            FCallFloor(ref d);
            return d;
        }

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static extern void FCallFloor(ref decimal d);

        // Converts this Decimal to a string. The resulting string consists of an
        // optional minus sign ("-") followed to a sequence of digits ("0" - "9"),
        // optionally followed by a decimal point (".") and another sequence of
        // digits.
        //
        [SecuritySafeCritical]
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatDecimal(this, null, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        public string ToString(string format)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatDecimal(this, format, NumberFormatInfo.CurrentInfo);
        }

        [SecuritySafeCritical]
        public string ToString(IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatDecimal(this, null, NumberFormatInfo.GetInstance(provider));
        }

        [SecuritySafeCritical]
        public string ToString(string format, IFormatProvider provider)
        {
            Contract.Ensures(Contract.Result<string>() != null);
            return NumberHelper.FormatDecimal(this, format, NumberFormatInfo.GetInstance(provider));
        }

        public static decimal Parse(string s)
        {
            return NumberHelper.ParseDecimal(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo);
        }

        public static decimal Parse(string s, NumberStyles style)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return NumberHelper.ParseDecimal(s, style, NumberFormatInfo.CurrentInfo);
        }

        public static decimal Parse(string s, IFormatProvider provider)
        {
            return NumberHelper.ParseDecimal(s, NumberStyles.Number, NumberFormatInfo.GetInstance(provider));
        }

        public static decimal Parse(string s, NumberStyles style, IFormatProvider provider)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return NumberHelper.ParseDecimal(s, style, NumberFormatInfo.GetInstance(provider));
        }

        public static bool TryParse(string s, out decimal result)
        {
            return NumberHelper.TryParseDecimal(s, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out decimal result)
        {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return NumberHelper.TryParseDecimal(s, style, NumberFormatInfo.GetInstance(provider), out result);
        }

        public static int[] GetBits(decimal d)
        {
            return new [] { d.lo, d.mid, d.hi, d.flags };
        }

        internal static void GetBytes(decimal d, byte[] buffer)
        {
            Contract.Requires((buffer != null && buffer.Length >= 16), "[GetBytes]buffer != null && buffer.Length >= 16");
            buffer[0] = (byte)d.lo;
            buffer[1] = (byte)(d.lo >> 8);
            buffer[2] = (byte)(d.lo >> 16);
            buffer[3] = (byte)(d.lo >> 24);

            buffer[4] = (byte)d.mid;
            buffer[5] = (byte)(d.mid >> 8);
            buffer[6] = (byte)(d.mid >> 16);
            buffer[7] = (byte)(d.mid >> 24);

            buffer[8] = (byte)d.hi;
            buffer[9] = (byte)(d.hi >> 8);
            buffer[10] = (byte)(d.hi >> 16);
            buffer[11] = (byte)(d.hi >> 24);

            buffer[12] = (byte)d.flags;
            buffer[13] = (byte)(d.flags >> 8);
            buffer[14] = (byte)(d.flags >> 16);
            buffer[15] = (byte)(d.flags >> 24);
        }

        internal static decimal ToDecimal(byte[] buffer)
        {
            Contract.Requires((buffer != null && buffer.Length >= 16), "[ToDecimal]buffer != null && buffer.Length >= 16");
            int lo = buffer[0] | (buffer[1] << 8) | (buffer[2] << 16) | (buffer[3] << 24);
            int mid = buffer[4] | (buffer[5] << 8) | (buffer[6] << 16) | (buffer[7] << 24);
            int hi = buffer[8] | (buffer[9] << 8) | (buffer[10] << 16) | (buffer[11] << 24);
            int flags = buffer[12] | (buffer[13] << 8) | (buffer[14] << 16) | (buffer[15] << 24);
            return new decimal(lo, mid, hi, flags);
        }

        private static void InternalAddUInt32RawUnchecked(ref decimal value, uint i)
        {
            uint v = (uint)value.lo;
            uint sum = v + i;
            value.lo = (int)sum;
            if (sum < v || sum < i)
            {
                v = (uint)value.mid;
                sum = v + 1;
                value.mid = (int)sum;
                if (sum < v || sum < 1)
                    value.hi = (int)((uint)value.hi + 1);
            }
        }

        private static uint InternalDivRemUInt32(ref decimal value, uint divisor)
        {
            uint remainder = 0;
            ulong n;
            if (value.hi != 0)
            {
                n = ((uint)value.hi);
                value.hi = (int)((uint)(n / divisor));
                remainder = (uint)(n % divisor);
            }
            if (value.mid != 0 || remainder != 0)
            {
                n = ((ulong)remainder << 32) | (uint)value.mid;
                value.mid = (int)((uint)(n / divisor));
                remainder = (uint)(n % divisor);
            }
            if (value.lo != 0 || remainder != 0)
            {
                n = ((ulong)remainder << 32) | (uint)value.lo;
                value.lo = (int)((uint)(n / divisor));
                remainder = (uint)(n % divisor);
            }
            return remainder;
        }

        private static void InternalRoundFromZero(ref decimal d, int decimalCount)
        {
            int scale = (d.flags & cScaleMask) >> cScaleShift;
            int scaleDifference = scale - decimalCount;
            if (scaleDifference <= 0)
                return;

            uint lastRemainder;
            uint lastDivisor;
            do
            {
                int diffChunk = (scaleDifference > cMaxInt32Scale) ? cMaxInt32Scale : scaleDifference;
                lastDivisor = _powers10[diffChunk];
                lastRemainder = InternalDivRemUInt32(ref d, lastDivisor);
                scaleDifference -= diffChunk;
            }
            while (scaleDifference > 0);
            if (lastRemainder >= (lastDivisor >> 1))
                InternalAddUInt32RawUnchecked(ref d, 1);
            d.flags = ((decimalCount << cScaleShift) & cScaleMask) | (d.flags & cSignMask);
        }

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal static decimal Max(decimal d1, decimal d2)
        {
            return FCallCompare(ref d1, ref d2) >= 0 ? d1 : d2;
        }

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        internal static decimal Min(decimal d1, decimal d2)
        {
            return FCallCompare(ref d1, ref d2) < 0 ? d1 : d2;
        }

        public static decimal Remainder(decimal d1, decimal d2)
        {
            d2.flags = (d2.flags & ~cSignMask) | (d1.flags & cSignMask);
            if (Abs(d1) < Abs(d2))
                return d1;
            d1 -= d2;

            if (d1 == 0)
                d1.flags = (d1.flags & ~cSignMask) | (d2.flags & cSignMask);

            decimal dividedResult = Truncate(d1 / d2);
            decimal multipliedResult = dividedResult * d2;
            decimal result = d1 - multipliedResult;
            if ((d1.flags & cSignMask) != (result.flags & cSignMask))
            {
                if (cNearNegativeZero <= result && result <= cNearPositiveZero)
                    result.flags = (result.flags & ~cSignMask) | (d1.flags & cSignMask);
                else
                    result += d2;
            }
            return result;
        }

        [SecuritySafeCritical]
        public static decimal Multiply(decimal d1, decimal d2)
        {
            FCallMultiply(ref d1, ref d2);
            return d1;
        }

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static extern void FCallMultiply(ref decimal d1, ref decimal d2);

        public static decimal Negate(decimal d)
        {
            return new decimal(d.lo, d.mid, d.hi, d.flags ^ cSignMask);
        }

        public static decimal Round(decimal d)
        {
            return Round(d, 0);
        }

        [SecuritySafeCritical]
        public static decimal Round(decimal d, int decimals)
        {
            FCallRound(ref d, decimals);
            return d;
        }

        public static decimal Round(decimal d, MidpointRounding mode)
        {
            return Round(d, 0, mode);
        }

        [SecuritySafeCritical]
        public static decimal Round(decimal d, int decimals, MidpointRounding mode)
        {
            if (decimals < 0 || decimals > 28)
                throw new ArgumentOutOfRangeException(nameof(decimals), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_DecimalRound));
            if (mode < MidpointRounding.ToEven || mode > MidpointRounding.AwayFromZero)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Argument_InvalidEnumValue, mode, "MidpointRounding"), nameof(mode));
            Contract.EndContractBlock();

            if (mode == MidpointRounding.ToEven)
                FCallRound(ref d, decimals);
            else
                InternalRoundFromZero(ref d, decimals);
            return d;
        }

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void FCallRound(ref decimal d, int decimals);

        [SecuritySafeCritical]
        public static decimal Subtract(decimal d1, decimal d2)
        {
            FCallAddSub(ref d1, ref d2, cDecimalNeg);
            return d1;
        }

        public static byte ToByte(decimal value)
        {
            uint temp;
            try
            {
                temp = ToUInt32(value);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte), e);
            }
            if (temp > byte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Byte));
            return (byte)temp;

        }

        [CLSCompliant(false)]
        public static sbyte ToSByte(decimal value)
        {
            int temp;
            try
            {
                temp = ToInt32(value);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte), e);
            }
            if (temp < sbyte.MinValue || temp > sbyte.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_SByte));
            return (sbyte)temp;
        }

        public static short ToInt16(decimal value)
        {
            int temp;
            try
            {
                temp = ToInt32(value);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16), e);
            }
            if (temp < short.MinValue || temp > short.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int16));
            return (short)temp;
        }

        [SecuritySafeCritical]
        internal static Currency ToCurrency(decimal d)
        {
            Currency result = new Currency();
            FCallToCurrency(ref result, d);
            return result;
        }

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void FCallToCurrency(ref Currency result, decimal d);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double ToDouble(decimal d);

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int FCallToInt32(decimal d);

        [SecuritySafeCritical]
        public static int ToInt32(decimal d)
        {
            if ((d.flags & cScaleMask) != 0)
                FCallTruncate(ref d);
            if (d.hi == 0 && d.mid == 0)
            {
                int i = d.lo;
                if (d.flags >= 0)
                {
                    if (i >= 0)
                        return i;
                }
                else
                {
                    i = -i;
                    if (i <= 0)
                        return i;
                }
            }
            throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int32));
        }

        [SecuritySafeCritical]
        public static long ToInt64(decimal d)
        {
            if ((d.flags & cScaleMask) != 0)
                FCallTruncate(ref d);
            if (d.hi == 0)
            {
                long l = d.lo & 0xFFFFFFFFL | (long)d.mid << 32;
                if (d.flags >= 0)
                {
                    if (l >= 0)
                        return l;
                }
                else
                {
                    l = -l;
                    if (l <= 0)
                        return l;
                }
            }
            throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Int64));
        }

        [CLSCompliant(false)]
        public static ushort ToUInt16(decimal value)
        {
            uint temp;
            try
            {
                temp = ToUInt32(value);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt16), e);
            }
            if (temp > ushort.MaxValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt16));
            return (ushort)temp;
        }

        [SecuritySafeCritical]
        [CLSCompliant(false)]
        public static uint ToUInt32(decimal d)
        {
            if ((d.flags & cScaleMask) != 0)
                FCallTruncate(ref d);
            if (d.hi == 0 && d.mid == 0)
            {
                uint i = (uint)d.lo;
                if (d.flags >= 0 || i == 0)
                    return i;
            }
            throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt32));
        }

        [SecuritySafeCritical]
        [CLSCompliant(false)]
        public static ulong ToUInt64(decimal d)
        {
            if ((d.flags & cScaleMask) != 0)
                FCallTruncate(ref d);
            if (d.hi == 0)
            {
                ulong l = (uint)d.lo | ((ulong)(uint)d.mid << 32);
                if (d.flags >= 0 || l == 0)
                    return l;
            }
            throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_UInt64));
        }

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern float ToSingle(decimal d);

        [SecuritySafeCritical]
        public static decimal Truncate(decimal d)
        {
            FCallTruncate(ref d);
            return d;
        }

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void FCallTruncate(ref decimal d);

        public static implicit operator decimal(byte value)
        {
            return new decimal(value);
        }

        [CLSCompliant(false)]
        public static implicit operator decimal(sbyte value)
        {
            return new decimal(value);
        }

        public static implicit operator decimal(short value)
        {
            return new decimal(value);
        }

        [CLSCompliant(false)]
        public static implicit operator decimal(ushort value)
        {
            return new decimal(value);
        }

        public static implicit operator decimal(char value)
        {
            return new decimal(value);
        }

        public static implicit operator decimal(int value)
        {
            return new decimal(value);
        }

        [CLSCompliant(false)]
        public static implicit operator decimal(uint value)
        {
            return new decimal(value);
        }

        public static implicit operator decimal(long value)
        {
            return new decimal(value);
        }

        [CLSCompliant(false)]
        public static implicit operator decimal(ulong value)
        {
            return new decimal(value);
        }

        public static explicit operator decimal(float value)
        {
            return new decimal(value);
        }

        public static explicit operator decimal(double value)
        {
            return new decimal(value);
        }

        public static explicit operator byte(decimal value)
        {
            return ToByte(value);
        }

        [CLSCompliant(false)]
        public static explicit operator sbyte(decimal value)
        {
            return ToSByte(value);
        }

        public static explicit operator char(decimal value)
        {
            ushort temp;
            try
            {
                temp = ToUInt16(value);
            }
            catch (OverflowException e)
            {
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Char), e);
            }
            return (char)temp;
        }

        public static explicit operator short(decimal value)
        {
            return ToInt16(value);
        }

        [CLSCompliant(false)]
        public static explicit operator ushort(decimal value)
        {
            return ToUInt16(value);
        }

        public static explicit operator int(decimal value)
        {
            return ToInt32(value);
        }

        [CLSCompliant(false)]
        public static explicit operator uint(decimal value)
        {
            return ToUInt32(value);
        }

        public static explicit operator long(decimal value)
        {
            return ToInt64(value);
        }

        [CLSCompliant(false)]
        public static explicit operator ulong(decimal value)
        {
            return ToUInt64(value);
        }

        public static explicit operator float(decimal value)
        {
            return ToSingle(value);
        }

        public static explicit operator double(decimal value)
        {
            return ToDouble(value);
        }

        public static decimal operator +(decimal d)
        {
            return d;
        }

        public static decimal operator -(decimal d)
        {
            return Negate(d);
        }

        public static decimal operator ++(decimal d)
        {
            return Add(d, One);
        }

        public static decimal operator --(decimal d)
        {
            return Subtract(d, One);
        }

        [SecuritySafeCritical]
        public static decimal operator +(decimal d1, decimal d2)
        {
            FCallAddSub(ref d1, ref d2, cDecimalAdd);
            return d1;
        }

        [SecuritySafeCritical]
        public static decimal operator -(decimal d1, decimal d2)
        {
            FCallAddSub(ref d1, ref d2, cDecimalNeg);
            return d1;
        }

        [SecuritySafeCritical]
        public static decimal operator *(decimal d1, decimal d2)
        {
            FCallMultiply(ref d1, ref d2);
            return d1;
        }

        [SecuritySafeCritical]
        public static decimal operator /(decimal d1, decimal d2)
        {
            FCallDivide(ref d1, ref d2);
            return d1;
        }

        public static decimal operator %(decimal d1, decimal d2)
        {
            return Remainder(d1, d2);
        }

        [SecuritySafeCritical]
        public static bool operator ==(decimal d1, decimal d2)
        {
            return FCallCompare(ref d1, ref d2) == 0;
        }

        [SecuritySafeCritical]
        public static bool operator !=(decimal d1, decimal d2)
        {
            return FCallCompare(ref d1, ref d2) != 0;
        }

        [SecuritySafeCritical]
        public static bool operator <(decimal d1, decimal d2)
        {
            return FCallCompare(ref d1, ref d2) < 0;
        }

        [SecuritySafeCritical]
        public static bool operator <=(decimal d1, decimal d2)
        {
            return FCallCompare(ref d1, ref d2) <= 0;
        }

        [SecuritySafeCritical]
        public static bool operator >(decimal d1, decimal d2)
        {
            return FCallCompare(ref d1, ref d2) > 0;
        }

        [SecuritySafeCritical]
        public static bool operator >=(decimal d1, decimal d2)
        {
            return FCallCompare(ref d1, ref d2) >= 0;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.Decimal;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(this);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Decimal", "Char"));
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(this);
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(this);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(this);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(this);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(this);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(this);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(this);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(this);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(this);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(this);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return this;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException(__Resources.GetResourceString(__Resources.InvalidCast_FromTo, "Decimal", "DateTime"));
        }

        object IConvertible.ToType(Type type, IFormatProvider provider)
        {
            return Convert.DefaultToType(this, type, provider);
        }
    }
}