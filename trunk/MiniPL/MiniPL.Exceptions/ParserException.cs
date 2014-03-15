using System;
using System.Collections.Generic;

namespace MiniPL.Exceptions
{
    /// @author Jani Viherväs
    /// @version 6.3.2014
    ///
    /// <summary>
    /// Class for parser exceptions
    /// </summary>
    public class ParserException : Exception
    {
        /// <summary>
        /// Gets the syntax errors
        /// </summary>
        public List<SyntaxError> Errors { get; private set; }

        /// <summary>
        /// Error message
        /// </summary>
        private static string _message = "There were syntax errors.";

        /// <summary>
        /// Creates a new parser exception
        /// </summary>
        /// <param name="message">Message to be passed on with the exception</param>
        public ParserException(string message)
        {
            _message = message;
            Errors = new List<SyntaxError>();
        }


        /// <summary>
        /// Creates a new parser exception
        /// </summary>
        /// <param name="errors">Syntax errors</param>
        public ParserException(List<SyntaxError> errors)
            : base(_message)
        {
            Errors = errors ?? new List<SyntaxError>();
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
