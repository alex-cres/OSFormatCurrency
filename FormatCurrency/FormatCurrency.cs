using System.Globalization;
using System.Text;

namespace FormatCurrency;

public class FormatCurrency : IFormatCurrency
{
    // ── Chinese numeral tables ────────────────────────────────────────────────
    private static readonly string[] ChineseDigits          = { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "百", "千" };
    private static readonly string[] ChineseDigitsFinancial = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾", "佰", "仟" };
    private static readonly string[] ChineseBigNums         = { "万", "亿", "兆", "京" };
    private static readonly string[] ChineseBigNumsFinancial = { "萬", "亿", "兆", "京" };

    private static readonly string[] NegativePatterns = { "($n)", "-$n", "$-n", "$n-", "(n$)", "-n$", "n-$", "n$-", "-n $", "-$ n", "n $-", "$ n-", "$ -n", "n- $", "($ n)", "(n $)", "$- n" };
    private static readonly string[] PositivePatterns = { "$n", "n$", "$ n", "n $" };

    public string GetCurrencyFormattedByLocale(
        string locale, decimal value, bool hasCurrency, string currency,
        bool useNativeDigits, bool useChineseExtendedNumbers, bool useFinancialChinese)
    {
        CultureInfo ci;
        try { ci = CultureInfo.GetCultureInfo(locale); }
        catch { ci = CultureInfo.InvariantCulture; }

        var nfi = (NumberFormatInfo)ci.NumberFormat.Clone();

        if (!string.IsNullOrEmpty(currency))
            nfi.CurrencySymbol = currency;

        if (!hasCurrency)
            nfi.CurrencySymbol = "";

        if (useChineseExtendedNumbers && useNativeDigits && locale.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
        {
            var digits  = useFinancialChinese ? ChineseDigitsFinancial : ChineseDigits;
            var bigNums = useFinancialChinese ? ChineseBigNumsFinancial : ChineseBigNums;

            long wholePart = (long)Math.Truncate(Math.Abs(value));
            string formatted;

            string absStr = Math.Abs(value).ToString(CultureInfo.InvariantCulture);
            int dotIdx = absStr.IndexOf('.');
            if (dotIdx > 0)
            {
                int decLen = absStr.Length - dotIdx - 1;
                long decimalPart = (long)(Math.Pow(10, decLen) * ((double)(Math.Abs(value) - Math.Truncate(Math.Abs(value)))));
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

            if (value >= 0)
            {
                int pp = nfi.CurrencyPositivePattern;
                string posPattern = pp < PositivePatterns.Length ? PositivePatterns[pp] : "$n";
                return posPattern.Replace("n", formatted).Replace("$", nfi.CurrencySymbol);
            }
            else
            {
                int np = nfi.CurrencyNegativePattern;
                string negPattern = np < NegativePatterns.Length ? NegativePatterns[np] : "($n)";
                return negPattern.Replace("n", formatted).Replace("-", nfi.NegativeSign).Replace("$", nfi.CurrencySymbol);
            }
        }
        else
        {
            string result = value.ToString("C", nfi);

            if (useNativeDigits)
            {
                for (int i = 0; i < nfi.NativeDigits.Length; i++)
                    result = result.Replace(i.ToString(), nfi.NativeDigits[i]);
            }

            return result;
        }
    }

    public List<LocaleInfo> GetLocales()
    {
        var list = new List<LocaleInfo>();

        foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
        {
            var nfi = ci.NumberFormat;
            list.Add(new LocaleInfo
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

        return list;
    }

    public ParseDecimalResult GetDecimalFromLocaleDecimalString(string inputLocaleDecimalString, string locale, string currency)
    {
        var result = new ParseDecimalResult
        {
            IsValidDecimal   = true,
            ErrorMessageCode = 0,
            ErrorMessage     = "",
            Value            = 0.0m
        };

        if (string.IsNullOrEmpty(inputLocaleDecimalString))
        {
            result.IsValidDecimal   = false;
            result.ErrorMessage     = "String Empty";
            result.ErrorMessageCode = 1;
            return result;
        }

        CultureInfo culture;
        try { culture = CultureInfo.GetCultureInfo(locale); }
        catch (CultureNotFoundException)
        {
            result.IsValidDecimal   = false;
            result.ErrorMessage     = "Locale Invalid/Not Provided";
            result.ErrorMessageCode = 2;
            return result;
        }

        try
        {
            var nfi = (NumberFormatInfo)culture.NumberFormat.Clone();

            // Replace native digits with 0-9
            string input = inputLocaleDecimalString;
            for (int i = 0; i < nfi.NativeDigits.Length; i++)
            {
                if (input.Contains(nfi.NativeDigits[i]))
                    input = input.Replace(nfi.NativeDigits[i], i.ToString());
            }

            if (!string.IsNullOrEmpty(currency))
                nfi.CurrencySymbol = currency;

            result.Value = decimal.Parse(input, NumberStyles.Currency, nfi);
        }
        catch (FormatException e)
        {
            result.IsValidDecimal   = false;
            result.ErrorMessage     = "FormatException [" + e + "]";
            result.ErrorMessageCode = 3;
        }
        catch (Exception e)
        {
            result.IsValidDecimal   = false;
            result.ErrorMessage     = "Other Errors [" + e + "]";
            result.ErrorMessageCode = 4;
        }

        return result;
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
