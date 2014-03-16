using System;

namespace MiniPL.Exceptions
{
    /// @author Jani Viherväs
    /// @version 28.2.2014
    ///
    /// <summary>
    /// Class for IO exceptions
    /// </summary>
    public class IOException : Exception
    {
        /// <summary>
        /// Creates a new IO exception
        /// </summary>
        /// <param name="message">Message to be passed on with the exception</param>
        public IOException(string message) : base(message) {}
    }
}
