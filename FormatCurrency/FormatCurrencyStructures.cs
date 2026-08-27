using OutSystems.ExternalLibraries.SDK;

namespace OutSystems.FormatCurrency;

[OSStructure(Description = "Locale information including currency formatting rules.")]
public struct Locale
{
    [OSStructureField(Description = "Display name of the locale (e.g. 'English (United States)').")]
    public string Name { get; set; }

    [OSStructureField(Description = "RFC 4646 locale tag (e.g. 'en-US', 'pt-PT', 'zh-CN').")]
    public string RFC4646 { get; set; }

    [OSStructureField(Description = "Number of decimal digits used in currency values for this locale.")]
    public string CurrencyDecimalDigits { get; set; }

    [OSStructureField(Description = "String used as the decimal separator in currency values.")]
    public string CurrencyDecimalSeparator { get; set; }

    [OSStructureField(Description = "String used as the group (thousands) separator in currency values.")]
    public string CurrencyGroupSeparator { get; set; }

    [OSStructureField(Description = "Array of group sizes (e.g. '[3]' or '[3,2]') for currency formatting.")]
    public string CurrencyGroupSizes { get; set; }

    [OSStructureField(Description = "Pattern for negative currency values with the currency symbol placed (e.g. '-$n', '($n)').")]
    public string CurrencyNegativePattern { get; set; }

    [OSStructureField(Description = "Pattern for positive currency values with the currency symbol placed (e.g. '$n', 'n $').")]
    public string CurrencyPositivePattern { get; set; }

    [OSStructureField(Description = "String used as the negative sign.")]
    public string NegativeSign { get; set; }

    [OSStructureField(Description = "Currency symbol for the locale (e.g. '$', '€', '¥').")]
    public string CurrencySymbol { get; set; }

    [OSStructureField(Description = "Comma-separated list of native digit characters (e.g. '0,1,2,...,9' or locale-specific digits).")]
    public string NativeDigits { get; set; }
}
