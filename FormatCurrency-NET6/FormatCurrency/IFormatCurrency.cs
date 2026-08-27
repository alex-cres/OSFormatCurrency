using OutSystems.ExternalLibraries.SDK;

namespace OutSystems.FormatCurrency
{
    [OSInterface(
        Description = "Formats the decimal based on the locale provided and currency symbol from the locale or provided." +
                      "Provides a list of the Locales available. " +
                      "Converts a Locale Decimal String (a decimal that was written in in a certain locale) into a valid Decimal.",
        IconResourceName = "OutSystems.FormatCurrency.resources.logo.png", 
        Name = "FormatCurrency")
    ]

    public interface IFormatCurrency
    {
        /// <summary>
        /// Formats the Decimal based on the Locale provided.
        /// </summary>
        /// <param name="Locale">The Locale of the format, see RFC4646 from GetLocales</param>
        /// <param name="Decimal">The number to format</param>
        /// <param name="HasCurrency">If the number has the currency symbol.</param>
        /// <param name="Currency">The Currency to use, if &quot;&quot; it uses the default currency of the locale</param>
        /// <param name="UseNativeDigits">If it uses the native symbols for 0-9.</param>
        /// <param name="UseChineseExtendedNumbers">If the locale starts with "zh", and this is true, the native numbers will be using the chinese unit system instead of the latim or partial chinese numbers.</param>
        /// <param name="UseFinancialChinese">If the locale starts with "zh", and this is true, it will use financial chinese numeration example:  零 "líng" instead of 〇, normally in financial applications 零 is used.</param>
        /// <param name="FormattedText">The formatted text</param>
        [OSAction(Description = "Formats the Decimal based on the Locale provided.", IconResourceName = "OutSystems.FormatCurrency.resources.logo.png")]
        public void GetCurrencyFormattedByLocale(
            [OSParameter(DataType = OSDataType.Text,    Description = "The Locale of the format, see RFC4646 from GetLocales")] 
            string Locale,
            
            [OSParameter(DataType = OSDataType.Decimal, Description = "The number to format")] 
            decimal Decimal,
            
            [OSParameter(DataType = OSDataType.Boolean, Description = "If the number has the currency symbol.")] 
            bool HasCurrency,
            
            [OSParameter(DataType = OSDataType.Text,    Description = "The Currency to use, if \"\" it uses the default currency of the locale")]
            string Currency,
            
            [OSParameter(DataType = OSDataType.Boolean, Description = "If it uses the native symbols for 0-9.")]
            bool UseNativeDigits,
            
            [OSParameter(DataType = OSDataType.Boolean, Description = "If the locale starts with \"zh\", and this is true, the native numbers will be using the chinese unit system instead of the latim or partial chinese numbers.")]
            bool UseChineseExtendedNumbers,
            
            [OSParameter(DataType = OSDataType.Boolean, Description = "If the locale starts with \"zh\", and this is true, it will use financial chinese numeration example:  零 \"líng\" instead of 〇, normally in financial applications 零 is used.")]
            bool UseFinancialChinese,
            
            [OSParameter(DataType = OSDataType.Text,    Description = "The formatted text")]
            out string FormattedText
        );

        //----------------------------------------------------------------
        /// <summary>
        /// Gets the C# supported Locales and their parametrizations.
        /// </summary>
        /// <param name="ListofLocals">The List of Locales</param>
        [OSAction(Description = "Gets the C# supported Locales and their parametrizations.", IconResourceName = "OutSystems.FormatCurrency.resources.logo.png")]
        public void GetLocales(
            [OSParameter(DataType = OSDataType.InferredFromDotNetType,    Description = "The List of Locales")]
            out List<Locale> ListofLocals
        );

        //----------------------------------------------------------------
        /// <summary>
		/// Converts a Locale Decimal String (a decimal that was written in in a certain locale) into a valid Decimal.
		/// For now &quot;zh&quot; locale that uses the extended numbers and financial numbers will not be converted accordingly.
		/// Error Message Codes and meanings:
		/// 0 - &quot;&quot; - No Error
		/// 1 - &quot;String Empty&quot; - The string was empty
		/// 2 - &quot;Locale Invalid/Not Provided&quot; - No Locale was found
		/// 3 - &quot;FormatException&quot; - an error in the format of the number
		/// 4 - &quot;Other Errors [x]&quot; - other errors - log represented in x
        /// 
		/// </summary>
		/// <param name="InputLocalelDecimalString">The String containing the Locale Decimal to convert into Decimal</param>
		/// <param name="Locale">The Locale that the string was written on. (It will try both native and 0-9 numbers from the locale).</param>
		/// <param name="Currency">The Currency to use, if &quot;&quot; it uses the default currency of the locale</param>
        /// <param name="IsValidDecimal">If the converted Decimal is a valid one, if not then the Error Message will have the reason that the conversion failed.</param>
		/// <param name="ErrorMessageCode">The code of the Error Message.</param>
		/// <param name="ErrorMessage">Error Message in case the converted Decimal is not valid.</param>
		/// <param name="Decimal">The Decimal converted, if the IsValidDecimal is notTrue, then this will be the default value 0.0.</param>
		[OSAction(Description = "Converts a Locale Decimal String (a decimal that was written in in a certain locale) into a valid Decimal. " +
                                "For now \"zh\" locale that uses the extended numbers and financial numbers will not be converted accordingly. " +
                                "Error Message Codes and meanings: \n" +
                                " 0 - &quot;&quot; - No Error \n" +
                                " 1 - &quot;String Empty&quot; - The string was empty\n" +
                                " 2 - &quot;Locale Invalid/Not Provided&quot; - No Locale was found\n" +
                                " 3 - &quot;FormatException&quot; - an error in the format of the number\n" +
                                " 4 - &quot;Other Errors [x]&quot; - other errors - log represented in x",
            IconResourceName = "OutSystems.FormatCurrency.resources.logo.png")]
        public void GetDecimalFromLocaleDecimalString(
            [OSParameter(DataType = OSDataType.Text,    Description = "The String containing the Locale Decimal to convert into Decimal")]
            string InputLocalelDecimalString,
            [OSParameter(DataType = OSDataType.Text,    Description = "The Locale that the string was written on. (It will try both native and 0-9 numbers from the locale).")]
            string Locale,
            [OSParameter(DataType = OSDataType.Text,    Description = "The Currency to use, if &quot;&quot; it uses the default currency of the locale.")]
            string Currency,
            [OSParameter(DataType = OSDataType.Boolean, Description = "If the converted Decimal is a valid one, if not then the Error Message will have the reason that the conversion failed.")]
            out bool IsValidDecimal,
            [OSParameter(DataType = OSDataType.Integer, Description = "The code of the Error Message.")]
            out int ErrorMessageCode,
            [OSParameter(DataType = OSDataType.Text,    Description = "Error Message in case the converted Decimal is not valid.")]
            out string ErrorMessage,
            [OSParameter(DataType = OSDataType.Decimal, Description = "The Decimal converted, if the IsValidDecimal is notTrue, then this will be the default value 0.0.")]
            out decimal Decimal);

    }
}