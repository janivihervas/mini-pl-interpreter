using System;

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
        /// Creates a new parser exception
        /// </summary>
        /// <param name="message">Message to be passed on with the exception</param>
        public ParserException(string message) : base(message) {}
    }
}
