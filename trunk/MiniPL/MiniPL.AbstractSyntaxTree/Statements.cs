using System.Collections.Generic;

namespace MiniPL.AbstractSyntaxTree
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Class to hold multiple statements
    /// </summary>
    public class Statements : Statement
    {
        /// <summary>
        /// List of all statements
        /// </summary>
        public List<Statement> StatementList { get; private set; }


        /// <summary>
        /// Creates an object to hold multiple statements
        /// </summary>
        public Statements()
        {
            StatementList = new List<Statement>();
        }


        /// <summary>
        /// Adds a statement to the statements list
        /// </summary>
        /// <param name="statement">Statement to add</param>
        public void AddStatement(Statement statement)
        {
            if (statement == null)
            {
                return;
            }
            StatementList.Add(statement);
        }


        /// <summary>
        /// Executes every statement
        /// </summary>
        public override void Execute()
        {
            DeleteAllVariables();
            foreach (var statement in StatementList)
            {
                statement.Execute();
            }
        }
    }
}
