using OutSystems.ExternalLibraries.SDK;
using System.Globalization;
using System.Text;

namespace OutSystems.FormatCurrency
{
    public class FormatCurrency : IFormatCurrency
    {

        //chinese support functions
        private string[] chineseNumbers = new string[] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "百", "千" }; // 0..9, 10, 100, 1000
        private string[] chineseNumbersfinancial = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾", "佰", "仟" }; // 0..9, 10, 100, 1000
        private string[] chineseBigNums = new string[] { "万", "亿", "兆", "京" };  // 10^4, 10^8, 10^12, 10^16
        private string[] chineseBigNumsfinancial = new string[] { "萬", "亿", "兆", "京" };  // 10^4, 10^8, 10^12, 10^16
        private string ChineseSmallNumberConvert(long number, string appendedNumber, string[] numbers)
        {
            if (number >= 10000)
            {
                throw (new ArgumentOutOfRangeException("n", "number must be less than 10000"));
            }

            StringBuilder b = new StringBuilder();

            int n = 1000;
            int idx = 12;

            do
            {
                if (number >= n)                                // bug fix: changed from > (thanks to Alexandre Realinho)
                {

                    b.Append(numbers[number / n]);

                    if (n >= 10)
                    {
                        b.Append(numbers[idx]);
                    }

                    number %= n;
                }
                else if (number / n == 0 && b.Length != 0 && number != 0 && b.ToString(b.Length - 1, 1) != chineseNumbersfinancial[0])
                { // gap 0 for small numbers
                    b.Append(chineseNumbersfinancial[0]);
                }
                else if (n == 1000 && number / n == 0 && appendedNumber.Length != 0 && number != 0 && appendedNumber[appendedNumber.Length - 1].ToString() != chineseNumbersfinancial[0])
                { // gap 0 for the big numbers
                    b.Append(chineseNumbersfinancial[0]);
                }

                n /= 10; // 100 / 10 = 10
                idx--; // 11 - 1 = 10
            }
            while (n > 0);

            return b.ToString();
        }
        private string ChineseNumberConvert(long _number, string[] numbers, string[] bigNumbers)
        {
            StringBuilder b = new StringBuilder();
            long baseNum = 10000000000000000;       // 10^16

            if (_number == 0)
            {
                b.Append(numbers[0]);
            }
            else
            {
                int baseIdx = 3;

                do
                {
                    long n = _number / baseNum;
                    if (n > 0)
                    {
                        b.Append(ChineseSmallNumberConvert(n, b.ToString(), numbers));
                        if (baseIdx >= 0)
                        {

                            b.Append(bigNumbers[baseIdx]);

                        }
                    }

                    _number %= baseNum;
                    baseNum /= 10000;
                    baseIdx--;
                }
                while (baseNum > 0);
            }
            if (b.Length >= 2 && ((b.ToString()).Substring(0, 2) == String.Concat(numbers[1], numbers[10])))
            {                   //bug fix: remove 一 from the first 十 of the string
                return b.Remove(0, 1).ToString();
            }
            return b.ToString();
        }

        //end chinese support functions

