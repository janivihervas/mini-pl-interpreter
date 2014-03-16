using System;

namespace MiniPL.Exceptions
{
    /// @author Jani Viherväs
    /// @version 6.3.2014
    ///
    /// <summary>
    /// Class for scanner exceptions
    /// </summary>
    public class ScannerException : Exception
    {
        /// <summary>
        /// Creates a new scanner exception
        /// </summary>
        /// <param name="message">Message to be passed on with the exception</param>
        public ScannerException(string message) : base(message) {}
    }
}
