using MiniPL.Exceptions;

namespace MiniPL.FrontEnd
{
    /// @author Jani Viherväs
    /// @version 28.2.2014
    ///
    /// <summary>
    /// Integer token class.
    /// </summary>
    public class TokenTerminal<T> : Token
    {
        /// <summary>
        /// Gets the integer value
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Creates a new integer token. ATTENTION! This constructor handles the 0th column and row, DON'T add one to neither one.
        /// </summary>
        /// <param name="line">Current line of the source code.</param>
        /// <param name="startColumn">Starting column of the lexeme.</param>
        /// <param name="value">Integer value.</param>
        public TokenTerminal(int line, int startColumn, T value)
            : base(line, startColumn, value.ToString().ToLower())
        {
            if (typeof(T) != typeof(int) &&
                typeof(T) != typeof(bool) &&
                typeof(T) != typeof(string) )
            {
                throw new TokenException("Value must be int, bool, or string");
            }
            Value = value;
        }
    }
}
