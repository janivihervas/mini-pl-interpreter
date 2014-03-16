using System.Collections.Generic;

namespace MiniPL.AbstractSyntaxTree
{
    /// <summary>
    /// Base class for statements
    /// </summary>
    public class Statement
    {
        /// <summary>
        /// Variables
        /// </summary>
        private static readonly List<Variable> Variables = new List<Variable>();


        /// <summary>
        /// Adds a new variable with default value
        /// </summary>
        /// <param name="identifier">Identifier</param>
        protected static void AddNewVariable(string identifier)
        {
            if (Variables.Exists(var => var.Identifier == identifier))
            {
                return;
            }
            Variables.Add(new Variable(identifier));
        }


        /// <summary>
        /// Adds a new variable
        /// </summary>
        /// <param name="variable">Variable to add</param>
        protected static void AddNewVariable(Variable variable)
        {
            if ( variable == null || Variables.Exists(var => var.Identifier == variable.Identifier) )
            {
                return;
            }
            Variables.Add(variable);
        }


        /// <summary>
        /// Adds a new int variable
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="value">Value</param>
        protected static void AddNewVariable(string identifier, int value)
        {
            if ( Variables.Exists(var => var.Identifier == identifier) )
            {
                return;
            }
            Variables.Add(new VariableType<int>(identifier, value));
        }


        /// <summary>
        /// Adds a new bool variable
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="value">Value</param>
        protected static void AddNewVariable(string identifier, bool value)
        {
            if ( Variables.Exists(var => var.Identifier == identifier) )
            {
                return;
            }
            Variables.Add(new VariableType<bool>(identifier, value));
        }


        /// <summary>
        /// Adds a new string variable
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="value">Value</param>
        protected static void AddNewVariable(string identifier, string value)
        {
            if ( Variables.Exists(var => var.Identifier == identifier) )
            {
                return;
            }
            Variables.Add(new VariableType<string>(identifier, value));
        }


        /// <summary>
        /// Gets the variable with the identifier
        /// </summary>
        /// <param name="identifier">Variable's identifier</param>
        /// <returns>Found variable or null</returns>
        public static Variable GetVariable(string identifier)
        {
            return Variables.Find(var => var.Identifier == identifier);
        }


        /// <summary>
        /// Delete's all variables
        /// </summary>
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
