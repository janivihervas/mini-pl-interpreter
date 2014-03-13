using System;
using System.Collections.Generic;
using MiniPL.AbstractSyntaxTree;
using MiniPL.Exceptions;
using MiniPL.Tokens;

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
        public Statements Parse(List<Token> tokens) 
        {
            if (tokens == null)
            {
                throw new ParserException("Token list was null");
            }
            SymbolTable.DeleteAllSymbols();
            _tokens = tokens;
            _i = 0;
            var rootNode = Statements();
            return rootNode;
        }


        /// <summary>
        /// Gets the current token or null if there are no more tokens. 
        /// Adds the counter so the next caller gets the next token.
        /// </summary>
        /// <returns>The current token or null if there are no more tokens</returns>
        private Token NextToken()
        {
            return _i < _tokens.Count
                       ? _tokens[_i++]
                       : null;
        }


        /// <summary>
        /// Gets the current token or null if there are no more tokens. 
        /// Doesn't add the counter so the next caller gets the same token.
        /// </summary>
        /// <returns>The current token or null if there are no more tokens</returns>
        private Token CurrentToken()
        {
            return _i < _tokens.Count
                       ? _tokens[_i]
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
        private Statements Statements()
        {
            var node = new Statements();
            while (_i < _tokens.Count)
            {
                node.AddStatement(Statement());
                var token = NextToken();
                if ( !CheckToken(token, ReservedKeywords.Semicolon) )
                {
                    ThrowParserException(token);
                    return null;
                }
                token = CurrentToken();
                if (CheckToken(token, ReservedKeywords.End)) // end of for loop
                {
                    return node;
                }
            }
            return node;
        }


        /// <summary>
        /// Handles the "stmt" production
        /// </summary>
        private Statement Statement()
        {
            var token = NextToken();
            switch (token.Lexeme)
            {
                case ReservedKeywords.Var:
                    {
                        var identifier = IdentifierAlt();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.Colon))
                        {
                            ThrowParserException(token);
                        }
                        var type = Type();
                        var expression = StatementAlt();
                        return new StatementVarInitialize(identifier, type, expression);
                    }
                case ReservedKeywords.For:
                    {
                        var identifier = Identifier();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.In))
                        {
                            ThrowParserException(token);
                        }
                        var firstExpr = Expression();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.Range))
                        {
                            ThrowParserException(token);
                        }
                        var secondExpr = Expression();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.Do))
                        {
                            ThrowParserException(token);
                        }
                        var stmts = Statements();
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
                        return new StatementFor(identifier, firstExpr, secondExpr, stmts);
                    }

                case ReservedKeywords.Read:
                    {
                        var id = Identifier();
                        return new StatementRead(id);
                    }
                case ReservedKeywords.Print:
                    {
                        var expr = Expression();
                        return new StatementPrint(expr);
                    }
                case ReservedKeywords.Assert:
                    {
                        token = NextToken();
                        if (!CheckToken(token, Operators.ParenthesisLeft))
                        {
                            ThrowParserException(token);
                        }
                        var expr = Expression();
                        token = NextToken();
                        if ( !CheckToken(token, Operators.ParenthesisRight) )
                        {
                            ThrowParserException(token);
                        }
                        return new StatementAssert(expr);
                    }
            }
            if ( token is TokenIdentifier )
            {
                _i--;
                var id = Identifier();
                token = NextToken();
                if (!CheckToken(token, ReservedKeywords.Assignment))
                {
                    ThrowParserException(token);
                }
                var expr = Expression();
                return new StatementVarAssignment(id, expr);
            }
            //ThrowParserException(token);
            return null;
        }


        /// <summary>
        /// Handles the "stmt'" production
        /// </summary>
        private Expression StatementAlt()
        {
            var token = NextToken();
            if (CheckToken(token, ReservedKeywords.Semicolon)) // epsilon
            {
                _i--;
                return null;
            }
            if (!CheckToken(token, ReservedKeywords.Assignment))
            {
                ThrowParserException(token);
                return null;
            }
            return Expression();
        }


        /// <summary>
        /// Handles the "expr" production
        /// </summary>
        private Expression Expression()
        {
            var expr = new Expression();
            Operand(expr, true);
            ExpressionAlt(expr);
            return expr;
        }


        /// <summary>
        /// Handles the "expr'" production
        /// </summary>
        private void ExpressionAlt(Expression expression)
        {
            var token = CurrentToken();
            if (CheckToken(token, ReservedKeywords.Semicolon) ||
                CheckToken(token, ReservedKeywords.Range) ||
                CheckToken(token, ReservedKeywords.Do) ||
                CheckToken(token, Operators.ParenthesisRight) )
            {
                return;
            }
            Operator(expression);
            Operand(expression, false);
        }
        
        
        /// <summary>
        /// Handles the "opnd" production
        /// </summary>
        private void Operand(Expression expression, bool addToFirst)
        {
            var token = NextToken();
            if (token == null)
            {
                ThrowParserException(null);
            }
            var tokenInt = token as TokenTerminal<int>;
            if ( tokenInt != null)
            {
                expression.AddOperand(tokenInt, addToFirst);
                return;
            }
            var tokenString = token as TokenTerminal<string>;
            if (tokenString != null)
            {
                expression.AddOperand(tokenString, addToFirst);
                return;
            }
            if (token is TokenIdentifier)
            {
                _i--;
                Identifier();
                expression.AddOperand(token as TokenIdentifier, addToFirst);
                return;
            }
            if ( CheckToken(token, Operators.ParenthesisLeft) )
            {
                var expr = Expression();
                token = NextToken();
                if ( !CheckToken(token, Operators.ParenthesisRight) )
                {
                    ThrowParserException(token);
                }
                expression.AddExpression(expr, addToFirst);
                return; 
            }
            _i--;
            var unary = OperandAlt();
            var tokenBool = NextToken() as TokenTerminal<bool>;
            if (tokenBool == null)
            {
                ThrowParserException(null);
                return;
            }
            tokenBool.Value = unary && tokenBool.Value;
            expression.AddOperand(tokenBool, addToFirst);
        }


        /// <summary>
        /// Handles the "opnd'" production
        /// </summary>
        /// <returns>Boolean value of the ! operator, i.e. if there is ! operator, returns false and true otherwise</returns>
        private bool OperandAlt()
        {
            var token = CurrentToken();
            if ( token is TokenTerminal<bool> ) // epsilon
            {
                return true;
            }
            return UnaryOperator();
        }


        /// <summary>
        /// Handles the "type" production
        /// </summary>
        private string Type()
        {
            var token = NextToken();
            if (token == null)
            {
                ThrowParserException(null);
                return null;
            }
            switch (token.Lexeme)
            {
                case Types.Int:
                    {
                        return Types.Int;
                    }
                case Types.String:
                    {
                        return Types.String;
                    }
                case Types.Bool:
                    {
                        return Types.Bool;
                    }
            }
            ThrowParserException(token);
            return null;
        }


        /// <summary>
        /// Handles the "op" production
        /// </summary>
        private void Operator(Expression expression)
        {
            var token = NextToken();
            if (token == null)
            {
                ThrowParserException(null);
                return;
            }
            if ( token.Lexeme == Operators.Plus ||
                 token.Lexeme == Operators.Minus ||
                 token.Lexeme == Operators.Multiply ||
                 token.Lexeme == Operators.Divide ||
                 token.Lexeme == Operators.LesserThan ||
                 token.Lexeme == Operators.GreaterThan ||
                 token.Lexeme == Operators.LesserOrEqualThan ||
                 token.Lexeme == Operators.GreaterOrEqualThan ||
                 token.Lexeme == Operators.Equal ||
                 token.Lexeme == Operators.NotEqual ||
                 token.Lexeme == Operators.And )
            {
                expression.Operator = token;
                return;
            }
            ThrowParserException(token);
        }


        /// <summary>
        /// Handles the "unary" production
        /// </summary>
        private bool UnaryOperator()
        {
            var token = NextToken();
            if (!CheckToken(token, Operators.Not))
            {
                ThrowParserException(token);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Handles the "ident" production. Looks the identifier from the symbol table
        /// </summary>
        private string Identifier()
        {
            var token = NextToken() as TokenIdentifier;
            if (token == null || !SymbolTable.FindSymbol(token.Identifier))
            {
                ThrowParserException(token);
                return null;
            }
            return token.Identifier;
        }


        /// <summary>
        /// Handles the "ident'" production. Adds the identifier to the symbol table
        /// </summary>
        private string IdentifierAlt()
        {
            var token = NextToken() as TokenIdentifier;
            if ( token == null || !SymbolTable.AddSymbol(token.Identifier) )
            {
                ThrowParserException(token);
                return null;
            }
            return token.Identifier;
        }
    }
}
