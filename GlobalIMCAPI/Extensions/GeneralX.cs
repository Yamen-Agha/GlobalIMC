using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalIMCAPI.Extensions
{
    public static class GeneralX
    {
        public static bool ContainsOrStartsWithAny(this string[] Target,string Text)
        {
            for (int i = 0; i < Target.Length; i++)
            {
                string TrimedWord = Target[i].Trim();
                if (Text.Equals(TrimedWord) || Text.StartsWith(TrimedWord)) return true;
            }

            return false;
        }
    }
}
