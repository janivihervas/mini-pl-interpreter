namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Expression for nesting multiple expressions
    /// </summary>
    public class ValueExpression : Value
    {
        /// <summary>
        /// Expression
        /// </summary>
        public Expression Expression { get; set; }

        /// <summary>
        /// Creates a new expression to nest multiple expressions
        /// </summary>
        /// <param name="expression">Expression</param>
        public ValueExpression(Expression expression)
        {
            Expression = expression;
        }
    }
}
