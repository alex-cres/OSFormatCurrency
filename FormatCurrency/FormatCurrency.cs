using System.Globalization;
using System.Text;

namespace OutSystems.FormatCurrency;

public class FormatCurrency : IFormatCurrency
{
    // ── Chinese numeral tables ────────────────────────────────────────────────
    private static readonly string[] ChineseDigits          = { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "百", "千" };
    private static readonly string[] ChineseDigitsFinancial = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾", "佰", "仟" };
    private static readonly string[] ChineseBigNums         = { "万", "亿", "兆", "京" };
    private static readonly string[] ChineseBigNumsFinancial = { "萬", "亿", "兆", "京" };

    private static readonly string[] NegativePatterns = { "($n)", "-$n", "$-n", "$n-", "(n$)", "-n$", "n-$", "n$-", "-n $", "-$ n", "n $-", "$ n-", "$ -n", "n- $", "($ n)", "(n $)", "$- n" };
    private static readonly string[] PositivePatterns = { "$n", "n$", "$ n", "n $" };

    public void GetCurrencyFormattedByLocale(
        string Locale, decimal Decimal, bool HasCurrency, string Currency,
        bool UseNativeDigits, bool UseChineseExtendedNumbers, bool UseFinancialChinese,
        out string FormattedText)
    {
        CultureInfo ci;
        try { ci = CultureInfo.GetCultureInfo(Locale); }
        catch { ci = CultureInfo.InvariantCulture; }

        var nfi = (NumberFormatInfo)ci.NumberFormat.Clone();

        if (!string.IsNullOrEmpty(Currency))
            nfi.CurrencySymbol = Currency;

        if (!HasCurrency)
            nfi.CurrencySymbol = "";

        if (UseChineseExtendedNumbers && UseNativeDigits && Locale.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
        {
            var digits  = UseFinancialChinese ? ChineseDigitsFinancial : ChineseDigits;
            var bigNums = UseFinancialChinese ? ChineseBigNumsFinancial : ChineseBigNums;

            long wholePart = (long)Math.Truncate(Math.Abs(Decimal));
            string formatted;

            string absStr = Math.Abs(Decimal).ToString(CultureInfo.InvariantCulture);
            int dotIdx = absStr.IndexOf('.');
            if (dotIdx > 0)
            {
                int decLen = absStr.Length - dotIdx - 1;
                long decimalPart = (long)(Math.Pow(10, decLen) * ((double)(Math.Abs(Decimal) - Math.Truncate(Math.Abs(Decimal)))));
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

            if (Decimal >= 0)
            {
                int pp = nfi.CurrencyPositivePattern;
                string posPattern = pp < PositivePatterns.Length ? PositivePatterns[pp] : "$n";
                FormattedText = posPattern.Replace("n", formatted).Replace("$", nfi.CurrencySymbol);
            }
            else
            {
                int np = nfi.CurrencyNegativePattern;
                string negPattern = np < NegativePatterns.Length ? NegativePatterns[np] : "($n)";
                FormattedText = negPattern.Replace("n", formatted).Replace("-", nfi.NegativeSign).Replace("$", nfi.CurrencySymbol);
            }
        }
        else
        {
            FormattedText = Decimal.ToString("C", nfi);

            if (UseNativeDigits)
            {
                for (int i = 0; i < nfi.NativeDigits.Length; i++)
                    FormattedText = FormattedText.Replace(i.ToString(), nfi.NativeDigits[i]);
            }
        }
    }

    public void GetLocales(out List<Locale> ListofLocals)
    {
        ListofLocals = new List<Locale>();

        foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
        {
            var nfi = ci.NumberFormat;
            ListofLocals.Add(new Locale
            {
                Name                     = ci.DisplayName,
                RFC4646                  = ci.Name,
                CurrencyDecimalDigits    = nfi.CurrencyDecimalDigits.ToString(),
                CurrencyDecimalSeparator = nfi.CurrencyDecimalSeparator,
                CurrencyGroupSeparator   = nfi.CurrencyGroupSeparator,
                CurrencyGroupSizes       = "[" + string.Join(",", nfi.CurrencyGroupSizes) + "]",
                CurrencyNegativePattern  = (nfi.CurrencyNegativePattern < NegativePatterns.Length
                                              ? NegativePatterns[nfi.CurrencyNegativePattern]
                                              : nfi.CurrencyNegativePattern.ToString())
                                              .Replace("-", nfi.NegativeSign)
                                              .Replace("$", nfi.CurrencySymbol),
                CurrencyPositivePattern  = (nfi.CurrencyPositivePattern < PositivePatterns.Length
                                              ? PositivePatterns[nfi.CurrencyPositivePattern]
                                              : nfi.CurrencyPositivePattern.ToString())
                                              .Replace("$", nfi.CurrencySymbol),
                NativeDigits             = string.Join(",", nfi.NativeDigits),
                NegativeSign             = nfi.NegativeSign,
                CurrencySymbol           = nfi.CurrencySymbol,
            });
        }
    }

    public void GetDecimalFromLocaleDecimalString(
        string InputLocalelDecimalString, string Locale, string Currency,
        out bool IsValidDecimal, out int ErrorMessageCode, out string ErrorMessage, out decimal Decimal)
    {
        IsValidDecimal   = true;
        ErrorMessageCode = 0;
        ErrorMessage     = "";
        Decimal          = 0.0m;

        if (string.IsNullOrEmpty(InputLocalelDecimalString))
        {
            IsValidDecimal   = false;
            ErrorMessage     = "String Empty";
            ErrorMessageCode = 1;
            return;
        }

        CultureInfo culture;
        try { culture = CultureInfo.GetCultureInfo(Locale); }
        catch (CultureNotFoundException)
        {
            IsValidDecimal   = false;
            ErrorMessage     = "Locale Invalid/Not Provided";
            ErrorMessageCode = 2;
            return;
        }

        try
        {
            var nfi = (NumberFormatInfo)culture.NumberFormat.Clone();

            string input = InputLocalelDecimalString;
            for (int i = 0; i < nfi.NativeDigits.Length; i++)
            {
                if (input.Contains(nfi.NativeDigits[i]))
                    input = input.Replace(nfi.NativeDigits[i], i.ToString());
            }

            if (!string.IsNullOrEmpty(Currency))
                nfi.CurrencySymbol = Currency;

            Decimal = decimal.Parse(input, NumberStyles.Currency, nfi);
        }
        catch (FormatException e)
        {
            IsValidDecimal   = false;
            ErrorMessage     = "FormatException [" + e + "]";
            ErrorMessageCode = 3;
        }
        catch (Exception e)
        {
            IsValidDecimal   = false;
            ErrorMessage     = "Other Errors [" + e + "]";
            ErrorMessageCode = 4;
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
        var b        = new StringBuilder();
        long baseNum = 10000000000000000L;
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

        if (b.Length >= 2 && b.ToString().Substring(0, 2) == string.Concat(digits[1], digits[10]))
            return b.Remove(0, 1).ToString();

        return b.ToString();
    }
}
