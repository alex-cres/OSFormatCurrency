using OutSystems.ExternalLibraries.SDK;

namespace OutSystems.FormatCurrency;

[OSInterface(Description = "Formats decimal values as locale-aware currency strings, lists available locales, and parses locale-formatted decimal strings back to decimal. Useful for displaying monetary values in end-user locale and converting locale-formatted user input into numeric types.", IconResourceName = "OutSystems.FormatCurrency.resources.icon.png", Name = "FormatCurrency")]
public interface IFormatCurrency
{
    [OSAction(Description = "Formats a decimal value as a currency string using the specified locale. Supports custom currency symbols, native digit rendering, and Chinese extended/financial numeral systems.", IconResourceName = "OutSystems.FormatCurrency.resources.icon.png")]
    void GetCurrencyFormattedByLocale(
        [OSParameter(Description = "RFC 4646 locale tag (e.g. 'en-US', 'pt-PT', 'zh-CN'). An invalid locale falls back to the invariant culture.")]
        string Locale,
        [OSParameter(Description = "The decimal value to format.")]
        decimal Decimal,
        [OSParameter(Description = "When true, includes the currency symbol in the formatted output.")]
        bool HasCurrency,
        [OSParameter(Description = "Custom currency symbol to use instead of the locale default. Empty string uses the locale's own currency symbol.")]
        string Currency,
        [OSParameter(Description = "When true, replaces 0-9 with the locale's native digit characters.")]
        bool UseNativeDigits,
        [OSParameter(Description = "When true and the locale starts with 'zh', uses the Chinese unit numeral system instead of Arabic or partial Chinese digits. Requires UseNativeDigits to also be true.")]
        bool UseChineseExtendedNumbers,
        [OSParameter(Description = "When true and the locale starts with 'zh', uses financial Chinese numerals (e.g. 零 instead of 〇). Requires UseChineseExtendedNumbers and UseNativeDigits to also be true.")]
        bool UseFinancialChinese,
        [OSParameter(Description = "The formatted currency text.")]
        out string FormattedText);

    [OSAction(Description = "Returns all .NET-supported locales with their currency formatting rules (symbol, separators, patterns, native digits, etc.).", IconResourceName = "OutSystems.FormatCurrency.resources.icon.png")]
    void GetLocales(
        [OSParameter(Description = "The list of locales.")]
        out List<Locale> ListofLocals);

    [OSAction(Description = "Parses a locale-formatted decimal string back into a decimal value. Supports native digit replacement and custom currency symbols. Returns error details when parsing fails.", IconResourceName = "OutSystems.FormatCurrency.resources.icon.png")]
    void GetDecimalFromLocaleDecimalString(
        [OSParameter(Description = "The string containing the locale-formatted decimal to parse.")]
        string InputLocalelDecimalString,
        [OSParameter(Description = "RFC 4646 locale tag that the string was written in. Native digits from the locale are replaced before parsing.")]
        string Locale,
        [OSParameter(Description = "Custom currency symbol to use instead of the locale default during parsing. Empty string uses the locale's own currency symbol.")]
        string Currency,
        [OSParameter(Description = "True when the input string was successfully parsed into a decimal value.")]
        out bool IsValidDecimal,
        [OSParameter(Description = "Error code: 0 = no error, 1 = empty string, 2 = invalid locale, 3 = format error, 4 = other error.")]
        out int ErrorMessageCode,
        [OSParameter(Description = "Error message describing why parsing failed. Empty on success.")]
        out string ErrorMessage,
        [OSParameter(Description = "The parsed decimal value. Defaults to 0 when IsValidDecimal is false.")]
        out decimal Decimal);
}
