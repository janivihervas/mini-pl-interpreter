using System;
using System.Collections.Generic;
using MiniPL.AbstractSyntaxTree;
using MiniPL.FrontEnd;
using MiniPL.Tokens;
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
                                "x := 0;",
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
                Statements ast = null;
                Assert.DoesNotThrow(() => ast = _parser.Parse(tokenList), "Source code to produce the error ( i = " + i + " ):\n\n" + String.Join("\n", _correctSourceCodes[i]) + "\n");
                Console.WriteLine(ast.ToString());
                SymbolTable.DeleteAllSymbols();
            }

        }

        [Test]
        public void TestParserProducesCorrectAbstractSyntaxTree()
        {
            var tokens = _correctSourceCodeTokens[1];
            var ast = _parser.Parse(tokens);
            SymbolTable.DeleteAllSymbols();

            Assert.AreEqual(3, ast.StatementList.Count);

            Assert.IsTrue(ast.StatementList[0] is StatementVarInitialize);
            Assert.IsTrue(ast.StatementList[1] is StatementVarInitialize);
            Assert.IsTrue(ast.StatementList[2] is StatementVarInitialize);

            var statement = ast.StatementList[0] as StatementVarInitialize;
            Assert.AreEqual("x", statement.Identifier);
            Assert.AreEqual(Types.Int, statement.Type);
            Assert.IsNull(statement.Expression);

            statement = ast.StatementList[1] as StatementVarInitialize;
            Assert.AreEqual("y", statement.Identifier);
            Assert.AreEqual(Types.Bool, statement.Type);
            Assert.IsNull(statement.Expression);

            statement = ast.StatementList[2] as StatementVarInitialize;
            Assert.AreEqual("z", statement.Identifier);
            Assert.AreEqual(Types.String, statement.Type);
            Assert.IsNull(statement.Expression);

            tokens = _correctSourceCodeTokens[0];
            ast = _parser.Parse(tokens);
            SymbolTable.DeleteAllSymbols();

            Assert.AreEqual(9, ast.StatementList.Count);

            Assert.IsTrue(ast.StatementList[0] is StatementVarInitialize);
            Assert.IsTrue(ast.StatementList[1] is StatementVarInitialize);
            Assert.IsTrue(ast.StatementList[2] is StatementPrint);
            Assert.IsTrue(ast.StatementList[3] is StatementRead);
            Assert.IsTrue(ast.StatementList[4] is StatementVarInitialize);
            Assert.IsTrue(ast.StatementList[5] is StatementVarAssignment);
            Assert.IsTrue(ast.StatementList[6] is StatementFor);
            Assert.IsTrue(ast.StatementList[7] is StatementVarInitialize);
            Assert.IsTrue(ast.StatementList[8] is StatementAssert);

            var print = ast.StatementList[2] as StatementPrint;
            Assert.AreEqual("s", ((TokenIdentifier)print.Expression.FirstOperand).Identifier);

            var read = ast.StatementList[3] as StatementRead;
            Assert.AreEqual("nTimes", read.Identifier);

            var assign = ast.StatementList[5] as StatementVarAssignment;
            Assert.AreEqual("x", assign.Identifier);
            Assert.AreEqual(0, ((TokenTerminal<int>)assign.Expression.FirstOperand).Value);

            var statementsFor = (ast.StatementList[6] as StatementFor).Statements.StatementList;
            Assert.AreEqual(2, statementsFor.Count);
            Assert.IsTrue(statementsFor[0] is StatementPrint);
            Assert.IsTrue(statementsFor[1] is StatementPrint);
        }


    }
}
