using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
    public static class Math
    {
        // ReSharper disable once InconsistentNaming
        public const double PI = 3.14159265358979323846;
        public const double E = 2.7182818284590452354;

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Acos(double d);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Asin(double d);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Atan(double d);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Atan2(double y, double x);

        public static decimal Ceiling(decimal d)
        {
            return decimal.Ceiling(d);
        }

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Ceiling(double a);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Cos(double d);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Cosh(double value);

        public static decimal Floor(decimal d)
        {
            return decimal.Floor(d);
        }

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Floor(double d);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Sin(double a);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Tan(double a);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Sinh(double value);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Tanh(double value);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Round(double a);

        public static double Round(double value, int digits)
        {
            const int maxRoundingDigits = 15;
            if (digits < 0 || digits > maxRoundingDigits)
                throw new ArgumentOutOfRangeException(nameof(digits), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_RoundingDigits));
            Contract.EndContractBlock();
            return InternalRound(value, digits, MidpointRounding.ToEven);
        }

        public static double Round(double value, MidpointRounding mode)
        {
            return Round(value, 0, mode);
        }

        public static double Round(double value, int digits, MidpointRounding mode)
        {
            const int maxRoundingDigits = 15;
            if (digits < 0 || digits > maxRoundingDigits)
                throw new ArgumentOutOfRangeException(nameof(digits), __Resources.GetResourceString(__Resources.ArgumentOutOfRange_RoundingDigits));
            if (mode < MidpointRounding.ToEven || mode > MidpointRounding.AwayFromZero)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Argument_InvalidEnumValue, mode, nameof(MidpointRounding)), nameof(mode));
            Contract.EndContractBlock();
            return InternalRound(value, digits, mode);
        }

        public static decimal Round(decimal d)
        {
            return decimal.Round(d, 0);
        }

        public static decimal Round(decimal d, int decimals)
        {
            return decimal.Round(d, decimals);
        }

        public static decimal Round(decimal d, MidpointRounding mode)
        {
            return decimal.Round(d, 0, mode);
        }

        public static decimal Round(decimal d, int decimals, MidpointRounding mode)
        {
            return decimal.Round(d, decimals, mode);
        }

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern unsafe double SplitFractionDouble(double* value);

        public static decimal Truncate(decimal d)
        {
            return decimal.Truncate(d);
        }

        public static double Truncate(double d)
        {
            return InternalTruncate(d);
        }

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Sqrt(double d);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Log(double d);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Log10(double d);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Exp(double d);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Pow(double x, double y);

        // ReSharper disable once InconsistentNaming
        public static double IEEERemainder(double x, double y)
        {
            if (double.IsNaN(x))
                return x;
            if (double.IsNaN(y))
                return y;

            double regularMod = x % y;
            if (double.IsNaN(regularMod))
                return double.NaN;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (regularMod == 0)
            {
                if (double.IsNegative(x))
                    return double.NegativeZero;
            }

            double alternativeResult = regularMod - Abs(y) * Sign(x);
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (Abs(alternativeResult) == Abs(regularMod))
            {
                double divisionResult = x / y;
                double roundedResult = Round(divisionResult);
                if (Abs(roundedResult) > Abs(divisionResult))
                    return alternativeResult;
                return regularMod;
            }
            if (Abs(alternativeResult) < Abs(regularMod))
                return alternativeResult;
            return regularMod;
        }

        [CLSCompliant(false)]
        public static sbyte Abs(sbyte value)
        {
            if (value >= 0)
                return value;
            return AbsHelper(value);
        }

        private static sbyte AbsHelper(sbyte value)
        {
            Contract.Requires(value < 0, "AbsHelper should only be called for negative values! (hack for JIT inlining)");
            if (value == sbyte.MinValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_NegateTwosCompNum));
            Contract.EndContractBlock();
            return (sbyte)-value;
        }

        public static short Abs(short value)
        {
            if (value >= 0)
                return value;
            return AbsHelper(value);
        }

        private static short AbsHelper(short value)
        {
            Contract.Requires(value < 0, "AbsHelper should only be called for negative values! (hack for JIT inlining)");
            if (value == short.MinValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_NegateTwosCompNum));
            Contract.EndContractBlock();
            return (short)-value;
        }

        public static int Abs(int value)
        {
            if (value >= 0)
                return value;
            return AbsHelper(value);
        }

        private static int AbsHelper(int value)
        {
            Contract.Requires(value < 0, "AbsHelper should only be called for negative values! (hack for JIT inlining)");
            if (value == int.MinValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_NegateTwosCompNum));
            Contract.EndContractBlock();
            return -value;
        }

        public static long Abs(long value)
        {
            if (value >= 0)
                return value;
            return AbsHelper(value);
        }

        private static long AbsHelper(long value)
        {
            Contract.Requires(value < 0, "AbsHelper should only be called for negative values! (hack for JIT inlining)");
            if (value == long.MinValue)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_NegateTwosCompNum));
            Contract.EndContractBlock();
            return -value;
        }

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern float Abs(float value);

        [SecuritySafeCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern double Abs(double value);

        public static decimal Abs(decimal value)
        {
            return decimal.Abs(value);
        }

        [CLSCompliant(false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static sbyte Max(sbyte val1, sbyte val2)
        {
            return val1 >= val2 ? val1 : val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static byte Max(byte val1, byte val2)
        {
            return val1 >= val2 ? val1 : val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static short Max(short val1, short val2)
        {
            return val1 >= val2 ? val1 : val2;
        }

        [CLSCompliant(false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static ushort Max(ushort val1, ushort val2)
        {
            return val1 >= val2 ? val1 : val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int Max(int val1, int val2)
        {
            return val1 >= val2 ? val1 : val2;
        }

        [CLSCompliant(false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static uint Max(uint val1, uint val2)
        {
            return val1 >= val2 ? val1 : val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static long Max(long val1, long val2)
        {
            return val1 >= val2 ? val1 : val2;
        }

        [CLSCompliant(false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static ulong Max(ulong val1, ulong val2)
        {
            return val1 >= val2 ? val1 : val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static float Max(float val1, float val2)
        {
            if (val1 > val2)
                return val1;
            if (float.IsNaN(val1))
                return val1;
            return val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static double Max(double val1, double val2)
        {
            if (val1 > val2)
                return val1;
            if (double.IsNaN(val1))
                return val1;
            return val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static decimal Max(decimal val1, decimal val2)
        {
            return decimal.Max(val1, val2);
        }

        [CLSCompliant(false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static sbyte Min(sbyte val1, sbyte val2)
        {
            return val1 <= val2 ? val1 : val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static byte Min(byte val1, byte val2)
        {
            return val1 <= val2 ? val1 : val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static short Min(short val1, short val2)
        {
            return val1 <= val2 ? val1 : val2;
        }

        [CLSCompliant(false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static ushort Min(ushort val1, ushort val2)
        {
            return val1 <= val2 ? val1 : val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static int Min(int val1, int val2)
        {
            return val1 <= val2 ? val1 : val2;
        }

        [CLSCompliant(false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static uint Min(uint val1, uint val2)
        {
            return val1 <= val2 ? val1 : val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static long Min(long val1, long val2)
        {
            return val1 <= val2 ? val1 : val2;
        }

        [CLSCompliant(false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static ulong Min(ulong val1, ulong val2)
        {
            return val1 <= val2 ? val1 : val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static float Min(float val1, float val2)
        {
            if (val1 < val2)
                return val1;
            if (float.IsNaN(val1))
                return val1;
            return val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static double Min(double val1, double val2)
        {
            if (val1 < val2)
                return val1;
            if (double.IsNaN(val1))
                return val1;
            return val2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static decimal Min(decimal val1, decimal val2)
        {
            return decimal.Min(val1, val2);
        }

        public static double Log(double a, double newBase)
        {
            if (double.IsNaN(a))
                return a;
            if (double.IsNaN(newBase))
                return newBase;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (newBase == 1)
                return double.NaN;
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (a != 1 && (newBase == 0 || double.IsPositiveInfinity(newBase)))
                return double.NaN;

            return Log(a) / Log(newBase);
        }

        [CLSCompliant(false)]
        public static int Sign(sbyte value)
        {
            if (value < 0)
                return -1;
            if (value > 0)
                return 1;
            return 0;
        }

        public static int Sign(short value)
        {
            if (value < 0)
                return -1;
            if (value > 0)
                return 1;
            return 0;
        }

        public static int Sign(int value)
        {
            if (value < 0)
                return -1;
            if (value > 0)
                return 1;
            return 0;
        }

        public static int Sign(long value)
        {
            if (value < 0)
                return -1;
            if (value > 0)
                return 1;
            return 0;
        }

        public static int Sign(float value)
        {
            if (value < 0)
                return -1;
            if (value > 0)
                return 1;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (value == 0)
                return 0;
            throw new ArithmeticException(__Resources.GetResourceString(__Resources.Arithmetic_NaN));
        }

        public static int Sign(double value)
        {
            if (value < 0)
                return -1;
            if (value > 0)
                return 1;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (value == 0)
                return 0;
            throw new ArithmeticException(__Resources.GetResourceString(__Resources.Arithmetic_NaN));
        }

        public static int Sign(decimal value)
        {
            if (value < 0)
                return -1;
            if (value > 0)
                return 1;
            return 0;
        }

        public static long BigMul(int a, int b)
        {
            return (long)a * b;
        }

        public static int DivRem(int a, int b, out int result)
        {
            result = a % b;
            return a / b;
        }

        public static long DivRem(long a, long b, out long result)
        {
            result = a % b;
            return a / b;
        }
    }
}