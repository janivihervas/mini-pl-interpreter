using MiniPL.Tokens;

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


        /// <summary>
        /// Initializes a new variable
        /// </summary>
        public override void Execute()
        {
            if (Expression == null)
            {
                CreateEmptyVariable();
            }
            else
            {
                CreateVariable();
            }
            
        }


        /// <summary>
        /// Creates a variable with value
        /// </summary>
        private void CreateVariable()
        {
            switch (Type)
            {
                case Types.Int:
                    {
                        AddNewVariable(new VariableType<int>(Identifier, Expression.EvaluateInt()));
                        return;
                    }
                case Types.Bool:
                    {
                        AddNewVariable(new VariableType<bool>(Identifier, Expression.EvaluateBool()));
                        return;
                    }
                case Types.String:
                    {
                        AddNewVariable(new VariableType<string>(Identifier, Expression.EvaluateString()));
                        return;
                    }
            }
        }


        /// <summary>
        /// Creates an empty variable
        /// </summary>
        private void CreateEmptyVariable()
        {
            switch (Type)
            {
                case Types.Int:
                    {
                        AddNewVariable(new VariableType<int>(Identifier));
                        return;
                    }
                case Types.Bool:
                    {
                        AddNewVariable(new VariableType<bool>(Identifier));
                        return;
                    }
                case Types.String:
                    {
                        AddNewVariable(new VariableType<string>(Identifier));
                        return;
                    }
            }
        }
    }
}
