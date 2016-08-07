using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateParser
{
    public class ParserHelper
    {
        public static Predicate<char> AnyOf(params char[] chars)
        {
            return c => 
            {
                foreach (var allowed in chars)
                    if (c == allowed)
                        return true;

                return false;
            };
        }

        // --------------------

        public static Predicate<char> Not(Predicate<char> predicate) => ch => predicate(ch) ? false : true;
    }
}
