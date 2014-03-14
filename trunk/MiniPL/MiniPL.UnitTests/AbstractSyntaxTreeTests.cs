using System;
using System.Collections.Generic;
using System.IO;
using MiniPL.AbstractSyntaxTree;
using MiniPL.FrontEnd;
using NUnit.Framework;

namespace MiniPL.UnitTests
{
    /// @author Jani Viherväs
    /// @version 10.3.2014
    /// 
    /// <summary>
    /// Tests for abstract syntax tree
    /// </summary>
    [TestFixture]
    public class AbstractSyntaxTreeTests
    {
        private Scanner _scanner;
        private Parser _parser;

        [SetUp]
        protected void SetUp()
        {
            _scanner = new Scanner();
            _parser = new Parser();
            Statement.DeleteAllVariables();
        }

        [Test]
        public void TestVarInitializeSimpleValueExecute()
        {
            var lines = new List<string>
                            {
                                "var i1 : int := 1;",
                                "var i11 : int := i1;",
                                "var s1 : string := \"test\";",
                                "var b1 : bool := false;",
                                "var i2 : int;",
                                "var s2 : string;",
                                "var b2 : bool;"
                            };
            var tree = _parser.Parse(_scanner.Tokenize(lines));
            tree.Execute();

            Assert.IsNotNull(Statement.GetVariable("i2"));
            Assert.IsNotNull(Statement.GetVariable("s2"));
            Assert.IsNotNull(Statement.GetVariable("b2"));

            var varInt = Statement.GetVariable("i1") as VariableType<int>;
            var varInt2 = Statement.GetVariable("i11") as VariableType<int>;
            var varString = Statement.GetVariable("s1") as VariableType<string>;
            var varBool = Statement.GetVariable("b1") as VariableType<bool>;

            Assert.IsNotNull(varInt);
            Assert.IsNotNull(varInt2);
            Assert.IsNotNull(varString);
            Assert.IsNotNull(varBool);

            Assert.AreEqual(1, varInt.Value);
            Assert.AreEqual(1, varInt2.Value);
            Assert.AreEqual("test", varString.Value);
            Assert.AreEqual(false, varBool.Value);
        }

        [Test]
        public void TestVarInitializeComplexIntValues()
        {
            var lines = new List<string>
                            {
                                "var i1 : int := (1 + 3) * (4 / 2);",
                                "var i2 : int := (3 + i1) * 2;", // grammar restricts calculating more than two operands without parentheses
                                "var i3 : int := 3 + (1-3);",    // TODO: change grammar if there's time left
                                "var i4 : int := 2 + ((2 - (3*6)) - (1+2));",
                            };                                   
            var tree = _parser.Parse(_scanner.Tokenize(lines));
            tree.Execute();

            var varInt = Statement.GetVariable("i1") as VariableType<int>;
            var varInt2 = Statement.GetVariable("i2") as VariableType<int>;
            var varInt3 = Statement.GetVariable("i3") as VariableType<int>;
            var varInt4 = Statement.GetVariable("i4") as VariableType<int>;

            Assert.IsNotNull(varInt);
            Assert.IsNotNull(varInt2);
            Assert.IsNotNull(varInt3);
            Assert.IsNotNull(varInt4);

            Assert.AreEqual(8, varInt.Value);
            Assert.AreEqual(22, varInt2.Value);
            Assert.AreEqual(1, varInt3.Value);
            Assert.AreEqual(-17, varInt4.Value); 
        }

        [Test]
        public void TestVarInitializeComplexStringValues()
        {
            var lines = new List<string>
                            {
                                "var s1 : string := \"test\" + \"test\";",
                                "var s2 : string := \"t\" + 2;", 
                                "var s3 : string := 2 + \"t\";", 
                                "var s4 : string := true + \"false\";",
                                "var s5 : string := \"false\" + true;", 
                                "var s6 : string := \"bool \" + (!true & !false);", 
                                "var s7 : string := \"int \" + (3 - (2*3));", 
                            };
            var tree = _parser.Parse(_scanner.Tokenize(lines));
            tree.Execute();

            var var1 = Statement.GetVariable("s1") as VariableType<string>;
            var var2 = Statement.GetVariable("s2") as VariableType<string>;
            var var3 = Statement.GetVariable("s3") as VariableType<string>;
            var var4 = Statement.GetVariable("s4") as VariableType<string>;
            var var5 = Statement.GetVariable("s5") as VariableType<string>;
            var var6 = Statement.GetVariable("s6") as VariableType<string>;
            var var7 = Statement.GetVariable("s7") as VariableType<string>;

            Assert.IsNotNull(var1);
            Assert.IsNotNull(var2);
            Assert.IsNotNull(var3);
            Assert.IsNotNull(var4);
            Assert.IsNotNull(var5);
            Assert.IsNotNull(var6);
            Assert.IsNotNull(var7);

            Assert.AreEqual("testtest", var1.Value);
            Assert.AreEqual("t2", var2.Value);
            Assert.AreEqual("2t", var3.Value);
            Assert.AreEqual("truefalse", var4.Value);
            Assert.AreEqual("falsetrue", var5.Value);
            Assert.AreEqual("bool false", var6.Value);
            Assert.AreEqual("int -3", var7.Value);
        }

