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
│   ├── FormatCurrency.cs           ← Implementation
│   ├── generate_upload_package.ps1
│   └── resources/
│       └── icon.png
├── FormatCurrency.Tests/           ← ODC xUnit test suite (net10.0)
│   ├── FormatCurrency.Tests.csproj
│   └── TestHelpers.cs
├── FormatCurrency.O11/             ← O11 extension (net48)
│   ├── FormatCurrency.O11.csproj
│   ├── IssFormatCurrency.cs
│   └── Actions/
│       └── FormatCurrencyActions.cs
└── FormatCurrency.O11.Tests/       ← O11 xUnit test suite (net48)
    ├── FormatCurrency.O11.Tests.csproj
    └── TestHelpers.cs
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
| Namespace | `FormatCurrency` | `OutSystems.NssFormatCurrency` |
