namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Statement for assigning a value to variable
    /// </summary>
    public class StatementVarAssignment : Statement
    {
        /// <summary>
        /// Variables identifier
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Value to assign to the variable
        /// </summary>
        public Expression Expression { get; set; }


        /// <summary>
        /// Creates a new statement to assign variables
        /// </summary>
        /// <param name="identifier">Variable's identifier</param>
        /// <param name="expression">Value to assign to the variable</param>
        public StatementVarAssignment(string identifier, Expression expression)
        {
            Identifier = identifier;
            Expression = expression;
        }


        /// <summary>
        /// Assigns a value to the variable
        /// </summary>
        public override void Execute()
        {
            var variable = GetVariable(Identifier);
            var i = variable as VariableType<int>;
            var b = variable as VariableType<bool>;
            var s = variable as VariableType<string>;

            if (i != null)
            {
                i.Value = Expression.EvaluateInt();
                return;
            }
            if ( b != null )
            {
                b.Value = Expression.EvaluateBool();
                return;
            }
            if ( s != null )
            {
                s.Value = Expression.EvaluateString();
            }
        }
    }
}
