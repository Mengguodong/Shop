using System.Text.RegularExpressions;

namespace SFO2O.Framework.Uitl
{
    public static class ValidationHelper
    {
        public static bool IsComplexPassword(this string password)
        {
            const string number = "^[0-9]{6,32}$";
            const string letter = "^[a-zA-Z]{6,32}$";
            const string sign = @"^[@!#\$%&'\*\+\-\/=\?\^_`{\|}~]{6,32}$";
            const string china = @"[\u4e00-\u9fa5]";

            var matchNumber = Regex.Match(password, number, RegexOptions.IgnoreCase);
            var matchLetter = Regex.Match(password, letter, RegexOptions.IgnoreCase);
            var matchSign = Regex.Match(password, sign, RegexOptions.IgnoreCase);
            var matchChina = Regex.Match(password, china, RegexOptions.IgnoreCase);

            if (matchNumber.Success || matchLetter.Success || matchSign.Success || matchChina.Success)
            {
                return false;
            }
            return true;

        }

        public static bool IsValidMobile(this string mobile)
        {
            string pattern = "^1[34578]\\d{9}$";
            var match = Regex.Match(mobile, pattern, RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsValidEmail(this string email)
        {
            string pattern = "^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$";
            var match = Regex.Match(email, pattern, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
