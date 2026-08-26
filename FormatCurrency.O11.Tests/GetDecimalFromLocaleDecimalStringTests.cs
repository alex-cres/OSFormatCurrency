using Xunit;

namespace FormatCurrency.Tests;

public class GetDecimalFromLocaleDecimalStringTests
{
    private readonly IFormatCurrency _sut = new FormatCurrency();

    // ── Happy path ────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("en-US", "$1,234.56", 1234.56)]
    [InlineData("en-US", "1,234.56", 1234.56)]
    [InlineData("en-US", "100.00", 100.00)]
    [InlineData("de-DE", "1.234,56", 1234.56)]
    public void Parse_ValidInput_ReturnsCorrectDecimal(string locale, string input, decimal expected)
    {
        var result = _sut.GetDecimalFromLocaleDecimalString(input, locale, "");
        Assert.True(result.IsValidDecimal);
        Assert.Equal(0, result.ErrorMessageCode);
        Assert.Equal("", result.ErrorMessage);
        Assert.Equal(expected, result.Value);
    }

    [Fact]
    public void Parse_PtPT_ValidInput_ReturnsCorrectDecimal()
    {
        // pt-PT uses NBSP (U+00A0) as group separator on ICU, dot on NLS
        var ci = System.Globalization.CultureInfo.GetCultureInfo("pt-PT");
        var formatted = 1234.56m.ToString("C", ci.NumberFormat);
        var result = _sut.GetDecimalFromLocaleDecimalString(formatted, "pt-PT", "");
        Assert.True(result.IsValidDecimal);
        Assert.Equal(1234.56m, result.Value);
    }

    [Fact]
    public void Parse_FrFR_ValidInput_ReturnsCorrectDecimal()
    {
        // fr-FR uses narrow NBSP (U+202F) as group separator on ICU
        var ci = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
        var formatted = 1234.56m.ToString("C", ci.NumberFormat);
        var result = _sut.GetDecimalFromLocaleDecimalString(formatted, "fr-FR", "");
        Assert.True(result.IsValidDecimal);
        Assert.Equal(1234.56m, result.Value);
    }

    // ── Zero ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Parse_Zero_ReturnsZero()
    {
        var result = _sut.GetDecimalFromLocaleDecimalString("0", "en-US", "");
        Assert.True(result.IsValidDecimal);
        Assert.Equal(0m, result.Value);
    }

    // ── Negative values ───────────────────────────────────────────────────────

    [Theory]
    [InlineData("en-US", "-$1,234.56", -1234.56)]
    [InlineData("en-US", "($1,234.56)", -1234.56)]
    public void Parse_NegativeValue_ReturnsNegativeDecimal(string locale, string input, decimal expected)
    {
        var result = _sut.GetDecimalFromLocaleDecimalString(input, locale, "");
        Assert.True(result.IsValidDecimal);
        Assert.Equal(expected, result.Value);
    }

    // ── Custom currency symbol ────────────────────────────────────────────────

    [Fact]
    public void Parse_CustomCurrency_ParsesCorrectly()
    {
        var result = _sut.GetDecimalFromLocaleDecimalString("BTC100.00", "en-US", "BTC");
        Assert.True(result.IsValidDecimal);
        Assert.Equal(100m, result.Value);
    }

    [Fact]
    public void Parse_CustomCurrency_OverridesLocaleCurrency()
    {
        // Use "EUR" instead of the default "$" for en-US
        var result = _sut.GetDecimalFromLocaleDecimalString("EUR1,234.56", "en-US", "EUR");
        Assert.True(result.IsValidDecimal);
        Assert.Equal(1234.56m, result.Value);
    }

    // ── Native digits ─────────────────────────────────────────────────────────

    [Fact]
    public void Parse_NativeDigits_ArabicLocale_ParsesCorrectly()
    {
        // Arabic-Indic digits ١٢٣ = 123
        var result = _sut.GetDecimalFromLocaleDecimalString("١٢٣", "ar-SA", "");
        Assert.True(result.IsValidDecimal);
        Assert.Equal(123m, result.Value);
    }

    // ── Error: Empty string ───────────────────────────────────────────────────

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Parse_EmptyOrNull_ReturnsError1(string? input)
    {
        var result = _sut.GetDecimalFromLocaleDecimalString(input!, "en-US", "");
        Assert.False(result.IsValidDecimal);
        Assert.Equal(1, result.ErrorMessageCode);
        Assert.Equal("String Empty", result.ErrorMessage);
        Assert.Equal(0m, result.Value);
    }

    // ── Error: Invalid locale ─────────────────────────────────────────────────

    [Fact]
    public void Parse_InvalidLocale_ReturnsError2()
    {
        var result = _sut.GetDecimalFromLocaleDecimalString("100", "xx-INVALID", "");
        Assert.False(result.IsValidDecimal);
        Assert.Equal(2, result.ErrorMessageCode);
        Assert.Equal("Locale Invalid/Not Provided", result.ErrorMessage);
        Assert.Equal(0m, result.Value);
    }

    // ── Error: Format exception ───────────────────────────────────────────────

    [Fact]
    public void Parse_InvalidFormat_ReturnsError3()
    {
        var result = _sut.GetDecimalFromLocaleDecimalString("not-a-number", "en-US", "");
        Assert.False(result.IsValidDecimal);
        Assert.Equal(3, result.ErrorMessageCode);
        Assert.StartsWith("FormatException", result.ErrorMessage);
        Assert.Equal(0m, result.Value);
    }

    // ── Round-trip: format then parse ─────────────────────────────────────────

    [Theory]
    [InlineData("en-US", 1234.56)]
    [InlineData("de-DE", 9876.54)]
    [InlineData("pt-PT", 42.00)]
    [InlineData("ja-JP", 10000)]
    [InlineData("fr-FR", 555.55)]
    public void RoundTrip_FormatThenParse_RecoversOriginalValue(string locale, decimal original)
    {
        var sut = _sut;
        var formatted = sut.GetCurrencyFormattedByLocale(locale, original, hasCurrency: true, currency: "", useNativeDigits: false, useChineseExtendedNumbers: false, useFinancialChinese: false);
        var parsed = sut.GetDecimalFromLocaleDecimalString(formatted, locale, "");
        Assert.True(parsed.IsValidDecimal);
        Assert.Equal(original, parsed.Value);
    }

    // ── Whitespace-only input ─────────────────────────────────────────────────

    [Fact]
    public void Parse_WhitespaceOnly_ReturnsFormatError()
    {
        var result = _sut.GetDecimalFromLocaleDecimalString("   ", "en-US", "");
        // Whitespace-only is not empty, so it goes through parsing and fails with FormatException
        Assert.False(result.IsValidDecimal);
        Assert.True(result.ErrorMessageCode == 3 || result.ErrorMessageCode == 4);
    }

    // ── Integer input (no decimal) ────────────────────────────────────────────

    [Fact]
    public void Parse_IntegerInput_ReturnsWholeDecimal()
    {
        var result = _sut.GetDecimalFromLocaleDecimalString("42", "en-US", "");
        Assert.True(result.IsValidDecimal);
        Assert.Equal(42m, result.Value);
    }

    // ── Very large number ─────────────────────────────────────────────────────

    [Fact]
    public void Parse_LargeNumber_ParsesCorrectly()
    {
        var result = _sut.GetDecimalFromLocaleDecimalString("$1,234,567,890.12", "en-US", "");
        Assert.True(result.IsValidDecimal);
        Assert.Equal(1234567890.12m, result.Value);
    }
}
