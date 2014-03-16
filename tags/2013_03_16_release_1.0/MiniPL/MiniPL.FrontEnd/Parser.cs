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
        private List<Error> _syntaxErrors;

        private static readonly List<string> FirstStatement = new List<string>
                                                             {
                                                                 ReservedKeywords.Var,
                                                                 ReservedKeywords.For,
                                                                 ReservedKeywords.Read,
                                                                 ReservedKeywords.Print,
                                                                 ReservedKeywords.Assert,
                                                                 ReservedKeywords.End
                                                             };

        private static readonly List<string> FollowStatement = new List<string>
                                                             {
                                                                 ReservedKeywords.Semicolon
                                                             };

        private static readonly List<string> FollowStatementAlt = new List<string>
                                                             {
                                                                 ReservedKeywords.Semicolon
                                                             };

        private static readonly List<string> FollowType = new List<string>
                                                             {
                                                                 ReservedKeywords.Semicolon,
                                                                 ReservedKeywords.Assignment
                                                             };


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
            _syntaxErrors = new List<Error>();
            _tokens = tokens;
            _i = 0;
            var rootNode = Statements();
            if (0 < _syntaxErrors.Count)
            {
                throw new ParserException(_syntaxErrors);
            }
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
        /// Skips tokens until one with the lexeme that is in the follow set
        /// </summary>
        /// <param name="followSet">Follow set</param>
        private void SkipTokens(List<string> followSet)
        {
            _i--;
            while (_i < _tokens.Count && !followSet.Contains(CurrentToken().Lexeme))
            {
                _i++;
            }
        }

        /// <summary>
        /// Skips tokens until one with the lexeme that is in the follow set or until next expression
        /// </summary>
        /// <param name="followSet">Follow set</param>
        private void SkipTokensUntilExpression(List<string> followSet)
        {
            _i--;
            while ( _i < _tokens.Count && !followSet.Contains(CurrentToken().Lexeme) &&
                !(CurrentToken() is TokenTerminal<int>) &&
                !(CurrentToken() is TokenTerminal<string>) &&
                !(CurrentToken() is TokenTerminal<bool>) &&
                CurrentToken().Lexeme != Operators.Not)
            {
                _i++;
            }
        }


        /// <summary>
        /// Skips tokens until one with the lexeme that is in the follow set or until next statement
        /// </summary>
        /// <param name="followSet">Follow set</param>

        private void SkipTokensUntilNextStatement(List<string> followSet )
        {
            if (_i < _tokens.Count)
            {
                _i--;
            }
            while ( _i < _tokens.Count && 
                !(CurrentToken() is TokenIdentifier) &&
                !FirstStatement.Contains(CurrentToken().Lexeme) &&
                !followSet.Contains(CurrentToken().Lexeme) )
            {
                _i++;
            }
        }


        /// <summary>
        /// Adds a syntax error to the list of errors
        /// </summary>
        /// <param name="token">Current token</param>
        /// <param name="expectedLexeme">Expected lexeme</param>
        /// <param name="useDefault">Use default message: "Was expecting {expectedLexeme}."</param>
        private void AddSyntaxError(Token token, string expectedLexeme, bool useDefault = true)
        {
            if (token == null)
            {
                _syntaxErrors.Add(
                    new Error(Scanner.Lines[Scanner.Lines.Count - 1], Scanner.Lines.Count - 1, Scanner.Lines[Scanner.Lines.Count - 1].Length, String.Format("Unexpected end of file, was expecting {0}.", expectedLexeme)));
                return;
            }
            if (useDefault)
            {
                _syntaxErrors.Add(
                    new Error(Scanner.Lines[token.Line - 1], token.Line, token.StartColumn, String.Format("Was expecting {0}.", expectedLexeme)));
            }
            else
            {
                _syntaxErrors.Add(
                    new Error(Scanner.Lines[token.Line - 1], token.Line, token.StartColumn, expectedLexeme));

            }
        }


        /// <summary>
        /// Handles the "stmts" production
        /// </summary>
        private Statements Statements()
        {
            var node = new Statements(Scanner.Lines);
            while (_i < _tokens.Count)
            {
                node.AddStatement(Statement());
                var token = NextToken();
                if ( !CheckToken(token, ReservedKeywords.Semicolon) )
                {
                    AddSyntaxError(token, ReservedKeywords.Semicolon);
                    SkipTokensUntilNextStatement(new List<string>{ReservedKeywords.End});
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
                            AddSyntaxError(token, ReservedKeywords.Colon);
                            var follow = new List<string>(Types.GetTypes());
                            follow.AddRange(FollowStatement);
                            SkipTokens(follow);
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
                            AddSyntaxError(token, ReservedKeywords.In);
                            SkipTokensUntilExpression(FollowStatement);
                        }
                        var firstExpr = Expression();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.Range))
                        {
                            AddSyntaxError(token, ReservedKeywords.Range);
                            SkipTokensUntilExpression(FollowStatement);
                        }
                        var secondExpr = Expression();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.Do))
                        {
                            AddSyntaxError(token, ReservedKeywords.Do);
                            SkipTokensUntilNextStatement(new List<string>{ReservedKeywords.Semicolon});
                        }
                        var stmts = Statements();
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.End))
                        {
                            AddSyntaxError(token, ReservedKeywords.End);
                            var follow = new List<string> {ReservedKeywords.For};
                            follow.AddRange(FollowStatement);
                            SkipTokens(follow);
                        }
                        token = NextToken();
                        if (!CheckToken(token, ReservedKeywords.For))
                        {
                            AddSyntaxError(token, ReservedKeywords.For);
                            SkipTokensUntilNextStatement(FollowStatement);
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
                            AddSyntaxError(token, Operators.ParenthesisLeft);
                            SkipTokensUntilExpression(FollowStatement);
                        }
                        var expr = Expression();
                        token = NextToken();
                        if ( !CheckToken(token, Operators.ParenthesisRight) )
                        {
                            AddSyntaxError(token, Operators.ParenthesisRight);
                            SkipTokens(FollowStatement);
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
                    AddSyntaxError(token, ReservedKeywords.Assignment);
                    SkipTokensUntilExpression(FollowStatement);
                }
                var expr = Expression();
                return new StatementVarAssignment(id, expr);
            }
            SkipTokens(FollowStatement);
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
                AddSyntaxError(token, ReservedKeywords.Assignment);
                SkipTokensUntilExpression(FollowStatementAlt);
                //return null;
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
            if (token != null && FirstStatement.Contains(token.Lexeme) || token is TokenIdentifier)
            {
                // syntax error
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
                AddSyntaxError(null, "int, string, bool, (, or variable identifier");
                return;
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
                    AddSyntaxError(token, Operators.ParenthesisRight);
                }
                expression.AddExpression(expr, addToFirst);
                return; 
            }
            _i--;
            var unary = OperandAlt();
            var tokenBool = NextToken() as TokenTerminal<bool>;
            if (tokenBool == null)
            {
                AddSyntaxError(null, "bool");
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
                AddSyntaxError(null, String.Format("{0}, {1} or {2}", Types.Int, Types.String, Types.Bool));
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
            AddSyntaxError(token, String.Format("{0}, {1} or {2}", Types.Int, Types.String, Types.Bool));
            SkipTokens(FollowType);
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
                AddSyntaxError(null, "operator symbol");
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
            var op = new List<string>
                         {
                             Operators.Plus,
                             Operators.Minus,
                             Operators.Multiply,
                             Operators.Divide,
                             Operators.LesserThan,
                             Operators.GreaterThan,
                             Operators.LesserOrEqualThan,
                             Operators.GreaterOrEqualThan,
                             Operators.Equal,
                             Operators.NotEqual,
                             Operators.And
                         };
            AddSyntaxError(token, String.Join(", ", op));
            SkipTokensUntilExpression(new List<string>());
        }


        /// <summary>
        /// Handles the "unary" production
        /// </summary>
        private bool UnaryOperator()
        {
            var token = NextToken();
            if (!CheckToken(token, Operators.Not))
            {
                AddSyntaxError(token, Operators.Not);
                _i--;
                while (_i < _tokens.Count && !(CurrentToken() is TokenTerminal<bool>))
                {
                    _i++;
                }
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
                AddSyntaxError(token, "Unknown variable", false);
                var follow = new List<string>
                                 {
                                    ReservedKeywords.Assignment,
                                    ReservedKeywords.In,
                                    ReservedKeywords.Range,
                                    ReservedKeywords.Do,
                                    ReservedKeywords.Semicolon,
                                    Operators.ParenthesisRight,
                                    Operators.Plus,
                                    Operators.Minus,
                                    Operators.Multiply,
                                    Operators.Divide,
                                    Operators.LesserThan,
                                    Operators.GreaterThan,
                                    Operators.LesserOrEqualThan,
                                    Operators.GreaterOrEqualThan,
                                    Operators.Equal,
                                    Operators.NotEqual,
                                    Operators.And
                                 };
                SkipTokens(follow);
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
                AddSyntaxError(token, "Variable name already in use", false);
                var follow = new List<string> {ReservedKeywords.Colon};
                follow.AddRange(FollowStatement);
                SkipTokens(follow);
                return null;
            }
            return token.Identifier;
        }
    }
}
