using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace AuthService.Data
{

    /// <summary>
    /// I don't use interface cause all of this methods are statics
    /// This class contains methods like upper and lower text...
    /// </summary>
    public class CommonResources
    {

        public static string UpperCaseText(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string pattern = @"(?<before>\w+) . (?<after>\w+)";
                StringBuilder sb = new StringBuilder();
                MatchCollection matches = Regex.Matches(input, pattern);

                for (int i = 0; i < matches.Count; i++)
                {
                    //Console.WriteLine("before:" + matches[i].Groups["before"].ToString());
                    //Console.WriteLine("after:" + matches[i].Groups["after"].ToString());
                }
                return input.ToUpper();
            }
            else
            {
                return null;
            }

        }

        public static string LowerCaseText(string input)
        {
            string pattern = @"(?<before>\w+) . (?<after>\w+)";
            StringBuilder sb = new StringBuilder();
            MatchCollection matches = Regex.Matches(input, pattern);

            for (int i = 0; i < matches.Count; i++)
            {
                //Console.WriteLine("before:" + matches[i].Groups["before"].ToString());
                //Console.WriteLine("after:" + matches[i].Groups["after"].ToString());
            }
            return input.ToLower(); ;
        }



    }

}
