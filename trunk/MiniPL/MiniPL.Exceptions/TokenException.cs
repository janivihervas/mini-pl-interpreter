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
        public TokenException(string message) : base(message)
        { }
    }
}
