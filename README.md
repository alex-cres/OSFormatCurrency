# OSFormatCurrency

**ODC External Library + O11 Integration Studio Extension**

Formats decimal values as locale-aware currency strings, lists available locales, and parses locale-formatted decimal strings back to decimal values.

---

## Objective

Applications that display monetary values to end users need locale-aware formatting — thousands separators, decimal marks, and currency symbols vary by region. This component provides three capabilities:

1. **Format a decimal** into a currency string using a specified locale and optional currency symbol override.
2. **List available locales** so the caller can present a locale picker or validate user input.
3. **Parse a locale-formatted decimal string** back into a numeric `decimal` value for storage or calculation.

---

## Server Actions

### GetCurrencyFormattedByLocale

Formats a decimal value as a currency string using the specified locale. Supports custom currency symbols, native digit rendering, and Chinese extended/financial numeral systems.

| Parameter | Type | Direction | Description |
|-----------|------|-----------|-------------|
| `locale` | `string` | Input | RFC 4646 locale tag (e.g. `en-US`, `pt-PT`, `zh-CN`). Invalid locale falls back to invariant culture. |
| `value` | `decimal` | Input | The decimal value to format. |
| `hasCurrency` | `bool` | Input | When `true`, includes the currency symbol in the output. |
| `currency` | `string` | Input | Custom currency symbol override. Empty string uses the locale default. |
| `useNativeDigits` | `bool` | Input | When `true`, replaces 0–9 with the locale's native digit characters. |
| `useChineseExtendedNumbers` | `bool` | Input | When `true` and the locale starts with `zh`, uses the Chinese unit numeral system. Requires `useNativeDigits`. |
| `useFinancialChinese` | `bool` | Input | When `true`, uses financial Chinese numerals (e.g. 零→壹). Requires `useChineseExtendedNumbers` and `useNativeDigits`. |
| *(return)* | `string` | Output | The formatted currency string. |

### GetLocales

Returns all .NET-supported locales with their currency formatting rules.

| Parameter | Type | Direction | Description |
|-----------|------|-----------|-------------|
| *(return)* | `List<LocaleInfo>` | Output | List of locale records with currency formatting metadata. |

**LocaleInfo fields:** `Name`, `RFC4646`, `CurrencyDecimalDigits`, `CurrencyDecimalSeparator`, `CurrencyGroupSeparator`, `CurrencyGroupSizes`, `CurrencyNegativePattern`, `CurrencyPositivePattern`, `NegativeSign`, `CurrencySymbol`, `NativeDigits`.

### GetDecimalFromLocaleDecimalString

Parses a locale-formatted decimal string back into a decimal value.

| Parameter | Type | Direction | Description |
|-----------|------|-----------|-------------|
| `inputLocaleDecimalString` | `string` | Input | The locale-formatted string to parse. |
| `locale` | `string` | Input | RFC 4646 locale tag the string was written in. |
| `currency` | `string` | Input | Custom currency symbol override for parsing. Empty string uses the locale default. |
| *(return)* | `ParseDecimalResult` | Output | Result containing the parsed value or error details. |

**ParseDecimalResult fields:** `IsValidDecimal` (`bool`), `ErrorMessageCode` (`int`: 0 = success, 1 = empty string, 2 = invalid locale, 3 = format error, 4 = other), `ErrorMessage` (`string`), `Value` (`decimal`).

---

## Platforms

| Platform | Target Framework | Forge | Current Version |
|----------|-----------------|-------|------------------|
| ODC | .NET 10 | [FormatCurrency](https://www.outsystems.com/forge/component-overview/15800/formatcurrency) | 1.1.0 |
| O11 | .NET Framework 4.8 | [Format Currency](https://www.outsystems.com/forge/component-overview/10096/format-currency) | 1.1.0 |

---

## Build

```bash
dotnet build FormatCurrency.sln
```

## Test

```bash
dotnet test FormatCurrency.sln
```

## Package (ODC)

```powershell
.\FormatCurrency\generate_upload_package.ps1
```

---

## License

[MIT](./LICENSE)
