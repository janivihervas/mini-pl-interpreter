using System;
using System.Collections.Generic;
using MiniPL.FrontEnd;
using NUnit.Framework;

namespace MiniPL.UnitTests
{
    /// @author Jani Viherväs
    /// @version 27.2.2014
    ///
    /// <summary>
    /// Unit tests for the Parser class
    /// </summary>
    [TestFixture]
    public class ParserTests
    {
        private Parser _parser;
        private List<string>[] _correctSourceCodes;
        private List<Token>[] _correctSourceCodeTokens;


        [SetUp]
        protected void SetUp()
        {
            var scanner = new Scanner();
            _parser = new Parser();
            _correctSourceCodes = new[] {
                new List<string>
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
                            }, 
                new List<string>
                            {
                                "var x : int;",
                                "var y :  bool;",
                                "var z :   string;",
                            },
            new List<string>
                            {
                                "var x : int := (4 + (6 * 2))/(2-0);",
                                "var y : bool := !true & true;",
                                "var z : bool := 3 < 2;",
                                "assert(x > 2);",
                                "assert(3 >= 2);",
                                "assert(x <= 12);",
                                "assert(x = 8);",
                            }, 
            new List<string>
                            {
                                "var x : int := (4 + (6 * 2))/(2-0);",
                                "var y : bool := !true & true;",
                                "var z : bool := 3 < 2;",
                                "assert(x > 2);",
                                "assert(3 >= 2);",
                                "assert(x <= 12);",
                                "assert(x = 8);",
                                "for x in 0..5*4 do", 
                                "print x;",
                                "end for;",
                                "read x;"
                            },
            new List<string>
                            {
                                "var x : int := 3;",
                                "var y2k : bool := true;",
                                "var CONSTANT_TEST : string := \"test\";",
                                "assert(x > 2);",
                                "assert(y2k != false);",
                                "assert(CONSTANT_TEST = \"test\");"
                            }, 
            new List<string>
                            {
                                "var i : int; // := 0;",
                                "//var s : string := \"How many times?\";",
                                "var b : bool := true;", 
                            },
            new List<string>
                            {
                                "var i : int; /* := 0;",
                                "//var s : string := \"How many times?\";*/",
                                "var b : bool := true;", 
                            }};
            _correctSourceCodeTokens = new List<Token>[_correctSourceCodes.Length];
            for (var i = 0; i < _correctSourceCodeTokens.Length; i++)
            {
                _correctSourceCodeTokens[i] = scanner.Tokenize(_correctSourceCodes[i]);
            }
        }


        [Test]
        public void TestParserDoesntThrowExceptionOnCorrectSourceCode()
        {
            for ( var i = 0; i < _correctSourceCodeTokens.Length; i++ )
            {
                var tokenList = _correctSourceCodeTokens[i];
                Assert.DoesNotThrow(() => _parser.Parse(tokenList), "Source code to produce the error ( i = " + i + " ):\n\n" + String.Join("\n", _correctSourceCodes[i]) + "\n");
                SymbolTable.DeleteAllSymbols();
            }

        }
    }
}
