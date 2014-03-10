using System.Collections.Generic;
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
    }
}
