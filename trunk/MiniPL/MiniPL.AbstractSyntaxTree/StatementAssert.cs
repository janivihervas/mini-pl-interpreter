namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Statement to assert whether the expression is true
    /// </summary>
    public class StatementAssert : Statement
    {
        /// <summary>
        /// Expression to assert
        /// </summary>
        public Expression Expression { get; set; }

        /// <summary>
        /// Creates a new statement to assert expressions
        /// </summary>
        /// <param name="expression">Expression to assert</param>
        public StatementAssert(Expression expression)
        {
            Expression = expression;
        }
    }
}
