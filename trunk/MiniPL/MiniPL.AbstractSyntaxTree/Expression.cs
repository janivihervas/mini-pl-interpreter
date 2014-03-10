namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// First expression. Can hold only a value (Operand) or expression
    /// </summary>
    public class Expression
    {
        /// <summary>
        /// Operand
        /// </summary>
        public Value Operand { get; private set; }

        /// <summary>
        /// Tail expression
        /// </summary>
        public ExpressionTail Tail { get; private set; }


        /// <summary>
        /// Creates a new expression to evaluate
        /// </summary>
        /// <param name="operand">Operand</param>
        /// <param name="tail">Tail expression, can be null</param>
        public Expression(Value operand, ExpressionTail tail)
        {
            Operand = operand;
            Tail = tail;
        }
    }
}
