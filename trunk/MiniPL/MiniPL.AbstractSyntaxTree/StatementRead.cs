namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Statement to read input and assign it to a variable
    /// </summary>
    public class StatementRead : Statement
    {
        /// <summary>
        /// Identifier of the variable to assign a value to
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// Creates a new read statement
        /// </summary>
        /// <param name="identifier">Identifier of the variable to assign a value to</param>
        public StatementRead(string identifier)
        {
            Identifier = identifier;
        }
    }
}
