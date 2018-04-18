using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoGenExcel
{
    public static class IdxOfAll
    {
        public static IEnumerable<int> findIdx(this string sourceString, string matchString)
        {
            matchString = Regex.Escape(matchString);
            return from Match match in Regex.Matches(sourceString, matchString) select match.Index;
        }
    }
}
