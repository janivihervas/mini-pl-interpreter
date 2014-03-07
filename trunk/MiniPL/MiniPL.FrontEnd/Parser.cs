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
            _i = 0;
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
            if (CheckToken(token, ReservedKeywords.End))
            {
                _i--;
                return;
            }
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
                        StatementAlt();
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
                _i--;
                Identifier();
                token = NextToken();
                if (!CheckToken(token, ReservedKeywords.Assignment))
                {
                    ThrowParserException(token);
                }
                Expression();
                return;
            }
            //ThrowParserException(token);
        }


        /// <summary>
        /// Handles the "stmt'" production
        /// </summary>
        private void StatementAlt()
        {
            var token = NextToken();
            if (CheckToken(token, ReservedKeywords.Semicolon)) // epsilon
            {
                _i--;
                return;
            }
            if (!CheckToken(token, ReservedKeywords.Assignment))
            {
                ThrowParserException(token);
            }
            Expression();
        }


        /// <summary>
        /// Handles the "expr" production
        /// </summary>
        private void Expression()
        {
            Operand();
            ExpressionAlt();
        }


        /// <summary>
        /// Handles the "expr'" production
        /// </summary>
        private void ExpressionAlt()
        {
            var token = NextToken();
            if (CheckToken(token, ReservedKeywords.Semicolon) ||
                CheckToken(token, ReservedKeywords.Range) ||
                CheckToken(token, ReservedKeywords.Do) ||
                CheckToken(token, Operators.ParenthesisRight) )
            {
                _i--;
                return;
            }
            _i--;
            Operator();
            Operand();
        }
        
        
        /// <summary>
        /// Handles the "opnd" production
        /// </summary>
        private void Operand()
        {
            var token = NextToken();
            if (token == null)
            {
                ThrowParserException(null);
            }
            if (token is TokenTerminal<int>)
            {
                return;
            }
            if (token is TokenTerminal<string>)
            {
                return;
            }
            if (token is TokenIdentifier)
            {
                _i--;
                Identifier();
                return;
            }
            if (CheckToken(token, Operators.ParenthesisLeft))
            {
                Expression();
                token = NextToken();
                if (!CheckToken(token, Operators.ParenthesisRight))
                {
                    ThrowParserException(token);
                }
                return;
            }
            _i--;
            OperandAlt();
            token = NextToken();
            if (!(token is TokenTerminal<bool>))
            {
                ThrowParserException(token);
            }

        }


        /// <summary>
        /// Handles the "opnd'" production
        /// </summary>
        private void OperandAlt()
        {
            var token = NextToken();
            if ( token is TokenTerminal<bool> ) // epsilon
            {
                _i--;
                return;
            }
            _i--;
            UnaryOperator();
        }


        /// <summary>
        /// Handles the "type" production
        /// </summary>
        private void Type()
        {
            var token = NextToken();
            if (token == null)
            {
                ThrowParserException(null);
            }
            switch (token.Lexeme)
            {
                case Types.Int:
                    {
                        return;
                    }
                case Types.String:
                    {
                        return;
                    }
                case Types.Bool:
                    {
                        return;
                    }
            }
            ThrowParserException(token);
        }


        /// <summary>
        /// Handles the "op" production
        /// </summary>
        private void Operator()
        {
            var token = NextToken();
            if (token == null)
            {
                ThrowParserException(null);
            }
            switch (token.Lexeme)
            {
                case Operators.Plus:
                    {
                        return;
                    }
                case Operators.Minus:
                    {
                        return;
                    }
                case Operators.Multiply:
                    {
                        return;
                    }
                case Operators.Divide:
                    {
                        return;
                    }
                case Operators.LesserThan:
                    {
                        return;
                    }
                case Operators.GreaterThan:
                    {
                        return;
                    }
                case Operators.LesserOrEqualThan:
                    {
                        return;
                    }
                case Operators.GreaterOrEqualThan:
                    {
                        return;
                    }
                case Operators.Equal:
                    {
                        return;
                    }
                case Operators.NotEqual:
                    {
                        return;
                    }
                case Operators.And:
                    {
                        return;
                    }
            }
            ThrowParserException(token);

        }


        /// <summary>
        /// Handles the "unary" production
        /// </summary>
        private void UnaryOperator()
        {
            var token = NextToken();
            if (!CheckToken(token, Operators.Not))
            {
                ThrowParserException(token);
            }
        }


        /// <summary>
        /// Handles the "ident" production. Looks the identifier from the symbol table
        /// </summary>
        private void Identifier()
        {
            var token = NextToken() as TokenIdentifier;
            if (token == null || !SymbolTable.FindSymbol(token.Identifier))
            {
                ThrowParserException(token);
            }
        }


        /// <summary>
        /// Handles the "ident'" production. Adds the identifier to the symbol table
        /// </summary>
        private void IdentifierAlt()
        {
            var token = NextToken() as TokenIdentifier;
            if ( token == null || !SymbolTable.AddSymbol(token.Identifier) )
            {
                ThrowParserException(token);
            }
        }
    }
}
