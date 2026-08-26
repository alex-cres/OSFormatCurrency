# OSFormatCurrency — Architecture

Structural reference for the solution, the runtime component, and the test
projects. Keep this file in sync with the code — the `documentation-updater`
agent updates it as part of the change cycle whenever the solution layout,
Server Action surface, dependency set, or project structure changes.

> **Runtime behaviour**, **Server Action signatures**, and **supported locales**
> live in [README.md](./README.md) and the [docs/platform/](./docs/platform)
> Forge copies. This document describes **structure only**.

---

## 1. Solution layout

```
FormatCurrency.sln
├── FormatCurrency/                 ← ODC external library (net10.0)
│   ├── FormatCurrency.csproj
│   ├── IFormatCurrency.cs          ← [OSInterface] declaration
│   ├── FormatCurrency.cs           ← Implementation (Chinese numeral conversion, formatting, parsing)
│   ├── FormatCurrencyStructures.cs ← [OSStructure] types: LocaleInfo, ParseDecimalResult
│   ├── generate_upload_package.ps1
│   └── resources/
│       └── icon.png
├── FormatCurrency.Tests/           ← ODC xUnit test suite (net10.0)
│   ├── FormatCurrency.Tests.csproj
│   ├── TestHelpers.cs
│   ├── GetCurrencyFormattedByLocaleTests.cs
│   ├── GetDecimalFromLocaleDecimalStringTests.cs
│   └── GetLocalesTests.cs
├── FormatCurrency.O11/             ← O11 extension (net48)
│   ├── FormatCurrency.O11.csproj
│   ├── IssFormatCurrency.cs        ← O11 interface (Mss-prefixed void methods with out params)
│   ├── RecLocale.cs                ← O11 Locale structure (ss-prefixed fields)
│   └── Actions/
│       └── FormatCurrencyActions.cs ← CssFormatCurrency : IssFormatCurrency
└── FormatCurrency.O11.Tests/       ← O11 xUnit test suite (net48)
    ├── FormatCurrency.O11.Tests.csproj
    ├── TestHelpers.cs              ← O11 adapter types (LocaleInfo, ParseDecimalResult, IFormatCurrency wrappers)
    ├── GetCurrencyFormattedByLocaleTests.cs
    ├── GetDecimalFromLocaleDecimalStringTests.cs
    └── GetLocalesTests.cs
```

## 2. Dependencies

| Package | Version | Platform | Purpose |
|---------|---------|----------|---------|
| `OutSystems.ExternalLibraries.SDK` | 1.5.0 | ODC only | `[OSInterface]`, `[OSAction]`, `[OSStructure]` attributes |

## 3. Naming conventions

| Concern | ODC (`net10.0`) | O11 (`net48`) |
|---------|----------------|---------------|
| Interface | `IFormatCurrency` | `IssFormatCurrency` |
| Implementation | `FormatCurrency` | `CssFormatCurrency` |
| Structures | `LocaleInfo`, `ParseDecimalResult` | `RecLocale` (flat out params for parse result) |
| Namespace | `FormatCurrency` | `OutSystems.NssFormatCurrency` |

## 4. Public surface

| Server Action | ODC return type | O11 signature style |
|---------------|----------------|---------------------|
| `GetCurrencyFormattedByLocale` | `string` | `void Mss…(…, out string)` |
| `GetLocales` | `List<LocaleInfo>` | `void Mss…(out List<RecLocale>)` |
| `GetDecimalFromLocaleDecimalString` | `ParseDecimalResult` | `void Mss…(…, out bool, out int, out string, out decimal)` |
