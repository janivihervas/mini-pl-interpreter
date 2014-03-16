using System;

namespace MiniPL.Exceptions
{
    /// @author Jani Viherväs
    /// @version 28.2.2014
    ///
    /// <summary>
    /// Class for token exceptions
    /// </summary>
    public class TokenException : Exception
    {
        /// <summary>
        /// Creates a new token exception
        /// </summary>
        /// <param name="message">Message to be passed on with the exception</param>
        public TokenException(string message) : base(message) {}
    }
}
