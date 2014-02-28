using System;
using System.Collections.Generic;

namespace MiniPL.FrontEnd
{
    /// @author Jani Viherväs
    /// @version 27.2.2014
    ///
    /// <summary>
    /// Class to handle the scanning of the source code. Produces tokens for the parser.
    /// </summary>
    public class Scanner
    {
        private int _column;
        private int _row;

        /// <summary>
        /// Produces tokens for the parser.
        /// </summary>
        /// <param name="lines">Source code</param>
        /// <returns>Tokens</returns>
        public List<Token> Tokenize(List<string> lines)
        {
            var tokens = new List<Token>();
            for (_row = 0; _row < lines.Count; _row++)
            {
                _column = 0;
                var line = lines[_row];
                while (_column < line.Length)
                {
                    SkipWhiteSpace(line);
                    Token token;

                    if ( (token = CreateTypeToken(line)) != null )
                    {
                        tokens.Add(token);
                        break;
                    }
                    _column++; // TODO: Remove this when fully implemented
                }
            }

            return tokens;
        }

        /// <summary>
        /// Skips whitespaces in the current line
        /// </summary>
        /// <param name="line"></param>
        private void SkipWhiteSpace(string line)
        {
            while (Char.IsWhiteSpace(line[_column]))
            {
                _column++;
            }
        }


        /// <summary>
        /// Creates a new token for variable types
        /// </summary>
        /// <param name="line">Current line</param>
        /// <returns>New token for variable types or null, if it wasn't a type token</returns>
        private Token CreateTypeToken(string line)
        {
            try
            {
                if (line.Substring(_column, Type.Int.Length) == Type.Int)
                {
                    var token = new Token(_row, _column, Type.Int);
                    _column += Type.Int.Length;
                    return token;
                }
                if (line.Substring(_column, Type.Bool.Length) == Type.Bool)
                {
                    var token = new Token(_row, _column, Type.Bool);
                    _column += Type.Bool.Length;
                    return token;
                }
                if (line.Substring(_column, Type.String.Length) == Type.String)
                {
                    var token = new Token(_row, _column, Type.String);
                    _column += Type.String.Length;
                    return token;
                }
            } catch(ArgumentOutOfRangeException) // _column to end of line was less then Type.Int.Length
            {
                return null;
            }
            return null;
        }
    }
}
