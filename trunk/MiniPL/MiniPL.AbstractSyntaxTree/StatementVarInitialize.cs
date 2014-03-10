using System;
using MiniPL.Exceptions;
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

        public override void Execute()
        {
            if (Expression == null)
            {
                CreateEmptyVariable();
            }
            else if (Expression.Tail == null)
            {
                CreateSimpleValueVariable();
            }
        }

        private void CreateSimpleValueVariable()
        {
            switch (Type)
            {
                case Types.Int:
                    {
                        var value = Expression.Operand as ValueType<int>;
                        if (value != null)
                        {
                            AddNewVariable(new VariableType<int>(Identifier, value.Value));
                            return;
                        }
                        var variableValue = Expression.Operand as ValueVar;
                        if (variableValue != null)
                        {
                            var variable = GetVariable(variableValue.Identifier) as VariableType<int>;
                            if (variable != null)
                            {
                                AddNewVariable(Identifier, variable.Value);
                                return;
                            }
                            throw new AbstractSyntaxTreeException(String.Format("Variable '{0}' was not the type of {1}", variableValue.Identifier, Types.Int));
                        }
                        throw new AbstractSyntaxTreeException(String.Format("The value to set for {0} was not of the type {1}.", Identifier, Types.Int));
                    }
                case Types.Bool:
                    {
                        var value = Expression.Operand as ValueType<bool>;
                        if ( value != null )
                        {
                            AddNewVariable(new VariableType<bool>(Identifier, value.Value));
                            return;
                        }
                        var variableValue = Expression.Operand as ValueVar;
                        if ( variableValue != null )
                        {
                            var variable = GetVariable(variableValue.Identifier) as VariableType<bool>;
                            if ( variable != null )
                            {
                                AddNewVariable(Identifier, variable.Value);
                                return;
                            }
                            throw new AbstractSyntaxTreeException(String.Format("Variable '{0}' was not the type of {1}", variableValue.Identifier, Types.Bool));
                        }
                        throw new AbstractSyntaxTreeException(String.Format("The value to set for {0} was not of the type {1}.", Identifier, Types.Bool));
                    }
                case Types.String:
                    {
                        var value = Expression.Operand as ValueType<string>;
                        if ( value != null )
                        {
                            AddNewVariable(new VariableType<string>(Identifier, value.Value));
                            return;
                        }
                        var variableValue = Expression.Operand as ValueVar;
                        if ( variableValue != null )
                        {
                            var variable = GetVariable(variableValue.Identifier) as VariableType<string>;
                            if ( variable != null )
                            {
                                AddNewVariable(Identifier, variable.Value);
                                return;
                            }
                            throw new AbstractSyntaxTreeException(String.Format("Variable '{0}' was not the type of {1}", variableValue.Identifier, Types.String));
                        }
                        throw new AbstractSyntaxTreeException(String.Format("The value to set for {0} was not of the type {1}.", Identifier, Types.String));
                    }
            }
        }

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
