using System.Globalization;
using System.Linq;
using Xunit;

namespace FormatCurrency.Tests;

public class GetLocalesTests
{
    private readonly IFormatCurrency _sut = new FormatCurrency();

    [Fact]
    public void GetLocales_ReturnsNonEmptyList()
    {
        var locales = _sut.GetLocales();
        Assert.NotEmpty(locales);
    }

    [Fact]
    public void GetLocales_ContainsInvariantCulture()
    {
        var locales = _sut.GetLocales();
        Assert.Contains(locales, l => l.RFC4646 == "");
    }

    [Theory]
    [InlineData("en-US")]
    [InlineData("pt-PT")]
    [InlineData("de-DE")]
    [InlineData("ja-JP")]
    [InlineData("fr-FR")]
    [InlineData("ar-SA")]
    public void GetLocales_ContainsWellKnownLocale(string rfc4646)
    {
        var locales = _sut.GetLocales();
        Assert.Contains(locales, l => l.RFC4646 == rfc4646);
    }

    [Fact]
    public void GetLocales_ContainsChineseLocale()
    {
        var locales = _sut.GetLocales();
        // zh-CN on NLS (.NET Framework), zh-Hans-CN on ICU (.NET 5+)
        Assert.Contains(locales, l => l.RFC4646.StartsWith("zh"));
    }

    [Fact]
    public void GetLocales_EnUS_HasDollarSymbol()
    {
        var locales = _sut.GetLocales();
        var enUS = locales.First(l => l.RFC4646 == "en-US");
        Assert.Equal("$", enUS.CurrencySymbol);
    }

    [Fact]
    public void GetLocales_PtPT_HasEuroSymbol()
    {
        var locales = _sut.GetLocales();
        var ptPT = locales.First(l => l.RFC4646 == "pt-PT");
        Assert.Equal("€", ptPT.CurrencySymbol);
    }

    [Fact]
    public void GetLocales_JaJP_HasYenSymbol()
    {
        var locales = _sut.GetLocales();
        var jaJP = locales.First(l => l.RFC4646 == "ja-JP");
        // Yen symbol: halfwidth \u00A5 on NLS, fullwidth \uFFE5 on ICU
        var ci = CultureInfo.GetCultureInfo("ja-JP");
        Assert.Equal(ci.NumberFormat.CurrencySymbol, jaJP.CurrencySymbol);
    }

    [Fact]
    public void GetLocales_AllEntriesHaveNonNullFields()
    {
        var locales = _sut.GetLocales();
        foreach (var locale in locales)
        {
            Assert.NotNull(locale.Name);
            Assert.NotNull(locale.RFC4646);
            Assert.NotNull(locale.CurrencyDecimalDigits);
            Assert.NotNull(locale.CurrencyDecimalSeparator);
            Assert.NotNull(locale.CurrencyGroupSeparator);
            Assert.NotNull(locale.CurrencyGroupSizes);
            Assert.NotNull(locale.CurrencyNegativePattern);
            Assert.NotNull(locale.CurrencyPositivePattern);
            Assert.NotNull(locale.NegativeSign);
            Assert.NotNull(locale.CurrencySymbol);
            Assert.NotNull(locale.NativeDigits);
        }
    }

    [Fact]
    public void GetLocales_CurrencyGroupSizes_HasArrayFormat()
    {
        var locales = _sut.GetLocales();
        foreach (var locale in locales)
        {
            Assert.StartsWith("[", locale.CurrencyGroupSizes);
            Assert.EndsWith("]", locale.CurrencyGroupSizes);
        }
    }

    [Fact]
    public void GetLocales_NativeDigits_HasTenDigits()
    {
        var locales = _sut.GetLocales();
        foreach (var locale in locales)
        {
            var digits = locale.NativeDigits.Split(',');
            Assert.Equal(10, digits.Length);
        }
    }

    [Fact]
    public void GetLocales_EnUS_HasDecimalPointSeparator()
    {
        var locales = _sut.GetLocales();
        var enUS = locales.First(l => l.RFC4646 == "en-US");
        Assert.Equal(".", enUS.CurrencyDecimalSeparator);
    }

    [Fact]
    public void GetLocales_DeDE_HasCommaDecimalSeparator()
    {
        var locales = _sut.GetLocales();
        var deDE = locales.First(l => l.RFC4646 == "de-DE");
        Assert.Equal(",", deDE.CurrencyDecimalSeparator);
    }

    [Fact]
    public void GetLocales_NegativePattern16_ProducesValidPatternString()
    {
        // On ICU (.NET 5+), some cultures use CurrencyNegativePattern = 16 ("$- n").
        // Verify GetLocales produces a resolved pattern string (not the raw index).
        var locales = _sut.GetLocales();
        foreach (var locale in locales)
        {
            // No pattern string should be a bare integer — it should contain currency symbols or n
            Assert.False(int.TryParse(locale.CurrencyNegativePattern, out _),
                $"Locale {locale.RFC4646} has unresolved negative pattern: {locale.CurrencyNegativePattern}");
            Assert.False(int.TryParse(locale.CurrencyPositivePattern, out _),
                $"Locale {locale.RFC4646} has unresolved positive pattern: {locale.CurrencyPositivePattern}");
        }
    }
}
