using System.Runtime.InteropServices;

namespace System.Globalization
{
    [Serializable]
    [ComVisible(true)]
    public abstract class Calendar: ICloneable
    {
        // TODO: members

        internal const int CAL_GREGORIAN = 1; // Gregorian (localized) calendar
        internal const int CAL_GREGORIAN_US = 2; // Gregorian (U.S.) calendar
        internal const int CAL_JAPAN = 3; // Japanese Emperor Era calendar
        internal const int CAL_TAIWAN = 4; // Taiwan Era calendar
        internal const int CAL_KOREA = 5; // Korean Tangun Era calendar
        internal const int CAL_HIJRI = 6; // Hijri (Arabic Lunar) calendar
        internal const int CAL_THAI = 7; // Thai calendar
        internal const int CAL_HEBREW = 8; // Hebrew (Lunar) calendar
        internal const int CAL_GREGORIAN_ME_FRENCH = 9; // Gregorian Middle East French calendar
        internal const int CAL_GREGORIAN_ARABIC = 10; // Gregorian Arabic calendar
        internal const int CAL_GREGORIAN_XLIT_ENGLISH = 11; // Gregorian Transliterated English calendar
        internal const int CAL_GREGORIAN_XLIT_FRENCH = 12;
        internal const int CAL_JULIAN = 13;
        internal const int CAL_JAPANESELUNISOLAR = 14;
        internal const int CAL_CHINESELUNISOLAR = 15;
        internal const int CAL_SAKA = 16; // reserved to match Office but not implemented in our code
        internal const int CAL_LUNAR_ETO_CHN = 17; // reserved to match Office but not implemented in our code
        internal const int CAL_LUNAR_ETO_KOR = 18; // reserved to match Office but not implemented in our code
        internal const int CAL_LUNAR_ETO_ROKUYOU = 19; // reserved to match Office but not implemented in our code
        internal const int CAL_KOREANLUNISOLAR = 20;
        internal const int CAL_TAIWANLUNISOLAR = 21;
        internal const int CAL_PERSIAN = 22;
        internal const int CAL_UMALQURA = 23;

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}