namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Statement to execute a for loop
    /// </summary>
    public class StatementFor : Statement
    {
        /// <summary>
        /// Iterator's identifier
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// Starting value to iterate
        /// </summary>
        public Expression FirstExpression { get; private set; }

        /// <summary>
        /// Ending value to stop iterating
        /// </summary>
        public Expression SecondExpression { get; private set; }

        /// <summary>
        /// Statements to execute
        /// </summary>
        public Statements Statements { get; private set; }


        /// <summary>
        /// Creates a new for loop statement. Usage:
        /// for 'identifier' in 'firstExpression' .. 'secondExpression' do 'statements' end for;
        /// </summary>
        /// <param name="identifier">Iterator's identifier</param>
        /// <param name="firstExpression">Starting value</param>
        /// <param name="secondExpression">Ending value</param>
        /// <param name="statements">Statements to execute</param>
        public StatementFor(string identifier, Expression firstExpression, Expression secondExpression, Statements statements)
        {
            Identifier = identifier;
            FirstExpression = firstExpression;
            SecondExpression = secondExpression;
            Statements = statements;
        }
    }
}
