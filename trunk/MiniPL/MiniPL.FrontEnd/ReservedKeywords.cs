using System.Collections.Generic;

namespace MiniPL.FrontEnd
{
    /// @author Jani Viherväs
    /// @version 28.2.2014
    /// 
    /// <summary>
    /// Reserved keywords (types are also reserved)
    /// </summary>
    public struct ReservedKeywords
    {
        /// <summary>
        /// "assert"
        /// </summary>
        public const string Assert = "assert";

        /// <summary>
        /// "do"
        /// </summary>
        public const string Do = "do";

        /// <summary>
        /// "end"
        /// </summary>
        public const string End = "end";

        /// <summary>
        /// "for"
        /// </summary>
        public const string For = "for";

        /// <summary>
        /// "in"
        /// </summary>
        public const string In = "in";

        /// <summary>
        /// "print"
        /// </summary>
        public const string Print = "print";

        /// <summary>
        /// "read"
        /// </summary>
        public const string Read = "read";

        /// <summary>
        /// "var"
        /// </summary>
        public const string Var = "var";


        /// <summary>
        /// ":="
        /// </summary>
        public const string Assignment = ":=";

        /// <summary>
        /// ":"
        /// </summary>
        public const string Colon = ":";

        /// <summary>
        /// ".."
        /// </summary>
        public const string Range = "..";

        /// <summary>
        /// ";"
        /// </summary>
        public const string Semicolon = ";";


        /// <summary>
        /// Returns all the reserved keywords in an order that doesn't mess up the longest matching rule, i.e. ":=" is before ":". 
        /// They are also ordered from shortest length to longest.
        /// </summary>
        /// <returns>All the reserved keywords</returns>
        public static IEnumerable<string> GetReservedKeywords()
        {
            return new[]
                       {
                           Assignment,
                           Colon,
                           Semicolon,
                           Range,
                           Do,
                           In,
                           End,
                           For,
                           Var,
                           Read,
                           Print,
                           Assert
                       };
        }
    }
}
