using System;
using System.Collections.Generic;
using MiniPL.Exceptions;
using MiniPL.Tokens;

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
        /// Semantic errors
        /// </summary>
        private static readonly List<Error> SemanticErrors = new List<Error>();

        /// <summary>
        /// Source code lines
        /// </summary>
        public static List<string> Lines;

        /// <summary>
        /// Creates an object to hold multiple statements
        /// </summary>
        public Statements(List<string> sourceCodeLines)
        {
            Lines = sourceCodeLines;
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
            foreach (var statement in StatementList)
            {
                try
                {
                    statement.Execute();
                }
                catch (AbstractSyntaxTreeCalculateException)
                {
                }
                catch(AbstractSyntaxTreeException)
                {
                }
                catch(AssertFailedException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    //SemanticErrors.Add(new Error("Unknown error: " + e.Message));
                }
            }
            if (0 < SemanticErrors.Count)
            {
                throw new AbstractSyntaxTreeException(SemanticErrors);
            }
        }


        /// <summary>
        /// Clears all errors
        /// </summary>
        public static void ClearErrors()
        {
            SemanticErrors.Clear();
        }


        /// <summary>
        /// Adds a new semantic error
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="message">Message</param>
        public static void AddSemanticError(Token token, string message)
        {
            if ( token != null )
            {
                SemanticErrors.Add(
                    new Error(Lines[token.Line - 1], token.Line, token.StartColumn, message));
            }
            else
            {
                SemanticErrors.Add(
                    new Error(Lines[Lines.Count - 1], Lines.Count, Lines[Lines.Count - 1].Length, message));
            }
        }


        /// <summary>
        /// Deletes last added error. Used when trying to evaluate different types of values
        /// </summary>
        public static void DeleteLastAddedError()
        {
            if (SemanticErrors.Count < 1)
            {
                return;
            }
            var error = SemanticErrors[SemanticErrors.Count - 1];
            SemanticErrors.Remove(error);
        }

    }
}
