using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextProcess.Provider.Helpers
{
    public static class StringExtensions
    {
        public static List<string> SeparateBySpaces(this string text) => text.Split(' ').ToList();

        public static int CountFrequencyByChar(this string text, char character) => text.Count(x => (x == character));

        public static string UrlDecode(this string text) => HttpUtility.UrlDecode(text);

    }
}
