using System;
using System.Collections;
using System.Data;
using OutSystems.HubEdition.RuntimePlatform;
using OutSystems.RuntimePublic.Db;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace OutSystems.NssFormatCurrency {

public class CssFormatCurrency: IssFormatCurrency {

// ── Chinese numeral tables ────────────────────────────────────────────────
private static readonly string[] ChineseDigits          = { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "百", "千" };
private static readonly string[] ChineseDigitsFinancial = { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾", "佰", "仟" };
private static readonly string[] ChineseBigNums         = { "万", "亿", "兆", "京" };
private static readonly string[] ChineseBigNumsFinancial = { "萬", "亿", "兆", "京" };

// ── Currency formatting patterns (matching .NET CurrencyNegativePattern / CurrencyPositivePattern indices) ──
private static readonly string[] NegativePatterns = { "($n)", "-$n", "$-n", "$n-", "(n$)", "-n$", "n-$", "n$-", "-n $", "-$ n", "n $-", "$ n-", "$ -n", "n- $", "($ n)", "(n $)", "$- n" };
private static readonly string[] PositivePatterns = { "$n", "n$", "$ n", "n $" };

/// <summary>
/// Converts a Locale Decimal String (a decimal that was written in in a certain locale) into a valid Decimal.
/// For now &quot;zh&quot; locale that uses the extended numbers and financial numbers will not be converted accordingly.
/// Error Message Codes and meanings:
/// 0 - &quot;&quot; - No Error
/// 1 - &quot;String Empty&quot; - The string was empty
/// 2 - &quot;Locale Invalid/Not Provided&quot; - No Locale was found
/// 3 - &quot;FormatException&quot; - an error in the format of the number
/// 4 - &quot;Other Errors [x]&quot; - other errors - log represented in x
/// </summary>
public void MssGetDecimalFromLocaleDecimalString(string ssInputLocalelDecimalString, string ssLocale, string ssCurrency, out bool ssIsValidDecimal, out int ssErrorMessageCode, out string ssErrorMessage, out decimal ssDecimal) {
ssIsValidDecimal = true;
ssErrorMessageCode = 0;
ssErrorMessage = "";
ssDecimal = 0.0m;

if (String.IsNullOrEmpty(ssInputLocalelDecimalString))
{
ssIsValidDecimal   = false;
ssErrorMessage     = "String Empty";
ssErrorMessageCode = 1;
return;
}

CultureInfo culture;
try
{
culture = CultureInfo.GetCultureInfo(ssLocale);
}
catch (CultureNotFoundException)
{
ssIsValidDecimal   = false;
ssErrorMessage     = "Locale Invalid/Not Provided";
ssErrorMessageCode = 2;
return;
}

try
{
NumberFormatInfo numberFormatInfo = (NumberFormatInfo)culture.NumberFormat.Clone();

string input = ssInputLocalelDecimalString;
for (int i = 0; i < numberFormatInfo.NativeDigits.Length; i++)
{
if (input.Contains(numberFormatInfo.NativeDigits[i]))
input = input.Replace(numberFormatInfo.NativeDigits[i], i.ToString());
}

if (!String.IsNullOrEmpty(ssCurrency))
{
numberFormatInfo.CurrencySymbol = ssCurrency;
}

ssDecimal = Decimal.Parse(input, NumberStyles.Currency, numberFormatInfo);
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
} // MssGetDecimalFromLocaleDecimalString

/// <summary>
/// Formats the Decimal based on the Locale provided.
/// </summary>
public void MssGetCurrencyFormattedByLocale(string ssLocale, decimal ssDecimal, bool ssHasCurrency, string ssCurrency, bool ssUseNativeDigits, bool ssUseChineseExtendedNumbers, bool ssUseFinancialChinese, out string ssFormattedText) {
ssFormattedText = "";

CultureInfo ci;
try
{
ci = CultureInfo.GetCultureInfo(ssLocale);
}
catch
{
ci = CultureInfo.InvariantCulture;
}

NumberFormatInfo numberFormatInfo = (NumberFormatInfo)ci.NumberFormat.Clone();

if (!String.IsNullOrEmpty(ssCurrency))
{
numberFormatInfo.CurrencySymbol = ssCurrency;
}

if (!ssHasCurrency)
{
numberFormatInfo.CurrencySymbol = "";
}

if (ssUseChineseExtendedNumbers && ssUseNativeDigits && ssLocale.StartsWith("zh", StringComparison.OrdinalIgnoreCase))
{
var digits  = ssUseFinancialChinese ? ChineseDigitsFinancial : ChineseDigits;
var bigNums = ssUseFinancialChinese ? ChineseBigNumsFinancial : ChineseBigNums;

long wholePart = (long)Math.Truncate(Math.Abs(ssDecimal));
string formatted;

string absStr = Math.Abs(ssDecimal).ToString(CultureInfo.InvariantCulture);
int dotIdx = absStr.IndexOf('.');
if (dotIdx > 0)
{
int decLen = absStr.Length - dotIdx - 1;
long decimalPart = (long)((decimal)Math.Pow(10, decLen) * (Math.Abs(ssDecimal) - Math.Truncate(Math.Abs(ssDecimal))));
string decPartStr = decimalPart.ToString();

numberFormatInfo.CurrencyDecimalSeparator = "点";

for (int i = 0; i <= 9; i++)
decPartStr = decPartStr.Replace(i.ToString(), digits[i]);

formatted = ChineseNumberConvert(wholePart, digits, bigNums) +
numberFormatInfo.CurrencyDecimalSeparator +
decPartStr;
}
else
{
formatted = ChineseNumberConvert(wholePart, digits, bigNums);
}

if (ssDecimal >= 0)
{
int pp = numberFormatInfo.CurrencyPositivePattern;
string posPattern = pp < PositivePatterns.Length ? PositivePatterns[pp] : "$n";
ssFormattedText = posPattern.Replace("n", formatted).Replace("$", numberFormatInfo.CurrencySymbol);
}
else
{
int np = numberFormatInfo.CurrencyNegativePattern;
string negPattern = np < NegativePatterns.Length ? NegativePatterns[np] : "($n)";
ssFormattedText = negPattern.Replace("n", formatted).Replace("-", numberFormatInfo.NegativeSign).Replace("$", numberFormatInfo.CurrencySymbol);
}
}
else
{
ssFormattedText = ssDecimal.ToString("C", numberFormatInfo);

if (ssUseNativeDigits)
{
for (int i = 0; i < numberFormatInfo.NativeDigits.Length; i++)
ssFormattedText = ssFormattedText.Replace(i.ToString(), numberFormatInfo.NativeDigits[i]);
}
}
} // MssGetCurrencyFormattedByLocale

/// <summary>
/// Gets the C# supported Locales and their parametrizations.
/// </summary>
public void MssGetLocales(out RLLocaleRecordList ssListofLocals) {
ssListofLocals = new RLLocaleRecordList();

foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
{
var nfi = cultureInfo.NumberFormat;

STLocaleStructure ssLocal = new STLocaleStructure(null);
ssLocal.ssName                     = cultureInfo.DisplayName;
ssLocal.ssRFC4646                  = cultureInfo.Name;
ssLocal.ssCurrencyDecimalDigits     = nfi.CurrencyDecimalDigits.ToString();
ssLocal.ssCurrencyDecimalSeparator  = nfi.CurrencyDecimalSeparator;
ssLocal.ssCurrencyGroupSeparator    = nfi.CurrencyGroupSeparator;
ssLocal.ssCurrencyGroupSizes        = "[" + string.Join(",", nfi.CurrencyGroupSizes) + "]";
ssLocal.ssCurrencyNegativePattern   = (nfi.CurrencyNegativePattern < NegativePatterns.Length
                                      ? NegativePatterns[nfi.CurrencyNegativePattern]
                                      : nfi.CurrencyNegativePattern.ToString())
                                        .Replace("-", nfi.NegativeSign)
                                        .Replace("$", nfi.CurrencySymbol);
ssLocal.ssCurrencyPositivePattern   = (nfi.CurrencyPositivePattern < PositivePatterns.Length
                                      ? PositivePatterns[nfi.CurrencyPositivePattern]
                                      : nfi.CurrencyPositivePattern.ToString())
                                        .Replace("$", nfi.CurrencySymbol);
ssLocal.ssNativeDigits              = string.Join(",", nfi.NativeDigits);
ssLocal.ssNegativeSign              = nfi.NegativeSign;
ssLocal.ssCurrencySymbol            = nfi.CurrencySymbol;

RCLocaleRecord ssLocaleRecord = new RCLocaleRecord(null);
ssLocaleRecord.ssSTLocale = ssLocal;
ssListofLocals.Add(ssLocaleRecord);
}
} // MssGetLocales

// ── Chinese number conversion ─────────────────────────────────────────────

private string ChineseSmallNumberConvert(long number, string appendedNumber, string[] digits)
{
if (number >= 10000)
throw new ArgumentOutOfRangeException("number", "number must be less than 10000");

StringBuilder b = new StringBuilder();
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

private string ChineseNumberConvert(long number, string[] digits, string[] bigNumbers)
{
StringBuilder b = new StringBuilder();
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
if (b.Length >= 2 && b.ToString().Substring(0, 2) == String.Concat(digits[1], digits[10]))
return b.Remove(0, 1).ToString();

return b.ToString();
}

} // CssFormatCurrency

} // OutSystems.NssFormatCurrency
