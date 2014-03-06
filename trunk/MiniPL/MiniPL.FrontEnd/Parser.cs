using System;
using System.Collections.Generic;
using MiniPL.Exceptions;

namespace MiniPL.FrontEnd
{
    /// @author Jani Viherväs
    /// @version 27.2.2014
    ///
    /// <summary>
    /// Class to handle the parsing of the source code. Creates the abstract syntax tree.
    /// </summary>
    public class Parser
    {
        private int _i;
        private List<Token> _tokens;

        /// <summary>
        /// Parses the token list
        /// </summary>
        /// <param name="tokens">Tokens scanned</param>
        public void Parse(List<Token> tokens) // TODO: construct the abstract syntax tree
        {
            if (tokens == null)
            {
                throw new ParserException("Token list was null");
            }
            _tokens = tokens;
            Statements();
        }


        /// <summary>
        /// Gets the next token or null if there are no more tokens.
        /// </summary>
        /// <returns>The next token or null if there are no more tokens</returns>
        private Token NextToken()
        {
            return _i < _tokens.Count
                       ? _tokens[_i++]
                       : null;
        }


        /// <summary>
        /// Checks the token for null reference and correct lexeme
        /// </summary>
        /// <param name="token">Current token</param>
        /// <param name="lexeme">Lexeme expected</param>
        /// <returns>True if ok</returns>
        public static bool CheckToken(Token token, string lexeme)
        {
            return token != null && token.Lexeme == lexeme;
        }

        /// <summary>
        /// Temporary method to throw exceptions
        /// </summary>
        /// <param name="token">Current token</param>
        private void ThrowParserException(Token token)
        {
            if (token == null)
            {
                throw new ParserException(String.Format("Unexpected ending of program, last token parsed was on line {0} starting at column {1}.", _tokens[_i - 1].Line, _tokens[_i - 1].StartColumn ));
            }
            throw new ParserException(String.Format("Syntax error on line {0} starting at column {1}.", token.Line, token.StartColumn));
        }

        
        /// <summary>
        /// Handles the "stmts" production
        /// </summary>
        private void Statements()
        {
            Statement();
            var token = NextToken();
            if ( CheckToken(token, ReservedKeywords.Semicolon) )
            {
                StatementsAlt();
            }
            else
            {
                // TODO: error handling
                ThrowParserException(token);
            }
        }


        /// <summary>
        /// Handles the "stmts'" production
        /// </summary>
        private void StatementsAlt()
        {
            var token = NextToken();
            if (token != null)
            {
                _i--; // there were more tokens
                Statements();
            }
            // else end of program to interpret
        }


        /// <summary>
        /// Handles the "stmt" production
        /// </summary>
        private void Statement()
        {
            var token = NextToken();
            switch (token.Lexeme)
            {
                case ReservedKeywords.Var:
                    {
                        IdentifierAlt();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.Colon))
                        {
                            ThrowParserException(token);
                        }
                        Type();
                        Statement_();
                        return;
                    }
                case ReservedKeywords.For:
                    {
                        Identifier();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.In))
                        {
                            ThrowParserException(token);
                        }
                        Expression();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.Range))
                        {
                            ThrowParserException(token);
                        }
                        Expression();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.Do))
                        {
                            ThrowParserException(token);
                        }
                        Statements();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.End))
                        {
                            ThrowParserException(token);
                        }
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.For))
                        {
                            ThrowParserException(token);
                        }
                        return;
                    }

                case ReservedKeywords.Read:
                    {
                        Identifier();
                        return;
                    }
                case ReservedKeywords.Print:
                    {
                        Expression();
                        return;
                    }
                case ReservedKeywords.Assert:
                    {
                        token = NextToken();
                        if (!CheckToken(token, Operators.ParenthesisLeft))
                        {
                            ThrowParserException(token);
                        }
                        Expression();
                        token = NextToken();
                        if ( !CheckToken(token, Operators.ParenthesisRight) )
                        {
                            ThrowParserException(token);
                        }
                        return;
                    }
            }
            if ( token is TokenIdentifier )
            {

            }

        }


        /// <summary>
        /// Handles the "stmt'" production
        /// </summary>
        private void Statement_()
        {
            
        }


        /// <summary>
        /// Handles the "expr" production
        /// </summary>
        private void Expression()
        {
            
        }


        /// <summary>
        /// Handles the "expr'" production
        /// </summary>
        private void ExpressionAlt()
        {
            
        }


        /// <summary>
        /// Handles the "opnd" production
        /// </summary>
        private void Operand()
        {
            
        }


        /// <summary>
        /// Handles the "type" production
        /// </summary>
        private void Type()
        {
            
        }


        /// <summary>
        /// Handles the "op" production
        /// </summary>
        private void Operator()
        {
            
        }


        /// <summary>
        /// Handles the "unary" production
        /// </summary>
        private void UnaryOperator()
        {
            
        }


        /// <summary>
        /// Handles the "ident" production
        /// </summary>
        private void Identifier()
        {
            
        }


        /// <summary>
        /// Handles the "ident'" production
        /// </summary>
        private void IdentifierAlt()
        {
            
        }
    }
}
