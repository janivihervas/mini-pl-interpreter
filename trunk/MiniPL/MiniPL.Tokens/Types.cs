using System.Collections.Generic;

namespace MiniPL.Tokens
{
    /// @author Jani Viherväs
    /// @version 28.2.2014
    /// 
    /// <summary>
    /// Types
    /// </summary>
    public struct Types
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
        public static IEnumerable<string> GetTypes()
        {
            return new[] {Int, Bool, String};
        }
    }
}
