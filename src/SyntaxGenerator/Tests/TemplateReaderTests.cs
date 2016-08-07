using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxGenerator.Reading;
using TemplateParser;
using TemplateParser.Extensions;
using static TemplateParser.ParserHelper;
using SyntaxGenerator.TemplateNodes;

namespace Tests
{
    [TestClass]
    public class TemplateReaderTests
    {
        [TestMethod]
        public void ReadString()
        {
            var input = @"foo""bar{""asd""}\{ \}""42";

            var parser = new Parser(input)
                .ReadWhile(c => c != '"')
                .ReadString(s =>
                {
                    Assert.AreEqual("bar{0}{ }", s.Format);
                    Assert.AreEqual("asd", (s.Arguments[0] as FormatString).Format);
                })
                .ReadChar('4')
                .ReadChar('2');
            Assert.IsTrue(parser.IsSucceed);
        }
    }
}
