namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Statement to print expressions value
    /// </summary>
    public class StatementPrint : Statement
    {
        /// <summary>
        /// Expression to print
        /// </summary>
        public Expression Expression { get; private set; }

        /// <summary>
        /// Creates a new print statement
        /// </summary>
        /// <param name="expression">Expression to print</param>
        public StatementPrint(Expression expression)
        {
            Expression = expression;
        }
    }
}
