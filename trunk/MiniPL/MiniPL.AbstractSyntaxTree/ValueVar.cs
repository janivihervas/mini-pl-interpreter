namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Class to hold a variable's value
    /// </summary>
    public class ValueVar : Value
    {
        /// <summary>
        /// Variable's identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Creates a new object to hold variable's value
        /// </summary>
        /// <param name="identifier"></param>
        public ValueVar(string identifier)
        {
            Identifier = identifier;
        }
    }
}
