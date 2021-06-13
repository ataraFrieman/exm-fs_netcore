using Quze.Infrastruture.Extensions;
using System.Text.RegularExpressions;

namespace Quze.Models.Entities
{
    public class PhoneNumberDelete
    {
        public PhoneNumberDelete()
        {

        }

        public PhoneNumberDelete(string countryCode, string areaCode, string number)
        {
            CountryCode = countryCode;
            AreaCode = areaCode;
            Number = number;

            if (IsIsraelyNumber() && areaCode.IsNullOrEmpty() && number.IsNotNullOrEmpty() && number.Length > 7)
            {
                AreaCode = number.Substring(0, number.Length - 7);
                Number = number.Substring(number.Length - 7);
            }
        }

        private string countryCode;
        /// <summary>
        /// The country code
        /// </summary>
        public string CountryCode
        {
            get { return countryCode; }
            set
            {
                countryCode = value;

                countryCode = value.IsNull() ? string.Empty : value.Trim().Replace("-",string.Empty);
                if (!value.StartsWith('+'))
                    countryCode = "+" + value;
            }
        }

        private string areaCode;

        public string AreaCode
        {
            get { return areaCode; }
            set { areaCode = value.IsNull() ? string.Empty : value.Trim().Replace("-", string.Empty); }
        }

        private string number;
        public string Number
        {
            get { return number; }
            set { number = value.IsNull() ? string.Empty : value.Trim().Replace("-", string.Empty); }
        }

        public bool IsIsraelyNumber()
        {
            return CountryCode == "+972";
        }

        /// <summary>
        /// Represents a valid international format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return CountryCode + AreaCode.TrimStart('0') + Number;
        }

        public bool IsValid()
        {
            return IsValidPhoneNumber(this.ToString());
        }

        public static bool IsValidPhoneNumber(string number)
        {
            return Regex.Match(number, @"\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*").Success;
        }
    }
}
