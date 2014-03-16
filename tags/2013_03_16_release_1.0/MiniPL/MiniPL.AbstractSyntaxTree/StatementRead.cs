using System;

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


        /// <summary>
        /// Reads a value and assigns it to a variable
        /// </summary>
        public override void Execute()
        {
            var value = Console.ReadLine();
            var variable = GetVariable(Identifier);

            var i = variable as VariableType<int>;
            var s = variable as VariableType<string>;
            var b = variable as VariableType<bool>;

            if ( i != null )
            {
                i.Value = int.Parse(value);
                return;
            }
            if ( b != null )
            {
                b.Value = bool.Parse(value);
                return;
            }
            if ( s != null )
            {
                s.Value = value;
            }
        }
    }
}
