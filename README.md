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

> TODO: Document Server Actions once the existing Forge implementations are ported.

---

## Platforms

| Platform | Target Framework | Status |
|----------|-----------------|--------|
| ODC | .NET 10 | Scaffold ready — implementation pending |
| O11 | .NET Framework 4.8 | Scaffold ready — implementation pending |

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
