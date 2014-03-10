namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Tail expression for the Expression class
    /// </summary>
    public class ExpressionTail
    {
        /// <summary>
        /// Binary operator
        /// </summary>
        public string Operator { get; private set; }

        /// <summary>
        /// Operand
        /// </summary>
        public Value Operand { get; private set; }


        /// <summary>
        /// Creates a new tail expression
        /// </summary>
        /// <param name="op">Binary operator</param>
        /// <param name="operand">Operand. Can be a single value or another expression</param>
        public ExpressionTail(string op, Value operand)
        {
            Operator = op;
            Operand = operand;
        }
    }
}
