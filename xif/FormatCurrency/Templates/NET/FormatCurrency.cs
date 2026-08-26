using System;
using System.Collections;
using System.Data;
using OutSystems.HubEdition.RuntimePlatform;
using OutSystems.RuntimePublic.Db;

namespace OutSystems.NssFormatCurrency {

	public class CssFormatCurrency: IssFormatCurrency {

		/// <summary>
		/// Formats the Decimal based on the Locale provided.
		/// </summary>
		/// <param name="ssLocale">The Locale of the format, see RFC4646 from GetLocales, if an invalid locale is used, the invariant locale will be used.</param>
		/// <param name="ssDecimal">The number to format
		/// </param>
		/// <param name="ssHasCurrency">If the number has the currency symbol.</param>
		/// <param name="ssCurrency">The Currency to use, if &quot;&quot; it uses the default currency of the locale</param>
		/// <param name="ssUseNativeDigits">If it uses the native symbols for 0-9.</param>
		/// <param name="ssUseChineseExtendedNumbers">If the locale starts with &quot;zh&quot;, and this is true, the native numbers will be using the chinese unit system instead of the arabic or partial chinese numbers.</param>
		/// <param name="ssUseFinancialChinese">If the locale starts with &quot;zh&quot;, and this is true, it will use financial chinese numeration example:  零 &quot;líng&quot; instead of 〇, normally in financial applications 零 is used.</param>
		/// <param name="ssFormattedText">The formatted text</param>
		public void MssGetCurrencyFormattedByLocale(string ssLocale, decimal ssDecimal, bool ssHasCurrency, string ssCurrency, bool ssUseNativeDigits, bool ssUseChineseExtendedNumbers, bool ssUseFinancialChinese, out string ssFormattedText) {
			ssFormattedText = "";
			// TODO: Write implementation for action
		} // MssGetCurrencyFormattedByLocale

		/// <summary>
		/// Gets the C# supported Locales and their parametrizations.
		/// </summary>
		/// <param name="ssListofLocals"></param>
		public void MssGetLocales(out RLLocaleRecordList ssListofLocals) {
			ssListofLocals = new RLLocaleRecordList();
			// TODO: Write implementation for action
		} // MssGetLocales

		/// <summary>
		/// Converts a Locale Decimal String (a decimal that was written in in a certain locale) into a valid Decimal.
		/// 
		/// For now &quot;zh&quot; locale that uses the extended numbers and financial numbers will not be converted.
		/// 
		/// Error Message Codes and meanings:
		/// 0 - &quot;&quot; - No Error
		/// 1 - &quot;String Empty&quot; - The string was empty
		/// 2 - &quot;Locale Invalid/Not Provided&quot; - No Locale was found
		/// 3 - &quot;FormatException [x]&quot; - an error in the format of the number - log represented in x
		/// 4 - &quot;Other Errors [x]&quot; - other errors - log represented in x
		/// </summary>
		/// <param name="ssInputLocalelDecimalString">The String containing the Locale Decimal to convert into Decimal</param>
		/// <param name="ssLocale">The Locale that the string was written on. (It will try both native and 0-9 numbers from the locale).</param>
		/// <param name="ssCurrency">If filled it will use the provided as the currency symbol of the locale instead of the locale&apos;s currency</param>
		/// <param name="ssIsValidDecimal">If the converted Decimal is a valid one, if not then the Error Message will have the reason that the conversion failed.</param>
		/// <param name="ssErrorMessageCode">The code of the Error Message.</param>
		/// <param name="ssErrorMessage">Error Message in case the converted Decimal is not valid.</param>
		/// <param name="ssDecimal">The Decimal converted, if the IsValidDecimal is notTrue, then this will be the default value 0.0.</param>
		public void MssGetDecimalFromLocaleDecimalString(string ssInputLocalelDecimalString, string ssLocale, string ssCurrency, out bool ssIsValidDecimal, out int ssErrorMessageCode, out string ssErrorMessage, out decimal ssDecimal) {
			ssIsValidDecimal = true;
			ssErrorMessageCode = 0;
			ssErrorMessage = "";
			ssDecimal = 0.0m;
			// TODO: Write implementation for action
		} // MssGetDecimalFromLocaleDecimalString

	} // CssFormatCurrency

} // OutSystems.NssFormatCurrency

