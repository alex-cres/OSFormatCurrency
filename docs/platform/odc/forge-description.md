# OSFormatCurrency — ODC Forge Description

> **Forge:** https://www.outsystems.com/forge/component-overview/15800/formatcurrency  
> **Current Forge version:** 1.1.0 (26 August 2026)  
> **O11 version:** https://www.outsystems.com/forge/component-overview/10096/format-currency

Formats decimal values as locale-aware currency strings, lists available locales, and parses locale-formatted decimal strings back to decimal values. Useful for displaying monetary values in end-user locale and converting locale-formatted user input into numeric types.

## Server Actions

**GetCurrencyFormattedByLocale** — Formats a decimal value as a currency string using the specified locale. Supports custom currency symbols, native digit rendering, and Chinese extended/financial numeral systems (standard and financial variants).

**GetLocales** — Returns all supported locales with their currency formatting rules: symbol, decimal/group separators, positive/negative patterns, and native digit characters.

**GetDecimalFromLocaleDecimalString** — Parses a locale-formatted decimal string back into a decimal value. Supports native digit replacement and custom currency symbols. Returns structured error details when parsing fails.

## Features

- Over 800 locales supported (all .NET cultures)
- Chinese unit numeral system (〇一二三 standard, 零壹贰叁 financial)
- Native digit rendering for any locale (Arabic-Indic, Devanagari, Thai, etc.)
- Custom currency symbol override for formatting and parsing
- Round-trip safe: format a value → parse it back → same decimal
- No external dependencies — runs entirely on the .NET Base Class Library
