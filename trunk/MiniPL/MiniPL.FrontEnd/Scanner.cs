using System;
using System.Collections.Generic;
using System.Linq;

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
                        continue;
                    }
                    if ( (token = CreateReservedKeywordToken(line)) != null )
                    {
                        tokens.Add(token);
                        continue;
                    }

                    if ( (token = CreateOperatorToken(line)) != null )
                    {
                        tokens.Add(token);
                        continue;
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
        /// Creates a token if it matches the given symbol
        /// </summary>
        /// <param name="line">Current line</param>
        /// <param name="symbol">Symbol to match</param>
        /// <returns>Created token or null</returns>
        private Token CreateToken(string line, string symbol)
        {
            // One can make this to work a lot faster, but this is nicer
            try
            {
                if ( line.Substring(_column, symbol.Length) == symbol )
                {
                    var token = new Token(_row, _column, symbol);
                    _column += symbol.Length;
                    return token;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            return null;
        }


        /// <summary>
        /// Creates a token if it matches the given symbols
        /// </summary>
        /// <param name="line">Current line</param>
        /// <param name="symbols">Symbols to match</param>
        /// <returns>Created token or null</returns>
        private Token CreateToken(string line, IEnumerable<string> symbols)
        {
            return symbols.Select(symbol => CreateToken(line, symbol)).FirstOrDefault(token => token != null);
        }


        /// <summary>
        /// Creates a new token for reserved keywords
        /// </summary>
        /// <param name="line">Current line</param>
        /// <returns>New token for reserved keywords or null</returns>
        private Token CreateReservedKeywordToken(string line)
        {
            return CreateToken(line, new[] { ReservedKeyword.Assignment, 
                                                  ReservedKeyword.Colon, 
                                                  ReservedKeyword.Semicolon, 
                                                  ReservedKeyword.Range, 
                                                  ReservedKeyword.Do, 
                                                  ReservedKeyword.In, 
                                                  ReservedKeyword.End, 
                                                  ReservedKeyword.For, 
                                                  ReservedKeyword.Var, 
                                                  ReservedKeyword.Read, 
                                                  ReservedKeyword.Print, 
                                                  ReservedKeyword.Assert,  });
        }

        
        /// <summary>
        /// Creates a new token for variable types
        /// </summary>
        /// <param name="line">Current line</param>
        /// <returns>New token for variable types or null, if it wasn't a type token</returns>
        private Token CreateTypeToken(string line)
        {
            return CreateToken(line, new[] {Type.Int, Type.Bool, Type.String});
        }

        
        /// <summary>
        /// Creates a new token for operators
        /// </summary>
        /// <param name="line">Current line</param>
        /// <returns>New token for operators or null, if it wasn't an operator token</returns>
        private Token CreateOperatorToken(string line)
        {
            return CreateToken(line, new[] { Operator.Plus, Operator.Minus, Operator.Multiply, Operator.Divide,
                                                  Operator.ParenthesisLeft, Operator.ParenthesisRight,
                                                  Operator.And, Operator.Not,
                                                  Operator.GreaterOrEqualThan, Operator.LesserOrEqualThan, 
                                                  Operator.GreaterThan, Operator.LesserThan, Operator.Equal });
        }

    }
}
