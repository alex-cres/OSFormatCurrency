using System.Collections.Generic;
using System.Linq;

namespace FormatCurrency.Tests;

// ── O11 adapter types ─────────────────────────────────────────────────────────
// Mirror the ODC LocaleInfo / ParseDecimalResult / IFormatCurrency surface so
// that all test files are byte-for-byte identical to the ODC test project.

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
    private readonly OutSystems.NssFormatCurrency.CssFormatCurrency _inner = new();

    public string GetCurrencyFormattedByLocale(string locale, decimal value, bool hasCurrency, string currency, bool useNativeDigits, bool useChineseExtendedNumbers, bool useFinancialChinese)
    {
        _inner.MssGetCurrencyFormattedByLocale(locale, value, hasCurrency, currency, useNativeDigits, useChineseExtendedNumbers, useFinancialChinese, out var result);
        return result;
    }

    public List<LocaleInfo> GetLocales()
    {
        _inner.MssGetLocales(out var list);
        return list.Select(r => new LocaleInfo
        {
            Name                     = r.ssName,
            RFC4646                  = r.ssRFC4646,
            CurrencyDecimalDigits    = r.ssCurrencyDecimalDigits,
            CurrencyDecimalSeparator = r.ssCurrencyDecimalSeparator,
            CurrencyGroupSeparator   = r.ssCurrencyGroupSeparator,
            CurrencyGroupSizes       = r.ssCurrencyGroupSizes,
            CurrencyNegativePattern  = r.ssCurrencyNegativePattern,
            CurrencyPositivePattern  = r.ssCurrencyPositivePattern,
            NegativeSign             = r.ssNegativeSign,
            CurrencySymbol           = r.ssCurrencySymbol,
            NativeDigits             = r.ssNativeDigits,
        }).ToList();
    }

    public ParseDecimalResult GetDecimalFromLocaleDecimalString(string inputLocaleDecimalString, string locale, string currency)
    {
        _inner.MssGetDecimalFromLocaleDecimalString(inputLocaleDecimalString, locale, currency, out var valid, out var code, out var msg, out var dec);
        return new ParseDecimalResult
        {
            IsValidDecimal   = valid,
            ErrorMessageCode = code,
            ErrorMessage     = msg,
            Value            = dec,
        };
    }
}

// ── Test data helpers ─────────────────────────────────────────────────────────

internal static partial class TestHelpers
{
}
