using System.Linq;

namespace FormatCurrency.Tests;

// ── ODC adapter types ─────────────────────────────────────────────────────────
// Mirror the O11 adapter so all test files are byte-for-byte identical.

internal struct LocaleInfo
{
    public string Name                     { get; set; }
    public string RFC4646                  { get; set; }
    public string CurrencyDecimalDigits    { get; set; }
    public string CurrencyDecimalSeparator { get; set; }
    public string CurrencyGroupSeparator   { get; set; }
    public string CurrencyGroupSizes       { get; set; }
    public string CurrencyNegativePattern  { get; set; }
    public string CurrencyPositivePattern  { get; set; }
    public string NegativeSign             { get; set; }
    public string CurrencySymbol           { get; set; }
    public string NativeDigits             { get; set; }
}

internal struct ParseDecimalResult
{
    public bool    IsValidDecimal   { get; set; }
    public int     ErrorMessageCode { get; set; }
    public string  ErrorMessage     { get; set; }
    public decimal Value            { get; set; }
}

internal interface IFormatCurrency
{
    string GetCurrencyFormattedByLocale(string locale, decimal value, bool hasCurrency, string currency, bool useNativeDigits, bool useChineseExtendedNumbers, bool useFinancialChinese);
    List<LocaleInfo> GetLocales();
    ParseDecimalResult GetDecimalFromLocaleDecimalString(string inputLocaleDecimalString, string locale, string currency);
}

internal sealed class FormatCurrency : IFormatCurrency
{
    private readonly OutSystems.FormatCurrency.FormatCurrency _inner = new();

    public string GetCurrencyFormattedByLocale(string locale, decimal value, bool hasCurrency, string currency, bool useNativeDigits, bool useChineseExtendedNumbers, bool useFinancialChinese)
    {
        _inner.GetCurrencyFormattedByLocale(locale, value, hasCurrency, currency, useNativeDigits, useChineseExtendedNumbers, useFinancialChinese, out var result);
        return result;
    }

    public List<LocaleInfo> GetLocales()
    {
        _inner.GetLocales(out var list);
        return list.Select(l => new LocaleInfo
        {
            Name                     = l.Name,
            RFC4646                  = l.RFC4646,
            CurrencyDecimalDigits    = l.CurrencyDecimalDigits,
            CurrencyDecimalSeparator = l.CurrencyDecimalSeparator,
            CurrencyGroupSeparator   = l.CurrencyGroupSeparator,
            CurrencyGroupSizes       = l.CurrencyGroupSizes,
            CurrencyNegativePattern  = l.CurrencyNegativePattern,
            CurrencyPositivePattern  = l.CurrencyPositivePattern,
            NegativeSign             = l.NegativeSign,
            CurrencySymbol           = l.CurrencySymbol,
            NativeDigits             = l.NativeDigits,
        }).ToList();
    }

    public ParseDecimalResult GetDecimalFromLocaleDecimalString(string inputLocaleDecimalString, string locale, string currency)
    {
        _inner.GetDecimalFromLocaleDecimalString(inputLocaleDecimalString, locale, currency, out var valid, out var code, out var msg, out var dec);
        return new ParseDecimalResult
        {
            IsValidDecimal   = valid,
            ErrorMessageCode = code,
            ErrorMessage     = msg,
            Value            = dec,
        };
    }
}

internal static partial class TestHelpers
{
}
