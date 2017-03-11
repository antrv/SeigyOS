using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Globalization
{
    [Serializable]
    [ComVisible(true)]
    public sealed class NumberFormatInfo : ICloneable, IFormatProvider
    {
        private static volatile NumberFormatInfo _invariantInfo;

        // READTHIS READTHIS READTHIS
        // This class has an exact mapping onto a native structure defined in COMNumber.cpp
        // DO NOT UPDATE THIS WITHOUT UPDATING THAT STRUCTURE. IF YOU ADD BOOL, ADD THEM AT THE END.
        // ALSO MAKE SURE TO UPDATE mscorlib.h in the VM directory to check field offsets.
        // READTHIS READTHIS READTHIS
        internal int[] numberGroupSizes = new int[] { 3 };
        internal int[] currencyGroupSizes = new int[] { 3 };
        internal int[] percentGroupSizes = new int[] { 3 };
        internal string positiveSign = "+";
        internal string negativeSign = "-";
        internal string numberDecimalSeparator = ".";
        internal string numberGroupSeparator = ",";
        internal string currencyGroupSeparator = ",";
        internal string _currencyDecimalSeparator = ".";
        internal string currencySymbol = "\x00a4";  // U+00a4 is the symbol for International Monetary Fund.
        // The alternative currency symbol used in Win9x ANSI codepage, that can not roundtrip between ANSI and Unicode.
        // Currently, only ja-JP and ko-KR has non-null values (which is U+005c, backslash)
        // NOTE: The only legal values for this string are null and "\x005c"
        internal string ansiCurrencySymbol = null;
        internal string nanSymbol = "NaN";
        internal string positiveInfinitySymbol = "Infinity";
        internal string negativeInfinitySymbol = "-Infinity";
        internal string percentDecimalSeparator = ".";
        internal string percentGroupSeparator = ",";
        internal string percentSymbol = "%";
        internal string perMilleSymbol = "\u2030";

        [OptionalField(VersionAdded = 2)]
        internal string[] nativeDigits = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        // an index which points to a record in Culture Data Table.
        // We shouldn't be persisting dataItem (since its useless & we weren't using it),
        // but since COMNumber.cpp uses it and since serialization isn't implimented, its stuck for now.
        [OptionalField(VersionAdded = 1)]
        internal int m_dataItem = 0;    // NEVER USED, DO NOT USE THIS! (Serialized in Everett)

        internal int numberDecimalDigits = 2;
        internal int _currencyDecimalDigits = 2;
        internal int currencyPositivePattern = 0;
        internal int currencyNegativePattern = 0;
        internal int numberNegativePattern = 1;
        internal int percentPositivePattern = 0;
        internal int percentNegativePattern = 0;
        internal int percentDecimalDigits = 2;

        [OptionalField(VersionAdded = 2)]
        internal int digitSubstitution = 1; // DigitShapes.None

        internal bool _isReadOnly = false;
        // We shouldn't be persisting m_useUserOverride (since its useless & we weren't using it),
        // but since COMNumber.cpp uses it and since serialization isn't implimented, its stuck for now.
        [OptionalField(VersionAdded = 1)]
        internal bool m_useUserOverride = false;    // NEVER USED, DO NOT USE THIS! (Serialized in Everett)

        // Is this NumberFormatInfo for invariant culture?
        [OptionalField(VersionAdded = 2)]
        internal bool _isInvariant = false;

        public NumberFormatInfo()
        {
        }

        private static void VerifyDecimalSeparator(string decSep, string propertyName)
        {
            if (decSep == null)
                throw new ArgumentNullException(propertyName, __Resources.GetResourceString(__Resources.ArgumentNull_String));
            if (decSep.Length == 0)
                throw new ArgumentException(__Resources.GetResourceString(__Resources.Argument_EmptyDecString));
            Contract.EndContractBlock();
        }

        private static void VerifyGroupSeparator(string groupSep, string propertyName)
        {
            if (groupSep == null)
                throw new ArgumentNullException(propertyName, __Resources.GetResourceString(__Resources.ArgumentNull_String));
            Contract.EndContractBlock();
        }

        private static void VerifyNativeDigits(string[] nativeDig, string propertyName)
        {
            if (nativeDig == null)
                throw new ArgumentNullException(propertyName, Environment.GetResourceString("ArgumentNull_Array"));
            if (nativeDig.Length != 10)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNativeDigitCount"), propertyName);
            }
            Contract.EndContractBlock();

            for (int i = 0; i < nativeDig.Length; i++)
            {
                if (nativeDig[i] == null)
                {
                    throw new ArgumentNullException(propertyName,
                        Environment.GetResourceString("ArgumentNull_ArrayValue"));
                }


                if (nativeDig[i].Length != 1)
                {
                    if (nativeDig[i].Length != 2)
                    {
                        // Not 1 or 2 UTF-16 code points
                        throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNativeDigitValue"), propertyName);
                    }
                    else if (!char.IsSurrogatePair(nativeDig[i][0], nativeDig[i][1]))
                    {
                        // 2 UTF-6 code points, but not a surrogate pair
                        throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNativeDigitValue"), propertyName);
                    }
                }

                if (CharUnicodeInfo.GetDecimalDigitValue(nativeDig[i], 0) != i &&
                    CharUnicodeInfo.GetUnicodeCategory(nativeDig[i], 0) != UnicodeCategory.PrivateUse)
                {
                    // Not the appropriate digit according to the Unicode data properties
                    // (Digit 0 must be a 0, etc.).
                    throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNativeDigitValue"), propertyName);
                }
            }
        }

        private static void VerifyDigitSubstitution(DigitShapes digitSub, string propertyName)
        {
            switch (digitSub)
            {
                case DigitShapes.Context:
                case DigitShapes.None:
                case DigitShapes.NativeNational:
                    break;

                default:
                    throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDigitSubstitution"), propertyName);
            }
        }

        // We aren't persisting dataItem any more (since its useless & we weren't using it),
        // Ditto with m_useUserOverride.  Don't use them, we use a local copy of everything.
        [System.Security.SecuritySafeCritical]  // auto-generated
        internal NumberFormatInfo(CultureData cultureData)
        {
            if (cultureData != null)
            {
                // We directly use fields here since these data is coming from data table or Win32, so we
                // don't need to verify their values (except for invalid parsing situations).
                cultureData.GetNFIValues(this);

                if (cultureData.IsInvariantCulture)
                {
                    // For invariant culture
                    this._isInvariant = true;
                }
            }
        }

        [Pure]
        private void VerifyWritable()
        {
            if (_isReadOnly)
                throw new InvalidOperationException(__Resources.GetResourceString(__Resources.InvalidOperation_ReadOnly));
            Contract.EndContractBlock();
        }

        public static NumberFormatInfo InvariantInfo
        {
            get
            {
                if (_invariantInfo == null)
                {
                    NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
                    numberFormatInfo._isInvariant = true;
                    _invariantInfo = ReadOnly(numberFormatInfo);
                }
                return _invariantInfo;
            }
        }

        public static NumberFormatInfo GetInstance(IFormatProvider formatProvider)
        {
            // Fast case for a regular CultureInfo
            NumberFormatInfo info;
            CultureInfo cultureProvider = formatProvider as CultureInfo;
            if (cultureProvider != null && !cultureProvider.m_isInherited)
            {
                info = cultureProvider.numInfo;
                if (info != null)
                {
                    return info;
                }
                else
                {
                    return cultureProvider.NumberFormat;
                }
            }
            // Fast case for an NFI;
            info = formatProvider as NumberFormatInfo;
            if (info != null)
            {
                return info;
            }
            if (formatProvider != null)
            {
                info = formatProvider.GetFormat(typeof(NumberFormatInfo)) as NumberFormatInfo;
                if (info != null)
                {
                    return info;
                }
            }
            return CurrentInfo;
        }

        public object Clone()
        {
            NumberFormatInfo numberFormatInfo = (NumberFormatInfo)MemberwiseClone();
            numberFormatInfo._isReadOnly = false;
            return numberFormatInfo;
        }

        public int CurrencyDecimalDigits
        {
            get
            {
                return _currencyDecimalDigits;
            }
            set
            {
                if (value < 0 || value > 99)
                    throw new ArgumentOutOfRangeException(nameof(CurrencyDecimalDigits),
                        string.Format(CultureInfo.CurrentCulture, __Resources.GetResourceString(__Resources.ArgumentOutOfRange_Range), 0, 99));
                Contract.EndContractBlock();
                VerifyWritable();
                _currencyDecimalDigits = value;
            }
        }

        public string CurrencyDecimalSeparator
        {
            get
            {
                return _currencyDecimalSeparator;
            }
            set
            {
                VerifyWritable();
                VerifyDecimalSeparator(value, nameof(CurrencyDecimalSeparator));
                _currencyDecimalSeparator = value;
            }
        }

        public bool IsReadOnly => _isReadOnly;

        internal static void CheckGroupSize(string propName, int[] groupSize)
        {
            for (int i = 0; i < groupSize.Length; i++)
            {
                if (groupSize[i] < 1)
                {
                    if (i == groupSize.Length - 1 && groupSize[i] == 0)
                        return;
                    throw new ArgumentException(Environment.GetResourceString("Argument_InvalidGroupSize"), propName);
                }
                if (groupSize[i] > 9)
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_InvalidGroupSize"), propName);
                }
            }
        }

        public int[] CurrencyGroupSizes
        {
            get
            {
                return (int[])currencyGroupSizes.Clone();
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("CurrencyGroupSizes", Environment.GetResourceString("ArgumentNull_Obj"));
                }
                Contract.EndContractBlock();
                VerifyWritable();

                int[] inputSizes = (int[])value.Clone();
                CheckGroupSize("CurrencyGroupSizes", inputSizes);
                currencyGroupSizes = inputSizes;
            }

        }



        public int[] NumberGroupSizes
        {
            get
            {
                return (int[])numberGroupSizes.Clone();
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("NumberGroupSizes",
                        Environment.GetResourceString("ArgumentNull_Obj"));
                }
                Contract.EndContractBlock();
                VerifyWritable();

                int[] inputSizes = (int[])value.Clone();
                CheckGroupSize("NumberGroupSizes", inputSizes);
                numberGroupSizes = inputSizes;
            }
        }


        public int[] PercentGroupSizes
        {
            get
            {
                return (int[])percentGroupSizes.Clone();
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("PercentGroupSizes",
                        Environment.GetResourceString("ArgumentNull_Obj"));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                int[] inputSizes = (int[])value.Clone();
                CheckGroupSize("PercentGroupSizes", inputSizes);
                percentGroupSizes = inputSizes;
            }

        }


        public string CurrencyGroupSeparator
        {
            get { return currencyGroupSeparator; }
            set
            {
                VerifyWritable();
                VerifyGroupSeparator(value, "CurrencyGroupSeparator");
                currencyGroupSeparator = value;
            }
        }


        public string CurrencySymbol
        {
            get { return currencySymbol; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("CurrencySymbol",
                        Environment.GetResourceString("ArgumentNull_String"));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                currencySymbol = value;
            }
        }

        // Returns the current culture's NumberFormatInfo.  Used by Parse methods.
        //

        public static NumberFormatInfo CurrentInfo
        {
            get
            {
                System.Globalization.CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (!culture.m_isInherited)
                {
                    NumberFormatInfo info = culture.numInfo;
                    if (info != null)
                    {
                        return info;
                    }
                }
                return (NumberFormatInfo)culture.GetFormat(typeof(NumberFormatInfo));
            }
        }


        public string NaNSymbol
        {
            get
            {
                return nanSymbol;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("NaNSymbol",
                        Environment.GetResourceString("ArgumentNull_String"));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                nanSymbol = value;
            }
        }



        public int CurrencyNegativePattern
        {
            get { return currencyNegativePattern; }
            set
            {
                if (value < 0 || value > 15)
                {
                    throw new ArgumentOutOfRangeException(
                        "CurrencyNegativePattern",
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Environment.GetResourceString("ArgumentOutOfRange_Range"),
                            0,
                            15));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                currencyNegativePattern = value;
            }
        }


        public int NumberNegativePattern
        {
            get { return numberNegativePattern; }
            set
            {
                //
                // NOTENOTE: the range of value should correspond to negNumberFormats[] in vm\COMNumber.cpp.
                //
                if (value < 0 || value > 4)
                {
                    throw new ArgumentOutOfRangeException(
                        "NumberNegativePattern",
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Environment.GetResourceString("ArgumentOutOfRange_Range"),
                            0,
                            4));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                numberNegativePattern = value;
            }
        }


        public int PercentPositivePattern
        {
            get { return percentPositivePattern; }
            set
            {
                //
                // NOTENOTE: the range of value should correspond to posPercentFormats[] in vm\COMNumber.cpp.
                //
                if (value < 0 || value > 3)
                {
                    throw new ArgumentOutOfRangeException(
                        "PercentPositivePattern",
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Environment.GetResourceString("ArgumentOutOfRange_Range"),
                            0,
                            3));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                percentPositivePattern = value;
            }
        }


        public int PercentNegativePattern
        {
            get { return percentNegativePattern; }
            set
            {
                //
                // NOTENOTE: the range of value should correspond to posPercentFormats[] in vm\COMNumber.cpp.
                //
                if (value < 0 || value > 11)
                {
                    throw new ArgumentOutOfRangeException(
                        "PercentNegativePattern",
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Environment.GetResourceString("ArgumentOutOfRange_Range"),
                            0,
                            11));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                percentNegativePattern = value;
            }
        }


        public string NegativeInfinitySymbol
        {
            get
            {
                return negativeInfinitySymbol;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("NegativeInfinitySymbol",
                        Environment.GetResourceString("ArgumentNull_String"));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                negativeInfinitySymbol = value;
            }
        }


        public string NegativeSign
        {
            get { return negativeSign; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("NegativeSign",
                        Environment.GetResourceString("ArgumentNull_String"));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                negativeSign = value;
            }
        }


        public int NumberDecimalDigits
        {
            get { return numberDecimalDigits; }
            set
            {
                if (value < 0 || value > 99)
                {
                    throw new ArgumentOutOfRangeException(
                        "NumberDecimalDigits",
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Environment.GetResourceString("ArgumentOutOfRange_Range"),
                            0,
                            99));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                numberDecimalDigits = value;
            }
        }


        public string NumberDecimalSeparator
        {
            get { return numberDecimalSeparator; }
            set
            {
                VerifyWritable();
                VerifyDecimalSeparator(value, "NumberDecimalSeparator");
                numberDecimalSeparator = value;
            }
        }


        public string NumberGroupSeparator
        {
            get { return numberGroupSeparator; }
            set
            {
                VerifyWritable();
                VerifyGroupSeparator(value, "NumberGroupSeparator");
                numberGroupSeparator = value;
            }
        }


        public int CurrencyPositivePattern
        {
            get { return currencyPositivePattern; }
            set
            {
                if (value < 0 || value > 3)
                {
                    throw new ArgumentOutOfRangeException(
                        "CurrencyPositivePattern",
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Environment.GetResourceString("ArgumentOutOfRange_Range"),
                            0,
                            3));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                currencyPositivePattern = value;
            }
        }


        public string PositiveInfinitySymbol
        {
            get
            {
                return positiveInfinitySymbol;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("PositiveInfinitySymbol",
                        Environment.GetResourceString("ArgumentNull_String"));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                positiveInfinitySymbol = value;
            }
        }


        public string PositiveSign
        {
            get { return positiveSign; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("PositiveSign",
                        Environment.GetResourceString("ArgumentNull_String"));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                positiveSign = value;
            }
        }


        public int PercentDecimalDigits
        {
            get { return percentDecimalDigits; }
            set
            {
                if (value < 0 || value > 99)
                {
                    throw new ArgumentOutOfRangeException(
                        "PercentDecimalDigits",
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Environment.GetResourceString("ArgumentOutOfRange_Range"),
                            0,
                            99));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                percentDecimalDigits = value;
            }
        }


        public string PercentDecimalSeparator
        {
            get { return percentDecimalSeparator; }
            set
            {
                VerifyWritable();
                VerifyDecimalSeparator(value, "PercentDecimalSeparator");
                percentDecimalSeparator = value;
            }
        }


        public string PercentGroupSeparator
        {
            get { return percentGroupSeparator; }
            set
            {
                VerifyWritable();
                VerifyGroupSeparator(value, "PercentGroupSeparator");
                percentGroupSeparator = value;
            }
        }


        public string PercentSymbol
        {
            get
            {
                return percentSymbol;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("PercentSymbol",
                        Environment.GetResourceString("ArgumentNull_String"));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                percentSymbol = value;
            }
        }


        public string PerMilleSymbol
        {
            get { return perMilleSymbol; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("PerMilleSymbol",
                        Environment.GetResourceString("ArgumentNull_String"));
                }
                Contract.EndContractBlock();
                VerifyWritable();
                perMilleSymbol = value;
            }
        }


        [System.Runtime.InteropServices.ComVisible(false)]
        public string[] NativeDigits
        {
            get { return (string[])nativeDigits.Clone(); }
            set
            {
                VerifyWritable();
                VerifyNativeDigits(value, "NativeDigits");
                nativeDigits = value;
            }
        }

