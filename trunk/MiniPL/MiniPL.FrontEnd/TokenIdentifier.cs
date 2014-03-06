namespace MiniPL.FrontEnd
{
    /// @author Jani Viherväs
    /// @version 5.3.2014
    ///
    /// <summary>
    /// Identifier token class.
    /// </summary>
    public class TokenIdentifier : Token
    {
        /// <summary>
        /// Gets the identifier
        /// </summary>
        public string Identifier { get; private set; }

        // TODO: maybe add property to get the value binded to the identifier?

        /// <summary>
        /// Creates a new integer token. ATTENTION! This constructor handles the 0th column and row, DON'T add one to neither one.
        /// </summary>
        /// <param name="line">Current line of the source code.</param>
        /// <param name="startColumn">Starting column of the lexeme.</param>
        /// <param name="identifier">Identifier</param>
        public TokenIdentifier(int line, int startColumn, string identifier) : base(line, startColumn, identifier)
        {
            // TODO: check for reserved keywords
            Identifier = identifier;
        }

        
    }
}