        [Test]
        public void TestVarInitializeComplexBooleanValues()
        {
            var lines = new List<string>
                            {
                                "var b1 : bool := !false & true;",
                                "var b2 : bool := false != false;", 
                                "var b3 : bool := true = true;", 
                                "var b4 : bool := \"t\" = \"t\";",
                                "var b5 : bool := 3 < 2;", 
                                //"var b6 : bool := (\"\t\" + \"t\") != (\"tt\" + \"t\");",  // Doesn't work
                                //"var b7 : bool := 0 != (3 - (2*3));",                      // Doesn't work
                            };
            var tree = _parser.Parse(_scanner.Tokenize(lines));
            tree.Execute();

            var var1 = Statement.GetVariable("b1") as VariableType<bool>;
            var var2 = Statement.GetVariable("b2") as VariableType<bool>;
            var var3 = Statement.GetVariable("b3") as VariableType<bool>;
            var var4 = Statement.GetVariable("b4") as VariableType<bool>;
            var var5 = Statement.GetVariable("b5") as VariableType<bool>;

            Assert.IsNotNull(var1);
            Assert.IsNotNull(var2);
            Assert.IsNotNull(var3);
            Assert.IsNotNull(var4);
            Assert.IsNotNull(var5);

            Assert.AreEqual(true, var1.Value);
            Assert.AreEqual(false, var2.Value);
            Assert.AreEqual(true, var3.Value);
            Assert.AreEqual(true, var4.Value);
            Assert.AreEqual(false, var5.Value);
        }

        [Test]
        public void TestVarAssignmentExecute()
        {
            var lines = new List<string>
                            {
                                "var i : int;",
                                "i := 1;", 
                                "var b : bool;",
                                "b := true;", 
                                "var s : string;",
                                "s := \"test\";", 
                            };
            var tree = _parser.Parse(_scanner.Tokenize(lines));
            tree.Execute();

            var var1 = Statement.GetVariable("i") as VariableType<int>;
            var var2 = Statement.GetVariable("b") as VariableType<bool>;
            var var3 = Statement.GetVariable("s") as VariableType<string>;

            Assert.IsNotNull(var1);
            Assert.IsNotNull(var2);
            Assert.IsNotNull(var3);

            Assert.AreEqual(1, var1.Value);
            Assert.AreEqual(true, var2.Value);
            Assert.AreEqual("test", var3.Value);
        }


        [Test]
        public void TestPrintExecute()
        {
            var lines = new List<string>
                            {
                                "var s : string := \"printing test\";",
                                "var i : int := 2;",
                                "var b : bool := true;",
                                "print s;", 
                                "print i;", 
                                "print b;", 
                            };
            var tree = _parser.Parse(_scanner.Tokenize(lines));

            var writer = new StringWriter();
            Console.SetOut(writer);

            Assert.AreEqual("", writer.ToString());

            tree.Execute();
            Assert.AreEqual("printing test" + "2" + "true", writer.ToString());
        }


        [Test]
        public void TestReadExecute()
        {
            var lines = new List<string>
                            {
                                "var s : string := \"printing test\";",
                                "read s;", 
                            };
            var tree = _parser.Parse(_scanner.Tokenize(lines));

            Console.SetIn(new StringReader("kapow!"));
            tree.Execute();
            var s = Statement.GetVariable("s") as VariableType<string>;

            lines = new List<string>
                            {
                                "var i : int := 2;",
                                "read i;", 
                            };
            tree = _parser.Parse(_scanner.Tokenize(lines));

            Console.SetIn(new StringReader("3"));
            tree.Execute();
            
            var i = Statement.GetVariable("i") as VariableType<int>;

            lines = new List<string>
                            {
                                "var b : bool := true;",
                                "read b;", 
                            };
            tree = _parser.Parse(_scanner.Tokenize(lines));

            Console.SetIn(new StringReader("false"));
            tree.Execute();
            var b = Statement.GetVariable("b") as VariableType<bool>;

            Assert.NotNull(s);
            Assert.NotNull(i);
            Assert.NotNull(b);

            Assert.AreEqual("kapow!", s.Value);
            Assert.AreEqual(3, i.Value);
            Assert.AreEqual(false, b.Value);
        }

        [Test]
        public void TestForLoopExecute()
        {
            var lines = new List<string>
                            {
                                "var i : int;",
                                "for i in 1 .. 10 do", 
                                "    print i; print \" \";",
                                "end for;"
                            };
            var tree = _parser.Parse(_scanner.Tokenize(lines));

            var writer = new StringWriter();
            Console.SetOut(writer);

            Assert.AreEqual("", writer.ToString());

            tree.Execute();
            Assert.AreEqual("1 2 3 4 5 6 7 8 9 10 ", writer.ToString());
        }
    }
}
