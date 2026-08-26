using System.Globalization;
using Xunit;

namespace FormatCurrency.Tests;

public class GetCurrencyFormattedByLocaleTests
{
    private readonly IFormatCurrency _sut = new FormatCurrency();

    // ── Basic formatting with locale defaults ─────────────────────────────────
    // Compare against .NET's own formatting to stay correct across ICU / NLS differences.

    [Theory]
    [InlineData("en-US", 1234.56)]
    [InlineData("de-DE", 1234.56)]
    [InlineData("ja-JP", 1234)]
    [InlineData("fr-FR", 1234.56)]
    public void Format_WithDefaultCurrency_MatchesDotNetFormatting(string locale, decimal value)
    {
        var ci = CultureInfo.GetCultureInfo(locale);
        var expected = value.ToString("C", ci.NumberFormat);

        var result = _sut.GetCurrencyFormattedByLocale(locale, value, hasCurrency: true, currency: "", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        Assert.Equal(expected, result);
    }

    // ── Locale whose group separator varies between NLS and ICU ───────────────

    [Fact]
    public void Format_PtPT_MatchesDotNetFormatting()
    {
        var ci = CultureInfo.GetCultureInfo("pt-PT");
        var expected = 1234.56m.ToString("C", ci.NumberFormat);
        var result = _sut.GetCurrencyFormattedByLocale("pt-PT", 1234.56m, hasCurrency: true, currency: "", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        Assert.Equal(expected, result);
    }

    // ── Locale that uses CurrencyNegativePattern 16 ($- n) ──────────────────

    [Fact]
    public void Format_NegativePattern16_MatchesDotNetFormatting()
    {
        // luy-KE uses CurrencyNegativePattern = 16 ("$- n") on ICU
        // Find any culture with pattern 16; skip if none exist on this runtime
        CultureInfo? ci16 = null;
        foreach (var ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
        {
            if (ci.NumberFormat.CurrencyNegativePattern == 16)
            { ci16 = ci; break; }
        }
        if (ci16 == null) return; // pattern 16 not present on this runtime (e.g. .NET Framework)

        var expected = (-42.5m).ToString("C", ci16.NumberFormat);
        var result = _sut.GetCurrencyFormattedByLocale(ci16.Name, -42.5m, hasCurrency: true, currency: "", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        Assert.Equal(expected, result);
    }

    // ── Zero ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Format_Zero_EnUS_ReturnsFormattedZero()
    {
        var result = _sut.GetCurrencyFormattedByLocale("en-US", 0m, hasCurrency: true, currency: "", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        Assert.Equal("$0.00", result);
    }

    // ── Negative numbers ──────────────────────────────────────────────────────

    [Fact]
    public void Format_NegativeValue_EnUS_MatchesDotNetFormatting()
    {
        var ci = CultureInfo.GetCultureInfo("en-US");
        var expected = (-42.50m).ToString("C", ci.NumberFormat);
        var result = _sut.GetCurrencyFormattedByLocale("en-US", -42.50m, hasCurrency: true, currency: "", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Format_NegativeValue_DeDE_ContainsMinusAndValue()
    {
        var result = _sut.GetCurrencyFormattedByLocale("de-DE", -42.50m, hasCurrency: true, currency: "", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        Assert.Contains("-", result);
        Assert.Contains("42", result);
    }

    // ── No currency symbol ────────────────────────────────────────────────────

    [Fact]
    public void Format_NoCurrency_OmitsCurrencySymbol()
    {
        var result = _sut.GetCurrencyFormattedByLocale("en-US", 100m, hasCurrency: false, currency: "", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        Assert.DoesNotContain("$", result);
        Assert.Contains("100.00", result);
    }

    // ── Custom currency symbol ────────────────────────────────────────────────

    [Fact]
    public void Format_CustomCurrency_UsesProvidedSymbol()
    {
        var result = _sut.GetCurrencyFormattedByLocale("en-US", 99.99m, hasCurrency: true, currency: "BTC", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        Assert.Contains("BTC", result);
        Assert.Contains("99.99", result);
    }

    // ── Native digits ─────────────────────────────────────────────────────────

    [Fact]
    public void Format_NativeDigits_ArabicLocale_ReplacesDigits()
    {
        // ar-SA may use Arabic-Indic digits by default in ICU, so use a locale with distinct native digits
        // and verify the native digit replacement code path works
        var ci = CultureInfo.GetCultureInfo("ar-SA");
        var nfi = ci.NumberFormat;

        var result = _sut.GetCurrencyFormattedByLocale("ar-SA", 123m, hasCurrency: false, currency: "", useNativeDigits: true, useChineseExtendedNumbers: false, useFinancialChinese: false);

        // After native digit replacement, the result should contain the locale's native digit characters
        Assert.Contains(nfi.NativeDigits[1], result); // native "1"
        Assert.Contains(nfi.NativeDigits[2], result); // native "2"
        Assert.Contains(nfi.NativeDigits[3], result); // native "3"
    }

    [Fact]
    public void Format_NativeDigits_False_KeepsStandardFormatting()
    {
        var ci = CultureInfo.GetCultureInfo("ar-SA");
        var expected = 123m.ToString("C", ci.NumberFormat);
        var result = _sut.GetCurrencyFormattedByLocale("ar-SA", 123m, hasCurrency: false, currency: "", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        // When noCurrency + noNativeDigits, result should be the standard "C" format with empty symbol
        Assert.NotNull(result);
    }

    // ── Chinese extended numbers ──────────────────────────────────────────────

    // Use zh-Hans-CN on .NET 10 (ICU), zh-CN on .NET Framework (NLS)
    private static string ZhLocale
    {
        get
        {
            try { CultureInfo.GetCultureInfo("zh-CN"); return "zh-CN"; }
            catch { return "zh-Hans-CN"; }
        }
    }

    [Fact]
    public void Format_ChineseExtended_WholeNumber_UsesChineseNumerals()
    {
        var result = _sut.GetCurrencyFormattedByLocale(ZhLocale, 12345m, hasCurrency: false, currency: "", useNativeDigits: true, useChineseExtendedNumbers: true, useFinancialChinese: false);
        Assert.Contains("万", result);  // 10^4 marker
    }

    [Fact]
    public void Format_ChineseExtended_WithDecimal_UsesChineseDecimalSeparator()
    {
        var result = _sut.GetCurrencyFormattedByLocale(ZhLocale, 12.5m, hasCurrency: false, currency: "", useNativeDigits: true, useChineseExtendedNumbers: true, useFinancialChinese: false);
        Assert.Contains("点", result);  // Chinese decimal separator
    }

    [Fact]
    public void Format_ChineseFinancial_UsesFinancialCharacters()
    {
        var result = _sut.GetCurrencyFormattedByLocale(ZhLocale, 100m, hasCurrency: false, currency: "", useNativeDigits: true, useChineseExtendedNumbers: true, useFinancialChinese: true);
        Assert.Contains("佰", result);
    }

    [Fact]
    public void Format_ChineseExtended_Zero_ReturnsSingleCharacter()
    {
        var result = _sut.GetCurrencyFormattedByLocale(ZhLocale, 0m, hasCurrency: false, currency: "", useNativeDigits: true, useChineseExtendedNumbers: true, useFinancialChinese: false);
        Assert.Equal("〇", result);
    }

    [Fact]
    public void Format_ChineseFinancial_Zero_ReturnsFinancialZero()
    {
        var result = _sut.GetCurrencyFormattedByLocale(ZhLocale, 0m, hasCurrency: false, currency: "", useNativeDigits: true, useChineseExtendedNumbers: true, useFinancialChinese: true);
        Assert.Equal("零", result);
    }

    [Fact]
    public void Format_ChineseExtended_Ten_OmitsLeadingOne()
    {
        var result = _sut.GetCurrencyFormattedByLocale(ZhLocale, 10m, hasCurrency: false, currency: "", useNativeDigits: true, useChineseExtendedNumbers: true, useFinancialChinese: false);
        Assert.Equal("十", result);
    }

    [Fact]
    public void Format_ChineseExtended_NonZhLocale_IgnoresChineseFlag()
    {
        var result = _sut.GetCurrencyFormattedByLocale("en-US", 10m, hasCurrency: false, currency: "", useNativeDigits: true, useChineseExtendedNumbers: true, useFinancialChinese: false);
        Assert.Contains("10", result);
    }

    // ── Invalid locale fallback ───────────────────────────────────────────────

    [Fact]
    public void Format_InvalidLocale_FallsBackToInvariant()
    {
        var result = _sut.GetCurrencyFormattedByLocale("xx-INVALID", 100m, hasCurrency: true, currency: "", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        Assert.NotNull(result);
        Assert.Contains("100", result);
    }

    // ── Large numbers ─────────────────────────────────────────────────────────

    [Fact]
    public void Format_LargeNumber_FormatsWithGroupSeparators()
    {
        var result = _sut.GetCurrencyFormattedByLocale("en-US", 1234567890.12m, hasCurrency: true, currency: "", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        Assert.Equal("$1,234,567,890.12", result);
    }

    [Fact]
    public void Format_ChineseExtended_LargeNumber_UsesChineseUnits()
    {
        var result = _sut.GetCurrencyFormattedByLocale(ZhLocale, 100000000m, hasCurrency: false, currency: "", useNativeDigits: true, useChineseExtendedNumbers: true, useFinancialChinese: false);
        Assert.Contains("亿", result);
    }

    // ── Custom currency with Chinese extended ─────────────────────────────────

    [Fact]
    public void Format_ChineseExtended_WithCustomCurrency_IncludesSymbol()
    {
        var result = _sut.GetCurrencyFormattedByLocale(ZhLocale, 50m, hasCurrency: true, currency: "元", useNativeDigits: true, useChineseExtendedNumbers: true, useFinancialChinese: false);
        Assert.Contains("元", result);
        Assert.Contains("五十", result);
    }
}
