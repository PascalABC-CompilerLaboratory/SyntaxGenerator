using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TemplateParser;
using TemplateParser.Extensions;

namespace Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Optional()
        {
            var inputs = new string[] { "abc", "aac", "acc" };
            foreach (var input in inputs)
            {
                var parser = new Parser(input);
                parser
                    .BeginAccumulation()
                    .ReadChar('a')
                    .Optional()
                        .ReadChar('a')
                    .Or()
                        .ReadChar('b')
                    .Or()
                        .ReadChar('c')
                    .Merge()
                    .ReadChar('c')
                    .EndAccumulation(s => Assert.AreEqual(input, s));
                Assert.IsTrue(parser.IsSucceed);
            }

            inputs = new string[] { "a", "ab" };
            foreach (var input in inputs)
            {
                var parser = new Parser(input);
                parser
                    .BeginAccumulation()
                    .ReadChar(c => c == 'a')
                    .Optional()
                        .ReadChar(c => c == 'b')
                    .Or()
                        .Merge()
                    .EndAccumulation(s => Assert.AreEqual(input, s));
                Assert.IsTrue(parser.IsSucceed);
            }
        }

        [TestMethod]
        public void ReadWhile()
        {
            var input = new string('a', 1000) + 'b';
            var parser = new Parser(input);
            parser
                .BeginAccumulation()
                .ReadWhile(c => c == 'a')
                .ReadChar(c => c == 'b')
                .EndAccumulation(s => Assert.AreEqual(input, s));
            Assert.IsTrue(parser.IsSucceed);
        }

        [TestMethod]
        public void ReadString()
        {
            var input = "abcd";
            var parser = new Parser(input)
                .BeginAccumulation()
                .ReadChar(c => c == 'a')
                .ReadString("bc")
                .ReadChar(c => c == 'd')
                .EndAccumulation(s => Assert.AreEqual(input, s));
            Assert.IsTrue(parser.IsSucceed);
        }

        [TestMethod]
        public void ReadUntil()
        {
            var input = "f<>oo<<a>bar<b>>";
            var parser = new Parser(input)
                .ReadUntil("<a>")
                .BeginAccumulation()
                .ReadString("<a>")
                .ReadUntil("<b>")
                .ReadString("<b>")
                .EndAccumulation(s => Assert.AreEqual("<a>bar<b>", s));
            Assert.IsTrue(parser.IsSucceed);
        }
    }
}