        public void GetDecimalFromLocaleDecimalString(string InputLocalelDecimalString, string Locale, string Currency, out bool IsValidDecimal, out int ErrorMessageCode, out string ErrorMessage, out decimal Decimal)
        {
            IsValidDecimal = true;
            ErrorMessageCode = 0;
            ErrorMessage = "";
            Decimal = 0.0m;
            CultureInfo culture;
            // Return if string is empty
            if (String.IsNullOrEmpty(InputLocalelDecimalString))
            {
                ErrorMessage = "String Empty";
                ErrorMessageCode = 1;
                return;
            }
            // Instantiate CultureInfo object for the user's locale
            try
            {
                culture = CultureInfo.GetCultureInfo(Locale);
            }
            catch (CultureNotFoundException e)
            {
                ErrorMessage = "Locale Invalid/Not Provided";
                ErrorMessageCode = 2;
                return;
            }
            // Convert user input from a string to a number
            try
            {
                NumberFormatInfo numberFormatInfo = (NumberFormatInfo)culture.NumberFormat.Clone();
                int count = 0;
                foreach (string item in numberFormatInfo.NativeDigits)
                {
                    if (InputLocalelDecimalString.IndexOf(item) > -1)
                    {
                        string scount = count.ToString();
                        InputLocalelDecimalString = InputLocalelDecimalString.Replace(item, scount);
                    }
                    count++;
                }
                if (!Currency.Equals(""))
                {
                    numberFormatInfo.CurrencySymbol = Currency;

                }
                Decimal = Decimal.Parse(InputLocalelDecimalString, NumberStyles.Currency, numberFormatInfo);

            }
            catch (FormatException e)
            {
                ErrorMessage = "FormatException [" + e + "]";
                ErrorMessageCode = 3;
                return;
            }
            catch (Exception e)
            {
                ErrorMessage = "Other Errors [" + e + "]";
                ErrorMessageCode = 4;
                return;
            }
        }

        public void GetCurrencyFormattedByLocale(string Locale, decimal Decimal, bool HasCurrency, string Currency, bool UseNativeDigits, bool UseChineseExtendedNumbers, bool UseFinancialChinese, out string FormattedText)
        {

            Dictionary<int, String> patterns = new Dictionary<int, String>();
            string[] patternStrings = { "($n)", "-$n", "$-n", "$n-", "(n$)",
                                 "-n$", "n-$", "n$-", "-n $", "-$ n",
                                 "n $-", "$ n-", "$ -n", "n- $", "($ n)",
                                 "(n $)" };
            for (int ctr = patternStrings.GetLowerBound(0);
                 ctr <= patternStrings.GetUpperBound(0); ctr++)
                patterns.Add(ctr, patternStrings[ctr]);

            Dictionary<int, String> patternspositive = new Dictionary<int, String>();
            string[] patternStrings2 = { "$n", "n$", "$ n", "n $" };
            for (int ctr = patternStrings2.GetLowerBound(0);
                 ctr <= patternStrings2.GetUpperBound(0); ctr++)
                patternspositive.Add(ctr, patternStrings2[ctr]);

            FormattedText = "";
            CultureInfo ci;
            try
            {
                ci = CultureInfo.GetCultureInfo(Locale);
            }
            catch (Exception e)
            {
                ci = CultureInfo.GetCultureInfo("");
            }
            NumberFormatInfo numberFormatInfo = (NumberFormatInfo)ci.NumberFormat.Clone();
            if (!Currency.Equals(""))
            {
                numberFormatInfo.CurrencySymbol = Currency;

            }
            if (!HasCurrency)
            {
                numberFormatInfo.CurrencySymbol = "";

            }

            if (UseChineseExtendedNumbers && UseNativeDigits && Locale.IndexOf("zh") == 0)
            {
                long wholePart = (long)Math.Truncate(Math.Abs(Decimal));
                string formattedChin = "";
                if (("" + Math.Abs(Decimal)).IndexOf(".") > 0)
                {

                    int lengthDecimal = ("" + Math.Abs(Decimal)).Substring(("" + Math.Abs(Decimal)).IndexOf(".")).Length - 1;
                    long decimalPart = (long)((decimal)Math.Pow(10, lengthDecimal) * (+Math.Abs(Decimal) - Math.Truncate(+Math.Abs(Decimal))));
                    string decimalPartString = decimalPart.ToString();
                    numberFormatInfo.CurrencyDecimalSeparator = "点";
                    if (UseFinancialChinese)
                    {
                        int count = 0;
                        foreach (string item in chineseNumbersfinancial)
                        {
                            string scount = count.ToString();
                            decimalPartString = decimalPartString.Replace(scount, item);
                            count++;
                            if (count > 9)
                            {
                                break;
                            }
                        }
                        formattedChin = ChineseNumberConvert(wholePart, chineseNumbersfinancial, chineseBigNumsfinancial) +
                                    numberFormatInfo.CurrencyDecimalSeparator +
                                    decimalPartString;
                    }
                    else
                    {
                        int count = 0;
                        foreach (string item in chineseNumbers)
                        {
                            string scount = count.ToString();
                            decimalPartString = decimalPartString.Replace(scount, item);
                            count++;
                            if (count > 9)
                            {
                                break;
                            }
                        }
                        formattedChin = ChineseNumberConvert(wholePart, chineseNumbers, chineseBigNums) +
                                    numberFormatInfo.CurrencyDecimalSeparator +
                                    decimalPartString;
                    }


                }
                else
                {
                    if (UseFinancialChinese)
                    {
                        formattedChin = ChineseNumberConvert(wholePart, chineseNumbersfinancial, chineseBigNumsfinancial);
                    }
                    else
                    {
                        formattedChin = ChineseNumberConvert(wholePart, chineseNumbers, chineseBigNums);
                    }
                }
                if (Decimal >= 0)
                {
                    FormattedText = patternspositive[numberFormatInfo.CurrencyPositivePattern].Replace("n", formattedChin).Replace("$", numberFormatInfo.CurrencySymbol);
                }
                else
                {
                    FormattedText = patterns[numberFormatInfo.CurrencyNegativePattern].Replace("n", formattedChin).Replace("-", numberFormatInfo.NegativeSign).Replace("$", numberFormatInfo.CurrencySymbol);
                }


            }
            else
            {
                FormattedText = Decimal.ToString("C", numberFormatInfo);
                if (UseNativeDigits)
                {
                    int count = 0;
                    foreach (string item in numberFormatInfo.NativeDigits)
                    {
                        string scount = count.ToString();
                        FormattedText = FormattedText.Replace(scount, item);
                        count++;
                    }
                }
            }

        }

