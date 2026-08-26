using System.Text.RegularExpressions;
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

		/// <summary>
		/// Converts a Locale Decimal String (a decimal that was written in in a certain locale) into a valid Decimal.
		/// For now &quot;zh&quot; locale that uses the extended numbers and financial numbers will not be converted accordingly.
		/// Error Message Codes and meanings:
		/// 0 - &quot;&quot; - No Error
		/// 1 - &quot;String Empty&quot; - The string was empty
		/// 2 - &quot;Locale Invalid/Not Provided&quot; - No Locale was found
		/// 3 - &quot;FormatException&quot; - an error in the format of the number
		/// 4 - &quot;Other Errors [x]&quot; - other errors - log represented in x
        /// 
		/// </summary>
		/// <param name="ssInputLocalelDecimalString">The String containing the Locale Decimal to convert into Decimal</param>
		/// <param name="ssLocale">The Locale that the string was written on. (It will try both native and 0-9 numbers from the locale).</param>
		/// <param name="ssCurrency">The Currency to use, if &quot;&quot; it uses the default currency of the locale</param>
        /// <param name="ssIsValidDecimal">If the converted Decimal is a valid one, if not then the Error Message will have the reason that the conversion failed.</param>
		/// <param name="ssErrorMessageCode">The code of the Error Message.</param>
		/// <param name="ssErrorMessage">Error Message in case the converted Decimal is not valid.</param>
		/// <param name="ssDecimal">The Decimal converted, if the IsValidDecimal is notTrue, then this will be the default value 0.0.</param>
		public void MssGetDecimalFromLocaleDecimalString(string ssInputLocalelDecimalString, string ssLocale, string ssCurrency, out bool ssIsValidDecimal, out int ssErrorMessageCode, out string ssErrorMessage, out decimal ssDecimal) {
			ssIsValidDecimal = true;
			ssErrorMessageCode = 0;
			ssErrorMessage = "";
			ssDecimal = 0.0m;
            CultureInfo culture;
            // Return if string is empty
            if (String.IsNullOrEmpty(ssInputLocalelDecimalString)){
                ssErrorMessage = "String Empty";
                ssErrorMessageCode = 1;
                return;
            }
            // Instantiate CultureInfo object for the user's locale
            try{
                culture = CultureInfo.GetCultureInfo(ssLocale);
            }catch(CultureNotFoundException e){
                ssErrorMessage = "Locale Invalid/Not Provided";
                ssErrorMessageCode = 2;
                return;
            }
            // Convert user input from a string to a number
            try
            {
                NumberFormatInfo numberFormatInfo = (NumberFormatInfo)culture.NumberFormat.Clone();
                int count = 0;
                foreach (string item in numberFormatInfo.NativeDigits)
                {
                    if(ssInputLocalelDecimalString.IndexOf(item)>-1){
                        string scount = count.ToString();
                        ssInputLocalelDecimalString = ssInputLocalelDecimalString.Replace(item, scount);
                    }
                    count++;   
                }
                 if (!ssCurrency.Equals(""))
                {
                    numberFormatInfo.CurrencySymbol = ssCurrency;

                }
                ssDecimal = Decimal.Parse(ssInputLocalelDecimalString, NumberStyles.Currency, numberFormatInfo);
                
            }
            catch (FormatException e)
            {
                ssErrorMessage = "FormatException ["+e+"]";
                ssErrorMessageCode = 3;
                return;
            }
            catch (Exception e)
            {
                ssErrorMessage = "Other Errors ["+e+"]";
                ssErrorMessageCode = 4;
                return;
            }
		} // MssGetDecimalFromLocaleDecimalString


        /// <summary>
		/// Formats the Decimal based on the Locale provided.
		/// </summary>
		/// <param name="ssLocale">The Locale of the format, see RFC4646 from GetLocales</param>
		/// <param name="ssDecimal">The number to format</param>
		/// <param name="ssHasCurrency">If the number has the currency symbol.</param>
		/// <param name="ssCurrency">The Currency to use, if &quot;&quot; it uses the default currency of the locale</param>
		/// <param name="ssUseNativeDigits">If it uses the native symbols for 0-9.</param>
		/// <param name="ssUseChineseExtendedNumbers">If the locale starts with "zh", and this is true, the native numbers will be using the chinese unit system instead of the latim or partial chinese numbers.</param>
        /// <param name="ssUseFinancialChinese">If the locale starts with "zh", and this is true, it will use financial chinese numeration example:  零 "líng" instead of 〇, normally in financial applications 零 is used.</param>
		public void MssGetCurrencyFormattedByLocale(string ssLocale, decimal ssDecimal, bool ssHasCurrency, string ssCurrency, bool ssUseNativeDigits, bool ssUseChineseExtendedNumbers, bool ssUseFinancialChinese, out string ssFormattedText) {
            
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

            ssFormattedText = "";
            CultureInfo ci;
            try{
                ci = CultureInfo.GetCultureInfo(ssLocale);
            }catch(Exception e){
                ci = CultureInfo.GetCultureInfo("");
            }
            NumberFormatInfo numberFormatInfo = (NumberFormatInfo)ci.NumberFormat.Clone();
            if (!ssCurrency.Equals(""))
            {
                numberFormatInfo.CurrencySymbol = ssCurrency;

            }
            if (!ssHasCurrency)
            {
                numberFormatInfo.CurrencySymbol = "";

            }

            if (ssUseChineseExtendedNumbers && ssUseNativeDigits && ssLocale.IndexOf("zh") == 0){
                long wholePart = (long)Math.Truncate(Math.Abs(ssDecimal));
                 string formattedChin ="";
                if((""+Math.Abs(ssDecimal)).IndexOf(".")>0){
                   
                    int lengthDecimal = (""+Math.Abs(ssDecimal)).Substring((""+Math.Abs(ssDecimal)).IndexOf(".")).Length-1;
                    long decimalPart = (long)((decimal)Math.Pow(10,lengthDecimal)*(+Math.Abs(ssDecimal)-Math.Truncate(+Math.Abs(ssDecimal))));
                    string decimalPartString = decimalPart.ToString();
                    numberFormatInfo.CurrencyDecimalSeparator = "点";
                    if(ssUseFinancialChinese){
                        int count = 0;
                        foreach (string item in chineseNumbersfinancial)
                        {
                            string scount = count.ToString();
                            decimalPartString = decimalPartString.Replace(scount, item);
                            count++;
                            if(count >9){
                                break;
                            }
                        }
                        formattedChin = ChineseNumberConvert(wholePart, chineseNumbersfinancial,chineseBigNumsfinancial) +
                                    numberFormatInfo.CurrencyDecimalSeparator +
                                    decimalPartString;
                    }else{
                        int count = 0;
                        foreach (string item in chineseNumbers)
                        {
                            string scount = count.ToString();
                            decimalPartString = decimalPartString.Replace(scount, item);
                            count++;
                            if(count >9){
                                break;
                            }
                        }
                        formattedChin = ChineseNumberConvert(wholePart, chineseNumbers,chineseBigNums) +
                                    numberFormatInfo.CurrencyDecimalSeparator +
                                    decimalPartString;
                    }
                    
                   
                }else{
                    if(ssUseFinancialChinese){
                        formattedChin = ChineseNumberConvert(wholePart, chineseNumbersfinancial,chineseBigNumsfinancial);
                    }else{
                        formattedChin = ChineseNumberConvert(wholePart, chineseNumbers,chineseBigNums);
                    }
                }
                if(ssDecimal>=0){
                  ssFormattedText =  patternspositive[numberFormatInfo.CurrencyPositivePattern].Replace("n",formattedChin).Replace("$", numberFormatInfo.CurrencySymbol);
                }else{
                  ssFormattedText =  patterns[numberFormatInfo.CurrencyNegativePattern].Replace("n",formattedChin).Replace("-", numberFormatInfo.NegativeSign).Replace("$", numberFormatInfo.CurrencySymbol);
                }
                

            }else{
                ssFormattedText = ssDecimal.ToString("C", numberFormatInfo);
                if (ssUseNativeDigits)
                {
                    int count = 0;
                    foreach (string item in numberFormatInfo.NativeDigits)
                    {
                        string scount = count.ToString();
                        ssFormattedText = ssFormattedText.Replace(scount, item);
                        count++;
                    }
                }
            }
            
        } // MssGetCurrencyFormattedByLocale

		/// <summary>
		/// Gets the C# supported Locales and their parametrizations.
		/// </summary>
		/// <param name="ssListofLocals"></param>
		public void MssGetLocales(out RLLocaleRecordList ssListofLocals) {
            ssListofLocals = new RLLocaleRecordList();

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


            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                STLocaleStructure ssLocal = new STLocaleStructure();
                ssLocal.ssCurrencySymbol = cultureInfo.NumberFormat.CurrencySymbol;
                ssLocal.ssName = cultureInfo.DisplayName;
                ssLocal.ssRFC4646 = cultureInfo.Name;
                ssLocal.ssCurrencyDecimalDigits = cultureInfo.NumberFormat.CurrencyDecimalDigits.ToString();
                ssLocal.ssCurrencyDecimalSeparator = cultureInfo.NumberFormat.CurrencyDecimalSeparator;
                ssLocal.ssCurrencyGroupSeparator = cultureInfo.NumberFormat.CurrencyGroupSeparator;
                ssLocal.ssCurrencyGroupSizes = "[" + string.Join(",", cultureInfo.NumberFormat.CurrencyGroupSizes) + "]";
                ssLocal.ssCurrencyNegativePattern = patterns[cultureInfo.NumberFormat.CurrencyNegativePattern].Replace("-", cultureInfo.NumberFormat.NegativeSign).Replace("$", cultureInfo.NumberFormat.CurrencySymbol);
                ssLocal.ssCurrencyPositivePattern = patternspositive[cultureInfo.NumberFormat.CurrencyPositivePattern].Replace("$", cultureInfo.NumberFormat.CurrencySymbol);
                ssLocal.ssNativeDigits = string.Join(",", cultureInfo.NumberFormat.NativeDigits);
                ssLocal.ssNegativeSign = cultureInfo.NumberFormat.NegativeSign;
                RCLocaleRecord ssLocaleRecord = new RCLocaleRecord();
                ssLocaleRecord.ssSTLocale = ssLocal;
                ssListofLocals.Add(ssLocaleRecord);
            }
        } // MssGetLocales
        
        //chinese support functions
           private string[] chineseNumbers = new string[] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "百", "千" }; // 0..9, 10, 100, 1000
           private string[] chineseNumbersfinancial = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "拾", "佰", "仟" }; // 0..9, 10, 100, 1000
           private string[] chineseBigNums = new string[] { "万", "亿", "兆", "京" };  // 10^4, 10^8, 10^12, 10^16
           private string[] chineseBigNumsfinancial = new string[] { "萬", "亿", "兆", "京" };  // 10^4, 10^8, 10^12, 10^16
            
           private string ChineseSmallNumberConvert(long number,string appendedNumber, string[] numbers)
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
                    if (number >= n)								// bug fix: changed from > (thanks to Alexandre Realinho)
                    {
                        
                        b.Append(numbers[number / n]);
                        
                        if (n >= 10)
                        {
                            b.Append(numbers[idx]); 
                        }

                        number %= n;
                    }else if (number / n == 0 && b.Length != 0 && number != 0 && b.ToString(b.Length-1, 1) != chineseNumbersfinancial[0] ){ // gap 0 for small numbers
                        b.Append(chineseNumbersfinancial[0]);
                    }else if (n == 1000 && number / n == 0 && appendedNumber.Length != 0 && number != 0 && appendedNumber[appendedNumber.Length-1].ToString() != chineseNumbersfinancial[0] ){ // gap 0 for the big numbers
                        b.Append(chineseNumbersfinancial[0]);
                    }

                    n /= 10; // 100 / 10 = 10
                    idx--; // 11 - 1 = 10
                }
                while (n > 0);
               
                return b.ToString();
            }

           private string ChineseNumberConvert(long _number, string[] numbers,string[] bigNumbers)
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
                            b.Append(ChineseSmallNumberConvert(n,b.ToString(),numbers));
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
                if(b.Length >= 2 && ((b.ToString()).Substring(0, 2)==String.Concat(numbers[1],numbers[10]))){					//bug fix: remove 一 from the first 十 of the string
				    return b.Remove(0,1).ToString();
			    }
                return b.ToString();
            }
            //private string ChineseToNumberConverter(string number,bool _financial){
            //    
            //}
            
            
            
            
            //end chinese support functions

            





	} // CssFormatCurrency

} // OutSystems.NssFormatCurrency

