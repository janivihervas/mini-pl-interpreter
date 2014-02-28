using System.Collections.Generic;
using MiniPL.FrontEnd;
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

            Assert.IsTrue(3 >= tokens.Count);
            
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Type.Int &&
                                  x.Line == 1 &&
                                  x.StartColumn == 9
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Type.Bool &&
                                  x.Line == 2 &&
                                  x.StartColumn == 10
                              ));
            
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Type.String &&
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
                            };

            var tokens = _scanner.Tokenize(lines);
            Assert.IsTrue(13 <= tokens.Count);

            #region Arithmetic
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.Plus &&
                                  x.Line == 1 &&
                                  x.StartColumn == 19
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.Minus &&
                                  x.Line == 1 &&
                                  x.StartColumn == 32
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.Multiply &&
                                  x.Line == 1 &&
                                  x.StartColumn == 24
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.Divide &&
                                  x.Line == 1 &&
                                  x.StartColumn == 29
                              ));
            #endregion

            #region Parenthesis
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.ParenthesisLeft &&
                                  x.Line == 7 &&
                                  x.StartColumn == 7
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.ParenthesisRight &&
                                  x.Line == 7 &&
                                  x.StartColumn == 13
                              ));

            #endregion

            #region Logical
            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.And &&
                                  x.Line == 2 &&
                                  x.StartColumn == 23
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.Not &&
                                  x.Line == 2 &&
                                  x.StartColumn == 17
                              ));

            #endregion

            #region Comparison

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.GreaterThan &&
                                  x.Line == 4 &&
                                  x.StartColumn == 10
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.GreaterOrEqualThan &&
                                  x.Line == 5 &&
                                  x.StartColumn == 10
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.LesserThan &&
                                  x.Line == 3 &&
                                  x.StartColumn == 19
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.LesserOrEqualThan &&
                                  x.Line == 6 &&
                                  x.StartColumn == 10
                              ));

            Assert.IsTrue(tokens.Exists(x =>
                                  x.Lexeme == Operator.Equal &&
                                  x.Line == 7 &&
                                  x.StartColumn == 10
                              ));

            #endregion
        }
    }
}