        public void GetLocales(out List<Locale> ListofLocals)
        {
            ListofLocals = new List<Locale>();

           

            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                ListofLocals.Add(new Locale(cultureInfo));
            }
        }
    }
    [OSStructure]
    public struct Locale
    {
        public string Name;
        public string RFC4646;
        public string CurrencyDecimalDigits;
        public string CurrencyDecimalSeparator;
        public string CurrencyGroupSeparator;
        public string CurrencyGroupSizes;
        public string CurrencyNegativePattern;
        public string CurrencyPositivePattern;
        public string NegativeSign;
        public string CurrencySymbol;
        public string NativeDigits;
        
        private string[] patternNegative = { "($n)", "-$n", "$-n", "$n-", "(n$)",
                                 "-n$", "n-$", "n$-", "-n $", "-$ n",
                                 "n $-", "$ n-", "$ -n", "n- $", "($ n)",
                                 "(n $)" };
        
        private string[] patternsPositive = { "$n", "n$", "$ n", "n $" };
        
        public Locale(CultureInfo cultureInfo)
        {
            this.CurrencySymbol = cultureInfo.NumberFormat.CurrencySymbol;
            this.Name = cultureInfo.DisplayName;
            this.RFC4646 = cultureInfo.Name;
            this.CurrencyDecimalDigits = cultureInfo.NumberFormat.CurrencyDecimalDigits.ToString();
            this.CurrencyDecimalSeparator = cultureInfo.NumberFormat.CurrencyDecimalSeparator;
            this.CurrencyGroupSeparator = cultureInfo.NumberFormat.CurrencyGroupSeparator;
            //Error correction for pattern 16 
            this.CurrencyNegativePattern = patternNegative[(cultureInfo.NumberFormat.CurrencyNegativePattern > 15) ? 2 : cultureInfo.NumberFormat.CurrencyNegativePattern];
            this.CurrencyPositivePattern = patternsPositive[cultureInfo.NumberFormat.CurrencyPositivePattern];
            this.CurrencyGroupSizes = "[" + string.Join(",", cultureInfo.NumberFormat.CurrencyGroupSizes) + "]";
            this.NativeDigits = string.Join(",", cultureInfo.NumberFormat.NativeDigits);
            this.NegativeSign = cultureInfo.NumberFormat.NegativeSign;
        }

    }
}
