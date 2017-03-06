using System.Runtime.InteropServices;

namespace System.Globalization
{
    [Serializable]
    [ComVisible(true)]
    public enum GregorianCalendarTypes
    {
        Localized = Calendar.CAL_GREGORIAN,
        // ReSharper disable once InconsistentNaming
        USEnglish = Calendar.CAL_GREGORIAN_US,
        MiddleEastFrench = Calendar.CAL_GREGORIAN_ME_FRENCH,
        Arabic = Calendar.CAL_GREGORIAN_ARABIC,
        TransliteratedEnglish = Calendar.CAL_GREGORIAN_XLIT_ENGLISH,
        TransliteratedFrench = Calendar.CAL_GREGORIAN_XLIT_FRENCH,
    }
}