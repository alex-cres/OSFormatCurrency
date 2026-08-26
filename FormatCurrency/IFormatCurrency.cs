using OutSystems.ExternalLibraries.SDK;

namespace FormatCurrency;

[OSInterface(Description = "Formats decimal values as locale-aware currency strings, lists available locales, and parses locale-formatted decimal strings back to decimal. Useful for displaying monetary values in end-user locale and converting locale-formatted user input into numeric types.", IconResourceName = "FormatCurrency.resources.icon.png")]
public interface IFormatCurrency
{
    [OSAction(Description = "Formats a decimal value as a currency string using the specified locale. Supports custom currency symbols, native digit rendering, and Chinese extended/financial numeral systems.")]
    string GetCurrencyFormattedByLocale(
        [OSParameter(Description = "RFC 4646 locale tag (e.g. 'en-US', 'pt-PT', 'zh-CN'). An invalid locale falls back to the invariant culture.")]
        string locale,
        [OSParameter(Description = "The decimal value to format.")]
        decimal value,
        [OSParameter(Description = "When true, includes the currency symbol in the formatted output.")]
        bool hasCurrency,
        [OSParameter(Description = "Custom currency symbol to use instead of the locale default. Empty string uses the locale's own currency symbol.")]
        string currency,
        [OSParameter(Description = "When true, replaces 0-9 with the locale's native digit characters.")]
        bool useNativeDigits,
        [OSParameter(Description = "When true and the locale starts with 'zh', uses the Chinese unit numeral system instead of Arabic or partial Chinese digits. Requires useNativeDigits to also be true.")]
        bool useChineseExtendedNumbers,
        [OSParameter(Description = "When true and the locale starts with 'zh', uses financial Chinese numerals (e.g. 零 instead of 〇). Requires useChineseExtendedNumbers and useNativeDigits to also be true.")]
        bool useFinancialChinese);

    [OSAction(Description = "Returns all .NET-supported locales with their currency formatting rules (symbol, separators, patterns, native digits, etc.).")]
    List<LocaleInfo> GetLocales();

    [OSAction(Description = "Parses a locale-formatted decimal string back into a decimal value. Supports native digit replacement and custom currency symbols. Returns error details when parsing fails.")]
    ParseDecimalResult GetDecimalFromLocaleDecimalString(
        [OSParameter(Description = "The string containing the locale-formatted decimal to parse.")]
        string inputLocaleDecimalString,
        [OSParameter(Description = "RFC 4646 locale tag that the string was written in. Native digits from the locale are replaced before parsing.")]
        string locale,
        [OSParameter(Description = "Custom currency symbol to use instead of the locale default during parsing. Empty string uses the locale's own currency symbol.")]
        string currency);
}
