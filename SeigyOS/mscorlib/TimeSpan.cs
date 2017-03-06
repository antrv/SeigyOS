using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public struct TimeSpan: IComparable, IComparable<TimeSpan>, IEquatable<TimeSpan>, IFormattable
    {
        public const long TicksPerMillisecond = 10000;
        private const double MillisecondsPerTick = 1.0 / TicksPerMillisecond;

        public const long TicksPerSecond = TicksPerMillisecond * 1000; // 10,000,000
        private const double SecondsPerTick = 1.0 / TicksPerSecond; // 0.0001

        public const long TicksPerMinute = TicksPerSecond * 60; // 600,000,000
        private const double MinutesPerTick = 1.0 / TicksPerMinute; // 1.6666666666667e-9

        public const long TicksPerHour = TicksPerMinute * 60; // 36,000,000,000
        private const double HoursPerTick = 1.0 / TicksPerHour; // 2.77777777777777778e-11

        public const long TicksPerDay = TicksPerHour * 24; // 864,000,000,000
        private const double DaysPerTick = 1.0 / TicksPerDay; // 1.1574074074074074074e-12

        private const int MillisPerSecond = 1000;
        private const int MillisPerMinute = MillisPerSecond * 60; //     60,000
        private const int MillisPerHour = MillisPerMinute * 60; //  3,600,000
        private const int MillisPerDay = MillisPerHour * 24; // 86,400,000

        internal const long MaxSeconds = long.MaxValue / TicksPerSecond;
        internal const long MinSeconds = long.MinValue / TicksPerSecond;

        internal const long MaxMilliSeconds = long.MaxValue / TicksPerMillisecond;
        internal const long MinMilliSeconds = long.MinValue / TicksPerMillisecond;

        internal const long TicksPerTenthSecond = TicksPerMillisecond * 100;

        public static readonly TimeSpan Zero = new TimeSpan(0);

        public static readonly TimeSpan MaxValue = new TimeSpan(long.MaxValue);
        public static readonly TimeSpan MinValue = new TimeSpan(long.MinValue);

        // internal so that DateTime doesn't have to call an extra get
        // method for some arithmetic operations.
        internal long _ticks;

        public TimeSpan(long ticks)
        {
            _ticks = ticks;
        }

        public TimeSpan(int hours, int minutes, int seconds)
        {
            _ticks = TimeToTicks(hours, minutes, seconds);
        }

        public TimeSpan(int days, int hours, int minutes, int seconds)
            : this(days, hours, minutes, seconds, 0)
        {
        }

        public TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds)
        {
            long totalMilliSeconds = ((long)days * 3600 * 24 + (long)hours * 3600 + (long)minutes * 60 + seconds) * 1000 + milliseconds;
            if (totalMilliSeconds > MaxMilliSeconds || totalMilliSeconds < MinMilliSeconds)
                throw new ArgumentOutOfRangeException(null, __Resources.GetResourceString(__Resources.Overflow_TimeSpanTooLong));
            _ticks = totalMilliSeconds * TicksPerMillisecond;
        }

        public long Ticks => _ticks;
        public int Days => (int)(_ticks / TicksPerDay);
        public int Hours => (int)((_ticks / TicksPerHour) % 24);
        public int Milliseconds => (int)((_ticks / TicksPerMillisecond) % 1000);
        public int Minutes => (int)((_ticks / TicksPerMinute) % 60);
        public int Seconds => (int)((_ticks / TicksPerSecond) % 60);
        public double TotalDays => _ticks * DaysPerTick;
        public double TotalHours => _ticks * HoursPerTick;

        public double TotalMilliseconds
        {
            get
            {
                double temp = _ticks * MillisecondsPerTick;
                if (temp > MaxMilliSeconds)
                    return MaxMilliSeconds;

                if (temp < MinMilliSeconds)
                    return MinMilliSeconds;
                return temp;
            }
        }

        public double TotalMinutes => _ticks * MinutesPerTick;
        public double TotalSeconds => _ticks * SecondsPerTick;

        public TimeSpan Add(TimeSpan ts)
        {
            long result = _ticks + ts._ticks;
            if ((_ticks >> 63 == ts._ticks >> 63) && (_ticks >> 63 != result >> 63))
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_TimeSpanTooLong));
            return new TimeSpan(result);
        }


        // Compares two TimeSpan values, returning an integer that indicates their
        // relationship.
        //
        public static int Compare(TimeSpan t1, TimeSpan t2)
        {
            if (t1._ticks > t2._ticks)
                return 1;
            if (t1._ticks < t2._ticks)
                return -1;
            return 0;
        }

        // Returns a value less than zero if this  object
        public int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (!(value is TimeSpan))
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_MustBeTimeSpan));
            long t = ((TimeSpan)value)._ticks;
            if (_ticks > t)
                return 1;
            if (_ticks < t)
                return -1;
            return 0;
        }

        public int CompareTo(TimeSpan value)
        {
            long t = value._ticks;
            if (_ticks > t)
                return 1;
            if (_ticks < t)
                return -1;
            return 0;
        }

        public static TimeSpan FromDays(double value)
        {
            return Interval(value, MillisPerDay);
        }

        public TimeSpan Duration()
        {
            if (Ticks == MinValue.Ticks)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_Duration));
            Contract.EndContractBlock();
            return new TimeSpan(_ticks >= 0 ? _ticks : -_ticks);
        }

        public override bool Equals(object value)
        {
            if (value is TimeSpan)
                return _ticks == ((TimeSpan)value)._ticks;
            return false;
        }

        public bool Equals(TimeSpan obj)
        {
            return _ticks == obj._ticks;
        }

        public static bool Equals(TimeSpan t1, TimeSpan t2)
        {
            return t1._ticks == t2._ticks;
        }

        public override int GetHashCode()
        {
            return (int)_ticks ^ (int)(_ticks >> 32);
        }

        public static TimeSpan FromHours(double value)
        {
            return Interval(value, MillisPerHour);
        }

        private static TimeSpan Interval(double value, int scale)
        {
            if (double.IsNaN(value))
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Arg_CannotBeNaN));
            Contract.EndContractBlock();
            double tmp = value * scale;
            double millis = tmp + (value >= 0 ? 0.5 : -0.5);
            if ((millis > long.MaxValue / TicksPerMillisecond) || (millis < long.MinValue / TicksPerMillisecond))
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_TimeSpanTooLong));
            return new TimeSpan((long)millis * TicksPerMillisecond);
        }

        public static TimeSpan FromMilliseconds(double value)
        {
            return Interval(value, 1);
        }

        public static TimeSpan FromMinutes(double value)
        {
            return Interval(value, MillisPerMinute);
        }

        public TimeSpan Negate()
        {
            if (Ticks == MinValue.Ticks)
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_NegateTwosCompNum));
            Contract.EndContractBlock();
            return new TimeSpan(-_ticks);
        }

        public static TimeSpan FromSeconds(double value)
        {
            return Interval(value, MillisPerSecond);
        }

        public TimeSpan Subtract(TimeSpan ts)
        {
            long result = _ticks - ts._ticks;
            if ((_ticks >> 63 != ts._ticks >> 63) && (_ticks >> 63 != result >> 63))
                throw new OverflowException(__Resources.GetResourceString(__Resources.Overflow_TimeSpanTooLong));
            return new TimeSpan(result);
        }

        public static TimeSpan FromTicks(long value)
        {
            return new TimeSpan(value);
        }

        internal static long TimeToTicks(int hour, int minute, int second)
        {
            long totalSeconds = (long)hour * 3600 + (long)minute * 60 + (long)second;
            if (totalSeconds > MaxSeconds || totalSeconds < MinSeconds)
                throw new ArgumentOutOfRangeException(null, __Resources.GetResourceString(__Resources.Overflow_TimeSpanTooLong));
            return totalSeconds * TicksPerSecond;
        }

        public static TimeSpan Parse(string s)
        {
            return TimeSpanParse.Parse(s, null);
        }

        public static TimeSpan Parse(string input, IFormatProvider formatProvider)
        {
            return TimeSpanParse.Parse(input, formatProvider);
        }

        public static TimeSpan ParseExact(string input, string format, IFormatProvider formatProvider)
        {
            return TimeSpanParse.ParseExact(input, format, formatProvider, TimeSpanStyles.None);
        }

        public static TimeSpan ParseExact(string input, string[] formats, IFormatProvider formatProvider)
        {
            return TimeSpanParse.ParseExactMultiple(input, formats, formatProvider, TimeSpanStyles.None);
        }

        public static TimeSpan ParseExact(string input, string format, IFormatProvider formatProvider, TimeSpanStyles styles)
        {
            TimeSpanParse.ValidateStyles(styles, "styles");
            return TimeSpanParse.ParseExact(input, format, formatProvider, styles);
        }

        public static TimeSpan ParseExact(string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles styles)
        {
            TimeSpanParse.ValidateStyles(styles, "styles");
            return TimeSpanParse.ParseExactMultiple(input, formats, formatProvider, styles);
        }

        public static bool TryParse(string s, out TimeSpan result)
        {
            return TimeSpanParse.TryParse(s, null, out result);
        }

        public static bool TryParse(string input, IFormatProvider formatProvider, out TimeSpan result)
        {
            return TimeSpanParse.TryParse(input, formatProvider, out result);
        }

        public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, out TimeSpan result)
        {
            return TimeSpanParse.TryParseExact(input, format, formatProvider, TimeSpanStyles.None, out result);
        }

        public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, out TimeSpan result)
        {
            return TimeSpanParse.TryParseExactMultiple(input, formats, formatProvider, TimeSpanStyles.None, out result);
        }

        public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, TimeSpanStyles styles, out TimeSpan result)
        {
            TimeSpanParse.ValidateStyles(styles, "styles");
            return TimeSpanParse.TryParseExact(input, format, formatProvider, styles, out result);
        }

        public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, TimeSpanStyles styles, out TimeSpan result)
        {
            TimeSpanParse.ValidateStyles(styles, "styles");
            return TimeSpanParse.TryParseExactMultiple(input, formats, formatProvider, styles, out result);
        }

        public override string ToString()
        {
            return TimeSpanFormat.Format(this, null, null);
        }

        public string ToString(string format)
        {
            return TimeSpanFormat.Format(this, format, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (LegacyMode)
            {
                return TimeSpanFormat.Format(this, null, null);
            }
            else
            {
                return TimeSpanFormat.Format(this, format, formatProvider);
            }
        }

        public static TimeSpan operator -(TimeSpan t)
        {
            if (t._ticks == MinValue._ticks)
                throw new OverflowException(Environment.GetResourceString("Overflow_NegateTwosCompNum"));
            return new TimeSpan(-t._ticks);
        }

        public static TimeSpan operator -(TimeSpan t1, TimeSpan t2)
        {
            return t1.Subtract(t2);
        }

        public static TimeSpan operator +(TimeSpan t)
        {
            return t;
        }

        public static TimeSpan operator +(TimeSpan t1, TimeSpan t2)
        {
            return t1.Add(t2);
        }

        public static bool operator ==(TimeSpan t1, TimeSpan t2)
        {
            return t1._ticks == t2._ticks;
        }

        public static bool operator !=(TimeSpan t1, TimeSpan t2)
        {
            return t1._ticks != t2._ticks;
        }

        public static bool operator <(TimeSpan t1, TimeSpan t2)
        {
            return t1._ticks < t2._ticks;
        }

        public static bool operator <=(TimeSpan t1, TimeSpan t2)
        {
            return t1._ticks <= t2._ticks;
        }

        public static bool operator >(TimeSpan t1, TimeSpan t2)
        {
            return t1._ticks > t2._ticks;
        }

        public static bool operator >=(TimeSpan t1, TimeSpan t2)
        {
            return t1._ticks >= t2._ticks;
        }

        [SecurityCritical]
        [ResourceExposure(ResourceScope.None)]
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern bool LegacyFormatMode();

        [SecuritySafeCritical]
        private static bool GetLegacyFormatMode()
        {
            if (LegacyFormatMode()) // FCALL to check COMPLUS_TimeSpan_LegacyFormatMode
                return true;
            return CompatibilitySwitches.IsNetFx40TimeSpanLegacyFormatMode;
        }

        private static volatile bool _legacyConfigChecked;
        private static volatile bool _legacyMode;

        private static bool LegacyMode
        {
            get
            {
                if (!_legacyConfigChecked)
                {
                    _legacyMode = GetLegacyFormatMode();
                    _legacyConfigChecked = true;
                }
                return _legacyMode;
            }
        }
    }
}