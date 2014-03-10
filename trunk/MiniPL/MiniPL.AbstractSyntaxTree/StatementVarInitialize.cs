namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Statement for initializing variable
    /// </summary>
    public class StatementVarInitialize : Statement
    {
        /// <summary>
        /// Variables identifier
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// Variables type
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Variables value
        /// </summary>
        public Expression Expression { get; private set; }


        /// <summary>
        /// Creates a new statement to initialize variables
        /// </summary>
        /// <param name="identifier">Variable's identifier</param>
        /// <param name="type">Variable's type</param>
        /// <param name="expression">Variable's value</param>
        public StatementVarInitialize(string identifier, string type, Expression expression)
        {
            Identifier = identifier;
            Type = type;
            Expression = expression;
        }
    }
}
