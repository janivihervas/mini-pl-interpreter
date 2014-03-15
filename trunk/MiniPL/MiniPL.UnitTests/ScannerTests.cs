using System.Collections.Generic;
using MiniPL.FrontEnd;
using MiniPL.Tokens;
using NUnit.Framework;

namespace MiniPL.UnitTests
{
    /// @author Jani Viherväs
    /// @version 27.2.2014
    ///
    /// <summary>
    /// Unit tests for the Scanner class
    /// </summary>
    [TestFixture]
    public class ScannerTests
    {
        private Scanner _scanner;

        [SetUp]
        protected void SetUp()
        {
            _scanner = new Scanner();
        }

        [Test]
        public void TestScannerCanTokenizeTypes()
        {
            var lines = new List<string>
                            {
                                "var x : int;",
                                "var y :  bool;",
                                "var z :   string;",
                            };
            
            var tokens = _scanner.Tokenize(lines);

            Assert.IsTrue(3 <= tokens.Count);
            
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Types.Int &&
                                  x.Line == 1 &&
                                  x.StartColumn == 9
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Types.Bool &&
                                  x.Line == 2 &&
                                  x.StartColumn == 10
                              ));
            
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Types.String &&
                                  x.Line == 3 &&
                                  x.StartColumn == 11
                              ));
        }

        [Test]
        public void TestScannerCanTokenizeOperators()
        {
            var lines = new List<string>
                            {
                                "var x : int := (4 + (6 * 2))/(2-0);",
                                "var y : bool := !true & true;",
                                "var z : bool := 3 < 2;",
                                "assert(x > 2);",
                                "assert(3 >= 2);",
                                "assert(x <= 12);",
                                "assert(x = 8);",
                                "assert(x != 8);"
                            };

            var tokens = _scanner.Tokenize(lines);
            Assert.IsTrue(13 <= tokens.Count);

            #region Arithmetic
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.Plus &&
                                  x.Line == 1 &&
                                  x.StartColumn == 19
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.Minus &&
                                  x.Line == 1 &&
                                  x.StartColumn == 32
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.Multiply &&
                                  x.Line == 1 &&
                                  x.StartColumn == 24
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.Divide &&
                                  x.Line == 1 &&
                                  x.StartColumn == 29
                              ));
            #endregion

            #region Parenthesis
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.ParenthesisLeft &&
                                  x.Line == 7 &&
                                  x.StartColumn == 7
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.ParenthesisRight &&
                                  x.Line == 7 &&
                                  x.StartColumn == 13
                              ));

            #endregion

            #region Logical
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.And &&
                                  x.Line == 2 &&
                                  x.StartColumn == 23
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.Not &&
                                  x.Line == 2 &&
                                  x.StartColumn == 17
                              ));

            #endregion

            #region Comparison

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.GreaterThan &&
                                  x.Line == 4 &&
                                  x.StartColumn == 10
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.GreaterOrEqualThan &&
                                  x.Line == 5 &&
                                  x.StartColumn == 10
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.LesserThan &&
                                  x.Line == 3 &&
                                  x.StartColumn == 19
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.LesserOrEqualThan &&
                                  x.Line == 6 &&
                                  x.StartColumn == 10
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operators.Equal &&
                                  x.Line == 7 &&
                                  x.StartColumn == 10
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                      x.Lexeme == Operators.NotEqual &&
                      x.Line == 8 &&
                      x.StartColumn == 10
                  ));

            #endregion
        }

        [Test]
        public void TestCanTokenizeReservedKeyWords()
        {
            var lines = new List<string>
                            {
                                "var x : int := (4 + (6 * 2))/(2-0);",
                                "var y : bool := !true & true;",
                                "var z : bool := 3 < 2;",
                                "assert(x > 2);",
                                "assert(3 >= 2);",
                                "assert(x <= 12);",
                                "assert(x = 8);",
                                "for x in 0..nTimes-1 do", 
                                "print x;",
                                "end for;",
                                "read nTimes;"
                            };

            var tokens = _scanner.Tokenize(lines);
            Assert.IsTrue(12 <= tokens.Count);

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.Assert &&
                                  x.Line == 4 &&
                                  x.StartColumn == 1
                              ));
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.Do &&
                                  x.Line == 8 &&
                                  x.StartColumn == 22
                              ));
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.End &&
                                  x.Line == 10 &&
                                  x.StartColumn == 1
                              ));
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.For &&
                                  x.Line == 8 &&
                                  x.StartColumn == 1
                              ));
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.In &&
                                  x.Line == 8 &&
                                  x.StartColumn == 7
                              ));
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.Print &&
                                  x.Line == 9 &&
                                  x.StartColumn == 1
                              ));
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.Read &&
                                  x.Line == 11 &&
                                  x.StartColumn == 1
                              ));
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.Var &&
                                  x.Line == 3 &&
                                  x.StartColumn == 1
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.Assignment &&
                                  x.Line == 2 &&
                                  x.StartColumn == 14
                              ));
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.Colon &&
                                  x.Line == 1 &&
                                  x.StartColumn == 7
                              ));
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.Range &&
                                  x.Line == 8 &&
                                  x.StartColumn == 11
                              ));
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == ReservedKeywords.Semicolon &&
                                  x.Line == 6 &&
                                  x.StartColumn == 16
                              ));
        }


        [Test]
        public void TestCanTokenizeVariableIdentifiers()
        {
            var lines = new List<string>
                            {
                                "var x : int := 3;",
                                "var y2k : bool := true;",
                                "var CONSTANT_TEST : string := \"test\";",
                                "assert(x > 2);",
                                "assert(y2k != false);",
                                "assert(CONSTANT_TEST = \"test\");"
                            };

            var tokens = _scanner.Tokenize(lines);
            Assert.IsTrue(6 <= tokens.Count);

            Assert.IsTrue(tokens.Exists(x =>
                                        x.Lexeme == "x" &&
                                        x.Line == 1 &&
                                        x.StartColumn == 5
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                        x.Lexeme == "x" &&
                                        x.Line == 4 &&
                                        x.StartColumn == 8
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                            x.Lexeme == "y2k" &&
                            x.Line == 2 &&
                            x.StartColumn == 5
                  ));

            Assert.IsTrue(tokens.Exists(x =>
                                        x.Lexeme == "y2k" &&
                                        x.Line == 5 &&
                                        x.StartColumn == 8
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                            x.Lexeme == "CONSTANT_TEST" &&
                            x.Line == 3 &&
                            x.StartColumn == 5
                  ));

            Assert.IsTrue(tokens.Exists(x =>
                                        x.Lexeme == "CONSTANT_TEST" &&
                                        x.Line == 6 &&
                                        x.StartColumn == 8
                              ));
        }

        [Test]
        public void TestTokenTerminalBool()
        {
            var s = "var x : bool := true;";
            var tokens = _scanner.Tokenize(new List<string> { s });
            var token = (TokenTerminal<bool>)tokens.Find(x => x is TokenTerminal<bool>);

            Assert.IsTrue(token.Value);
            Assert.AreEqual("true", token.Lexeme);

            s = "var x : bool := false;";
            tokens = _scanner.Tokenize(new List<string> { s });
            token = (TokenTerminal<bool>)tokens.Find(x => x is TokenTerminal<bool>);

            Assert.IsFalse(token.Value);
            Assert.AreEqual("false", token.Lexeme);
        }

        [Test]
        public void TestTokenTerminalInt()
        {
            var s = "var x : int := 153;";
            var tokens = _scanner.Tokenize(new List<string> { s });
            var token = (TokenTerminal<int>)tokens.Find(x => x is TokenTerminal<int>);

            Assert.AreEqual(153, token.Value);
            Assert.AreEqual("153", token.Lexeme);

            s = "var x : int := -1133;";
            tokens = _scanner.Tokenize(new List<string> { s });
            token = null;
            Token previousToken = null;
            for (var i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] is TokenTerminal<int>)
                {
                    token = (TokenTerminal<int>) tokens[i];
                    previousToken = tokens[i - 1];
                }
            }

            Assert.IsNotNull(token);
            Assert.IsNotNull(previousToken);

            Assert.AreEqual(1133, token.Value);
            Assert.AreEqual("1133", token.Lexeme);

            Assert.AreEqual(Operators.Minus, previousToken.Lexeme);
        }

        [Test]
        public void TestTokenTerminalString()
        {
            const string s = "var x : string := \"test\";";
            var tokens = _scanner.Tokenize(new List<string> { s });

            TokenTerminal<string> token = null;
            Token nextToken = null;

            for ( var i = 0; i < tokens.Count; i++ )
            {
                if ( tokens[i] is TokenTerminal<string> )
                {
                    token = (TokenTerminal<string>)tokens[i];
                    nextToken = tokens[i + 1];
                }
            }

            Assert.IsNotNull(token);
            Assert.IsNotNull(nextToken);

            Assert.AreEqual("test", token.Value);
            Assert.AreEqual("test", token.Lexeme);
            Assert.AreEqual(";", nextToken.Lexeme);
        }

        [Test]
        public void CanTokenizeCorrectSourceCode()
        {
            var lines = new List<string>
                            {
                                "var nTimes : int := 0;",
                                "var s : string := \"How many times?\";",
                                "print s;", 
                                "read nTimes;", 
                                "var x : int;",
                                "for x in 0..nTimes-1 do ",
                                "    print x;",
                                "    print \" : Hello, World!\\n\";",
                                "end for;",
                                "var b : bool := x = nTimes;",
                                "assert (b);"
                            };

            var tokens = _scanner.Tokenize(lines);
            Assert.AreEqual(57, tokens.Count);
            var i = 0;

            var token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Var, token.Lexeme); Assert.AreEqual(1, token.Line); Assert.AreEqual(1, token.StartColumn); 

            token = tokens[i++];
            Assert.AreEqual("nTimes", token.Lexeme);             
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("nTimes", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Colon, token.Lexeme);              
            token = tokens[i++];
            Assert.AreEqual(Types.Int, token.Lexeme);
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Assignment, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("0", token.Lexeme);  
            Assert.IsTrue(token is TokenTerminal<int>);
            Assert.AreEqual(0, ((TokenTerminal<int>)token).Value);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Var, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("s", token.Lexeme);  
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("s", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Colon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(Types.String, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Assignment, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("How many times?", token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Print, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("s", token.Lexeme);  
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("s", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Read, token.Lexeme);  
            
            token = tokens[i++]; 
            Assert.AreEqual("nTimes", token.Lexeme);  
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("nTimes", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Var, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("x", token.Lexeme);  
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("x", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Colon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(Types.Int, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.For, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("x", token.Lexeme);  
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("x", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.In, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("0", token.Lexeme);  
            Assert.IsTrue(token is TokenTerminal<int>);
            Assert.AreEqual(0, ((TokenTerminal<int>)token).Value);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Range, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("nTimes", token.Lexeme);  
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("nTimes", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(Operators.Minus, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("1", token.Lexeme);  
            Assert.IsTrue(token is TokenTerminal<int>);
            Assert.AreEqual(1, ((TokenTerminal<int>)token).Value);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Do, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Print, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("x", token.Lexeme);  
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("x", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Print, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(" : Hello, World!\\n", token.Lexeme);  
            Assert.IsTrue(token is TokenTerminal<string>);
            Assert.AreEqual(" : Hello, World!\\n", ((TokenTerminal<string>)token).Value);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.End, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.For, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Var, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("b", token.Lexeme);  
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("b", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Colon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(Types.Bool, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Assignment, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("x", token.Lexeme);  
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("x", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(Operators.Equal, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("nTimes", token.Lexeme);  
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("nTimes", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Assert, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(Operators.ParenthesisLeft, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual("b", token.Lexeme);  
            Assert.IsTrue(token is TokenIdentifier);
            Assert.AreEqual("b", ((TokenIdentifier)token).Identifier);

            token = tokens[i++];
            Assert.AreEqual(Operators.ParenthesisRight, token.Lexeme);  
            
            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);  

            Assert.AreEqual(tokens.Count, i);
        }

        [Test]
        public void CanSkipLineComments()
        {
            var lines = new List<string>
                            {
                                "var i : int; // := 0;",
                                "//var s : string := \"How many times?\";",
                                "var b : bool := true;", 
                            };

            var tokens = _scanner.Tokenize(lines);
            Assert.AreEqual(12, tokens.Count);
            var i = 0;

            var token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Var, token.Lexeme);
            Assert.AreEqual(1, token.Line);

            token = tokens[i++];
            Assert.AreEqual("i", token.Lexeme);
            Assert.AreEqual("i", ((TokenIdentifier)token).Identifier);
            Assert.AreEqual(1, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Colon, token.Lexeme);
            Assert.AreEqual(1, token.Line);

            token = tokens[i++];
            Assert.AreEqual(Types.Int, token.Lexeme);
            Assert.AreEqual(1, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);
            Assert.AreEqual(1, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Var, token.Lexeme);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual("b", token.Lexeme);
            Assert.AreEqual("b", ((TokenIdentifier)token).Identifier);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Colon, token.Lexeme);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual(Types.Bool, token.Lexeme);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Assignment, token.Lexeme);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual("true", token.Lexeme);
            Assert.IsTrue(((TokenTerminal<bool>)token).Value);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);
            Assert.AreEqual(3, token.Line);

            Assert.AreEqual(i, tokens.Count);
        }


        [Test]
        public void CanSkipMultiLineComments()
        {
            var lines = new List<string>
                            {
                                "var i : int; /* := 0;",
                                "//var s : string := \"How many times?\";*/",
                                "var b : bool := true;", 
                            };

            var tokens = _scanner.Tokenize(lines);
            Assert.AreEqual(12, tokens.Count);
            var i = 0;

            var token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Var, token.Lexeme);
            Assert.AreEqual(1, token.Line);

            token = tokens[i++];
            Assert.AreEqual("i", token.Lexeme);
            Assert.AreEqual("i", ((TokenIdentifier)token).Identifier);
            Assert.AreEqual(1, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Colon, token.Lexeme);
            Assert.AreEqual(1, token.Line);

            token = tokens[i++];
            Assert.AreEqual(Types.Int, token.Lexeme);
            Assert.AreEqual(1, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);
            Assert.AreEqual(1, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Var, token.Lexeme);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual("b", token.Lexeme);
            Assert.AreEqual("b", ((TokenIdentifier)token).Identifier);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Colon, token.Lexeme);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual(Types.Bool, token.Lexeme);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Assignment, token.Lexeme);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual("true", token.Lexeme);
            Assert.IsTrue(((TokenTerminal<bool>)token).Value);
            Assert.AreEqual(3, token.Line);

            token = tokens[i++];
            Assert.AreEqual(ReservedKeywords.Semicolon, token.Lexeme);
            Assert.AreEqual(3, token.Line);

            Assert.AreEqual(i, tokens.Count);
        }

        [Test]
        public void TestScannerProducesErrorTokens()
        {
            var lines = new List<string>
                            {
                                "var i : i.n.t := 1;"
                            };
            var tokens = _scanner.Tokenize(lines);

            Assert.AreEqual(11, tokens.Count);
            
            var error1 = tokens[4] as TokenError;
            var error2 = tokens[6] as TokenError;
            
            Assert.IsNotNull(error1);
            Assert.IsNotNull(error2);

            Assert.AreEqual(10, error1.StartColumn);
            Assert.AreEqual(1, error1.Line);
            Assert.AreEqual(".", error1.ErrorLexeme);

            Assert.AreEqual(12, error2.StartColumn);
            Assert.AreEqual(1, error2.Line);
            Assert.AreEqual(".", error2.ErrorLexeme);
        }

        
        [Test]
        public void TestScannerCombinesErrorTokens()
        {
            var lines = new List<string>
                            {
                                ". .,, . var i : int;"
                            };
            var tokens = _scanner.Tokenize(lines);

            Assert.AreEqual(6, tokens.Count);

            var error = tokens[0] as TokenError;

            Assert.IsNotNull(error);

            Assert.AreEqual(1, error.StartColumn);
            Assert.AreEqual(1, error.Line);
            Assert.AreEqual(". .,, .", error.ErrorLexeme);

        }
    }
}
