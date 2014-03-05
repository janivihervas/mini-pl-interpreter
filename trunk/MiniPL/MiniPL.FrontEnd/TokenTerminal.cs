using MiniPL.Exceptions;

namespace MiniPL.FrontEnd
{
    /// @author Jani Viherväs
    /// @version 28.2.2014
    ///
    /// <summary>
    /// Terminal token class.
    /// </summary>
    public class TokenTerminal<T> : Token
    {
        /// <summary>
        /// Gets the token value
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Creates a new terminal token. ATTENTION! This constructor handles the 0th column and row, DON'T add one to neither one.
        /// </summary>
        /// <param name="line">Current line of the source code.</param>
        /// <param name="startColumn">Starting column of the lexeme.</param>
        /// <param name="value">Value, must be int, string or bool</param>
        public TokenTerminal(int line, int startColumn, T value) : base(line, startColumn)
        {
            if (typeof(T) != typeof(int) &&
                typeof(T) != typeof(bool) &&
                typeof(T) != typeof(string) )
            {
                throw new TokenException("Value must be int, bool, or string");
            }
            Value = value;
            Lexeme = typeof(T) == typeof(bool) ? value.ToString().ToLower() : value.ToString(); // boolean value's string representation is "True" or "False"
                                                                                                // and we don't want to mess string values
        }
    }
}
