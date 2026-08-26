using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OutSystems.NssFormatCurrency;

public partial class CssFormatCurrency : IssFormatCurrency
{
    // ── Chinese numeral tables ────────────────────────────────────────────────
    private static readonly string[] ChineseDigits         = { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "百", "千" };
    private static readonly string[] ChineseDigitsFinancial = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾", "佰", "仟" };
    private static readonly string[] ChineseBigNums         = { "万", "亿", "兆", "京" };
    private static readonly string[] ChineseBigNumsFinancial = { "萬", "亿", "兆", "京" };

    // ── Currency formatting patterns (matching .NET CurrencyNegativePattern / CurrencyPositivePattern indices) ──
    private static readonly string[] NegativePatterns = { "($n)", "-$n", "$-n", "$n-", "(n$)", "-n$", "n-$", "n$-", "-n $", "-$ n", "n $-", "$ n-", "$ -n", "n- $", "($ n)", "(n $)", "$- n" };
    private static readonly string[] PositivePatterns = { "$n", "n$", "$ n", "n $" };

    public void MssGetCurrencyFormattedByLocale(
        string ssLocale, decimal ssDecimal, bool ssHasCurrency, string ssCurrency,
        bool ssUseNativeDigits, bool ssUseChineseExtendedNumbers, bool ssUseFinancialChinese,
        out string ssFormattedText)
    {
        ssFormattedText = "";

        CultureInfo ci;
        try { ci = CultureInfo.GetCultureInfo(ssLocale); }
        catch { ci = CultureInfo.InvariantCulture; }

        var nfi = (NumberFormatInfo)ci.NumberFormat.Clone();

        if (!string.IsNullOrEmpty(ssCurrency))
            nfi.CurrencySymbol = ssCurrency;

        if (!ssHasCurrency)
            nfi.CurrencySymbol = "";

        if (ssUseChineseExtendedNumbers && ssUseNativeDigits && ssLocale.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
        {
            var digits  = ssUseFinancialChinese ? ChineseDigitsFinancial : ChineseDigits;
            var bigNums = ssUseFinancialChinese ? ChineseBigNumsFinancial : ChineseBigNums;

            long wholePart  = (long)Math.Truncate(Math.Abs(ssDecimal));
            string formatted;

            string absStr = Math.Abs(ssDecimal).ToString(CultureInfo.InvariantCulture);
            int dotIdx = absStr.IndexOf('.');
            if (dotIdx > 0)
            {
                int decLen = absStr.Length - dotIdx - 1;
                long decimalPart = (long)(Math.Pow(10, decLen) * ((double)(Math.Abs(ssDecimal) - Math.Truncate(Math.Abs(ssDecimal)))));
                string decPartStr = decimalPart.ToString();

                nfi.CurrencyDecimalSeparator = "点";

                for (int i = 0; i <= 9; i++)
                    decPartStr = decPartStr.Replace(i.ToString(), digits[i]);

                formatted = ChineseNumberConvert(wholePart, digits, bigNums) +
                            nfi.CurrencyDecimalSeparator +
                            decPartStr;
            }
            else
            {
                formatted = ChineseNumberConvert(wholePart, digits, bigNums);
            }

            if (ssDecimal >= 0)
            {
                int pp = nfi.CurrencyPositivePattern;
                string posPattern = pp < PositivePatterns.Length ? PositivePatterns[pp] : "$n";
                ssFormattedText = posPattern.Replace("n", formatted).Replace("$", nfi.CurrencySymbol);
            }
            else
            {
                int np = nfi.CurrencyNegativePattern;
                string negPattern = np < NegativePatterns.Length ? NegativePatterns[np] : "($n)";
                ssFormattedText = negPattern.Replace("n", formatted).Replace("-", nfi.NegativeSign).Replace("$", nfi.CurrencySymbol);
            }
        }
        else
        {
            ssFormattedText = ssDecimal.ToString("C", nfi);

            if (ssUseNativeDigits)
            {
                for (int i = 0; i < nfi.NativeDigits.Length; i++)
                    ssFormattedText = ssFormattedText.Replace(i.ToString(), nfi.NativeDigits[i]);
            }
        }
    }

    public void MssGetLocales(out List<RecLocale> ssListOfLocales)
    {
        ssListOfLocales = new List<RecLocale>();

        foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
        {
            var nfi = ci.NumberFormat;
            ssListOfLocales.Add(new RecLocale
            {
                ssName                     = ci.DisplayName,
                ssRFC4646                  = ci.Name,
                ssCurrencyDecimalDigits    = nfi.CurrencyDecimalDigits.ToString(),
                ssCurrencyDecimalSeparator = nfi.CurrencyDecimalSeparator,
                ssCurrencyGroupSeparator   = nfi.CurrencyGroupSeparator,
                ssCurrencyGroupSizes       = "[" + string.Join(",", nfi.CurrencyGroupSizes) + "]",
                ssCurrencyNegativePattern  = (nfi.CurrencyNegativePattern < NegativePatterns.Length
                                              ? NegativePatterns[nfi.CurrencyNegativePattern]
                                              : nfi.CurrencyNegativePattern.ToString())
                                                .Replace("-", nfi.NegativeSign)
                                                .Replace("$", nfi.CurrencySymbol),
                ssCurrencyPositivePattern  = (nfi.CurrencyPositivePattern < PositivePatterns.Length
                                              ? PositivePatterns[nfi.CurrencyPositivePattern]
                                              : nfi.CurrencyPositivePattern.ToString())
                                                .Replace("$", nfi.CurrencySymbol),
                ssNativeDigits             = string.Join(",", nfi.NativeDigits),
                ssNegativeSign             = nfi.NegativeSign,
                ssCurrencySymbol           = nfi.CurrencySymbol,
            });
        }
    }

    public void MssGetDecimalFromLocaleDecimalString(
        string ssInputLocaleDecimalString, string ssLocale, string ssCurrency,
        out bool ssIsValidDecimal, out int ssErrorMessageCode, out string ssErrorMessage, out decimal ssDecimal)
    {
        ssIsValidDecimal   = true;
        ssErrorMessageCode = 0;
        ssErrorMessage     = "";
        ssDecimal          = 0.0m;

        if (string.IsNullOrEmpty(ssInputLocaleDecimalString))
        {
            ssIsValidDecimal   = false;
            ssErrorMessage     = "String Empty";
            ssErrorMessageCode = 1;
            return;
        }

        CultureInfo culture;
        try { culture = CultureInfo.GetCultureInfo(ssLocale); }
        catch (CultureNotFoundException)
        {
            ssIsValidDecimal   = false;
            ssErrorMessage     = "Locale Invalid/Not Provided";
            ssErrorMessageCode = 2;
            return;
        }

        try
        {
            var nfi = (NumberFormatInfo)culture.NumberFormat.Clone();

            // Replace native digits with 0-9
            for (int i = 0; i < nfi.NativeDigits.Length; i++)
            {
                if (ssInputLocaleDecimalString.Contains(nfi.NativeDigits[i]))
                    ssInputLocaleDecimalString = ssInputLocaleDecimalString.Replace(nfi.NativeDigits[i], i.ToString());
            }

            if (!string.IsNullOrEmpty(ssCurrency))
                nfi.CurrencySymbol = ssCurrency;

            ssDecimal = decimal.Parse(ssInputLocaleDecimalString, NumberStyles.Currency, nfi);
        }
        catch (FormatException e)
        {
            ssIsValidDecimal   = false;
            ssErrorMessage     = "FormatException [" + e + "]";
            ssErrorMessageCode = 3;
        }
        catch (Exception e)
        {
            ssIsValidDecimal   = false;
            ssErrorMessage     = "Other Errors [" + e + "]";
            ssErrorMessageCode = 4;
        }
    }

    // ── Chinese number conversion ─────────────────────────────────────────────

    private static string ChineseSmallNumberConvert(long number, string appendedNumber, string[] digits)
    {
        if (number >= 10000)
            throw new ArgumentOutOfRangeException(nameof(number), "number must be less than 10000");

        var b   = new StringBuilder();
        int n   = 1000;
        int idx = 12;

        do
        {
            if (number >= n)
            {
                b.Append(digits[number / n]);
                if (n >= 10) b.Append(digits[idx]);
                number %= n;
            }
            else if (number / n == 0 && b.Length != 0 && number != 0 &&
                     b.ToString(b.Length - 1, 1) != ChineseDigitsFinancial[0])
            {
                b.Append(ChineseDigitsFinancial[0]);
            }
            else if (n == 1000 && number / n == 0 && appendedNumber.Length != 0 && number != 0 &&
                     appendedNumber[appendedNumber.Length - 1].ToString() != ChineseDigitsFinancial[0])
            {
                b.Append(ChineseDigitsFinancial[0]);
            }

            n   /= 10;
            idx -= 1;
        }
        while (n > 0);

        return b.ToString();
    }

    private static string ChineseNumberConvert(long number, string[] digits, string[] bigNumbers)
    {
        var b       = new StringBuilder();
        long baseNum = 10000000000000000L; // 10^16
        int baseIdx  = 3;

        if (number == 0)
        {
            b.Append(digits[0]);
        }
        else
        {
            do
            {
                long n = number / baseNum;
                if (n > 0)
                {
                    b.Append(ChineseSmallNumberConvert(n, b.ToString(), digits));
                    if (baseIdx >= 0) b.Append(bigNumbers[baseIdx]);
                }

                number  %= baseNum;
                baseNum /= 10000;
                baseIdx -= 1;
            }
            while (baseNum > 0);
        }

        // Remove leading 一 before 十 (e.g. 一十二 → 十二)
        if (b.Length >= 2 && b.ToString().Substring(0, 2) == string.Concat(digits[1], digits[10]))
            return b.Remove(0, 1).ToString();

        return b.ToString();
    }
}
