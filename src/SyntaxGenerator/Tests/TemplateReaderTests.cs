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
            var input = @"foo""bar{""a""}bar2{b25}\{ \}""42";

            var parser = new Parser(input)
                .ReadWhile(c => c != '"')
                .ReadString(s =>
                {
                    Assert.AreEqual("bar{0}bar2{1}{ }", s.Format);
                    Assert.AreEqual("a", (s.Arguments[0] as FormatString).Format);
                    Assert.AreEqual("b25", (s.Arguments[1] as Identifier).Value);
                })
                .ReadString("42");
            Assert.IsTrue(parser.IsSucceed);
        }
    }
}
