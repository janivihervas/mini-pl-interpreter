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
    }
}
