using HelperFunctions;
using NUnit.Framework;

namespace MiniPL.UnitTests
{
    /// @author Jani Viherväs
    /// @version 3.3.2014
    ///
    /// <summary>
    /// Unit tests for StringParse class
    /// </summary>
    [TestFixture]
    public class StringParseTests
    {
        [Test]
        public void TestScanStringCanScanRegularStrings()
        {
            Assert.AreEqual("test", StringParse.ScanString("\"test\""));
            Assert.AreEqual("test", StringParse.ScanString(" \"test\"", 1));
            Assert.AreEqual("multiple words", StringParse.ScanString("\"multiple words\""));
            Assert.AreEqual("multiple words and numbers 2221123", StringParse.ScanString("\"multiple words and numbers 2221123\""));
        }

        [Test]
        public void TestScanStringCanScanStringsWithEscapes()
        {
            Assert.AreEqual("te\"st", StringParse.ScanString("\"te\\\"st\""));                          // te\"st => te"st
            Assert.AreEqual("\"test\"", StringParse.ScanString("\"\\\"test\\\"\""));                    // "\"test\"" => "test"
            Assert.AreEqual("line break \\n", StringParse.ScanString("\"line break \\n\""));            // line break \n => line break \n
            Assert.AreEqual("tab \\t tab", StringParse.ScanString("\"tab \\t tab\""));                  // tab \t tab => tab \t tab
            Assert.AreEqual("carriage \\r return", StringParse.ScanString("\"carriage \\r return\""));  // carriage \r return => carriage \r return
        }

        [Test]
        public void TestSkipString()
        {
            var test = "\"test\";";
            Assert.AreEqual(';', test[StringParse.SkipString(test)]);
            test = "\"te\\\"st\";";
            Assert.AreEqual(';', test[StringParse.SkipString(test)]);
            test = "\"te\\st\";";
            Assert.AreEqual(';', test[StringParse.SkipString(test)]);
            test = "\"\\\"test\\\"\";";
            Assert.AreEqual(';', test[StringParse.SkipString(test)]);
            test = "\"te\\tst\";";
            Assert.AreEqual(';', test[StringParse.SkipString(test)]);
            test = "\"test \\n\";";
            Assert.AreEqual(';', test[StringParse.SkipString(test)]);

        }
    }
}
