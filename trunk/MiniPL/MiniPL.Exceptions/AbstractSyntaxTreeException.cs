using System;

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
        /// Creates a new abstract syntax tree exception
        /// </summary>
        /// <param name="message">Message to be passed on with the exception</param>
        public AbstractSyntaxTreeException(string message) : base(message) { }
    }
}
