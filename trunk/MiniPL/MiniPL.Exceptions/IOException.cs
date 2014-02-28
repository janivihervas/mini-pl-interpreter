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
        public IOException(string message)
            : base(message)
        {
            
        }
    }
}
