using System.Collections.Generic;

namespace MiniPL.FrontEnd
{
    /// @author Jani Viherväs
    /// @version 28.2.2014
    /// 
    /// <summary>
    /// Types
    /// </summary>
    public struct Type
    {
        /// <summary>
        /// Boolean type
        /// </summary>
        public const string Bool = "bool";

        /// <summary>
        /// Integer type
        /// </summary>
        public const string Int = "int";

        /// <summary>
        /// String type
        /// </summary>
        public const string String = "string";

        /// <summary>
        /// Returns all the types in an order from shortest length to longest.
        /// </summary>
        /// <returns>All the types</returns>
        public static IEnumerable<string> Types()
        {
            return new[] {Int, Bool, String};
        }
    }
}
