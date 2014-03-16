using System;
using System.Collections.Generic;

namespace MiniPL.Exceptions
{
    /// @author Jani Viherväs
    /// @version 6.3.2014
    ///
    /// <summary>
    /// Class for abstract syntax tree exceptions
    /// </summary>
    public class AbstractSyntaxTreeException : Exception
    {
        /// <summary>
        /// Gets the syntax errors
        /// </summary>
        public List<Error> Errors { get; private set; }

        /// <summary>
        /// Error message
        /// </summary>
        private static string _message = "There were semantic errors.";

        public AbstractSyntaxTreeException()
        {}

        /// <summary>
        /// Creates a new abstract syntax tree exception
        /// </summary>
        /// <param name="message">Message to be passed on with the exception</param>
        public AbstractSyntaxTreeException(string message) : base(message)
        {
            _message = message;
        }

               /// <summary>
        /// Creates a new parser exception
        /// </summary>
        /// <param name="errors">Syntax errors</param>
        public AbstractSyntaxTreeException(List<Error> errors)
            : base(_message)
        {
            Errors = errors ?? new List<Error>();
        }


        /// <summary>
        /// Gets the error message
        /// </summary>
        public override string Message
        {
            get { return _message + "\n" + String.Join("\n", Errors); }
        }
    }
}
