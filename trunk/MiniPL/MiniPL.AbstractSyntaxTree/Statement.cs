using System.Collections.Generic;

namespace MiniPL.AbstractSyntaxTree
{
    /// <summary>
    /// Base class for statements
    /// </summary>
    public class Statement
    {
        private static readonly List<Variable> Variables = new List<Variable>();

        protected static void AddNewVariable(string identifier)
        {
            if (Variables.Exists(var => var.Identifier == identifier))
            {
                return;
            }
            Variables.Add(new Variable(identifier));
        }

        protected static void AddNewVariable(Variable variable)
        {
            if ( variable == null || Variables.Exists(var => var.Identifier == variable.Identifier) )
            {
                return;
            }
            Variables.Add(variable);
        }

        protected static void AddNewVariable(string identifier, int value)
        {
            if ( Variables.Exists(var => var.Identifier == identifier) )
            {
                return;
            }
            Variables.Add(new VariableType<int>(identifier, value));
        }

        protected static void AddNewVariable(string identifier, bool value)
        {
            if ( Variables.Exists(var => var.Identifier == identifier) )
            {
                return;
            }
            Variables.Add(new VariableType<bool>(identifier, value));
        }

        protected static void AddNewVariable(string identifier, string value)
        {
            if ( Variables.Exists(var => var.Identifier == identifier) )
            {
                return;
            }
            Variables.Add(new VariableType<string>(identifier, value));
        }

        public static Variable GetVariable(string identifier)
        {
            return Variables.Find(var => var.Identifier == identifier);
        }

        public static void DeleteAllVariables()
        {
            Variables.Clear();
        }

        /// <summary>
        /// Executes the statement. Must be overridden.
        /// </summary>
        public virtual void Execute()
        {
            throw new System.NotImplementedException("Execute() method was not overrided");
        }
    }
}
