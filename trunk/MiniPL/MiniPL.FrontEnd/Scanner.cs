using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

                    if ( (token = CreateTerminalToken(line)) != null )
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


        /// <summary>
        /// Creates a new token for terminals
        /// </summary>
        /// <param name="line">Current line</param>
        /// <returns>New token for terminal or null</returns>
        private Token CreateTerminalToken(string line)
        {
            if ( line[_column] == '"' ) // string
            {
                var end = line.Substring(_column);
                var value = ScanString(end); // TODO: Remember that this gets the contents without the "-characters
                var token = new TokenTerminal<string>(_row, _column, value);
                //_column += value.Length;
                SkipString(line);
                return token;
            }
            if ( Char.IsNumber(line[_column]) ) // int
            {
                var lenght = 1;
                while (_column + lenght < line.Length && Char.IsNumber(line[_column + lenght]))
                {
                    lenght++;
                }
                var subString = line.Substring(_column, lenght);
                var value = int.Parse(subString);
                var token = new TokenTerminal<int>(_row, _column, value);
                _column += lenght;
                return token;
            }
            if (line[_column] == 't' || line[_column] == 'f') // boolean
            {
                var lenght = 1;
                while ( Char.IsLetter(line[_column + lenght]) )
                {
                    lenght++;
                }
                var subString = line.Substring(_column, lenght);
                var value = Boolean.Parse(subString);
                var token = new TokenTerminal<bool>(_row, _column, value);
                _column += subString.Length;
                return token;
            }
            return null;
        }

        
        /// <summary>
        /// Moves the index so that it skips the string
        /// </summary>
        private void SkipString(string line)
        {
            if (line[_column] != '"')
            {
                return;
            }
            _column++;
            for (var i = _column; i < line.Length; i++)
            {
                if (line[i] == '"')
                {
                    _column++;
                    break;
                }
                if (line[i] == '\\')
                {
                    _column++;
                    i++;
                }
                _column++;
            }
        }


        /// <summary>
        /// Scans the string literal
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>"te\"st" => te"st</returns>
        public static string ScanString(string input)
        {
            var s = input.ToCharArray();
            if ( s.Length < 2 )
            {
                return "";
            }
            if ( s[0] != '"' )
            {
                return "";
            }

            var result = new StringBuilder();

            for ( var i = 1; i < s.Length; i++ )
            {
                if ( s[i] == '"' ) // End of the string, i.e. string s = "test";
                {
                    break;
                }

                if ( i == s.Length - 1 && s[i] != '"' ) // We are at the last character 
                {                                       // and have not stumbled upon closing quotes
                    return "";                          // f.g. string s = "test
                }

                if ( s[i] == '\\' && i < s.Length - 1 ) // Escape character, i.e. string s = "te\"st"  ==> te"st
                {
                    if (s[i+1] != 'n' ||
                        s[i+1] != 'r' ||
                        s[i+1] != 't')
                    i++;
                }

                result.Append(s[i]);
            }
            return result.ToString();
        }

    }
}
