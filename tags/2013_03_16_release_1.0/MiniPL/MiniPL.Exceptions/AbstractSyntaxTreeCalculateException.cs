using System;

namespace MiniPL.Exceptions
{
    /// @author Jani Viherväs
    /// @version 8.3.2014
    ///
    /// <summary>
    /// Exception for calculating expression values
    /// </summary>
    public class AbstractSyntaxTreeCalculateException : Exception
    {
        /// <summary>
        /// Creates a new exception for calculating expression values
        /// </summary>
        /// <param name="message">Error message</param>
        public AbstractSyntaxTreeCalculateException(string message) : base(message) { }
    }
}