#if !FEATURE_CORECLR
        [System.Runtime.InteropServices.ComVisible(false)]
        public DigitShapes DigitSubstitution
        {
            get { return (DigitShapes)digitSubstitution; }
            set
            {
                VerifyWritable();
                VerifyDigitSubstitution(value, "DigitSubstitution");
                digitSubstitution = (int)value;
            }
        }
#endif // !FEATURE_CORECLR

        public object GetFormat(Type formatType)
        {
            return formatType == typeof(NumberFormatInfo) ? this : null;
        }

        public static NumberFormatInfo ReadOnly(NumberFormatInfo nfi)
        {
            if (nfi == null)
            {
                throw new ArgumentNullException("nfi");
            }
            Contract.EndContractBlock();
            if (nfi.IsReadOnly)
            {
                return nfi;
            }
            NumberFormatInfo info = (NumberFormatInfo)nfi.MemberwiseClone();
            info._isReadOnly = true;
            return info;
        }

        // private const NumberStyles InvalidNumberStyles = unchecked((NumberStyles) 0xFFFFFC00);
        private const NumberStyles InvalidNumberStyles = ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite
                                                           | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign
                                                           | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint
                                                           | NumberStyles.AllowThousands | NumberStyles.AllowExponent
                                                           | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier);

        internal static void ValidateParseStyleInteger(NumberStyles style)
        {
            // Check for undefined flags
            if ((style & InvalidNumberStyles) != 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNumberStyles"), "style");
            }
            Contract.EndContractBlock();
            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            { // Check for hex number
                if ((style & ~NumberStyles.HexNumber) != 0)
                {
                    throw new ArgumentException(Environment.GetResourceString("Arg_InvalidHexStyle"));
                }
            }
        }

        internal static void ValidateParseStyleFloatingPoint(NumberStyles style)
        {
            // Check for undefined flags
            if ((style & InvalidNumberStyles) != 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNumberStyles"), "style");
            }
            Contract.EndContractBlock();
            if ((style & NumberStyles.AllowHexSpecifier) != 0)
            { // Check for hex number
                throw new ArgumentException(Environment.GetResourceString("Arg_HexStyleNotSupported"));
            }
        }
    }
}