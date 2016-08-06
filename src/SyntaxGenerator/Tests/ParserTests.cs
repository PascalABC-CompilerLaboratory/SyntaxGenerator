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
                    .ReadChar(c => c == 'a')
                    .Optional()
                        .ReadChar(c => c == 'a')
                    .Or()
                        .ReadChar(c => c == 'b')
                    .Or()
                        .ReadChar(c => c == 'c')
                        .Merge()
                    .ReadChar(c => c == 'c')
                    .EndAccumulation(s => Assert.AreEqual(input, s));
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
        }

        [TestMethod]
        public void ReadString()
        {
            var input = "abcd";
            new Parser(input)
                .BeginAccumulation()
                .ReadChar(c => c == 'a')
                .ReadString("bc")
                .ReadChar(c => c == 'd')
                .EndAccumulation(s => Assert.AreEqual(input, s));
        }

        [TestMethod]
        public void ReadUntil()
        {
            var input = "f<>oo<<a>bar<b>>";
            new Parser(input)
                .ReadUntil("<a>")
                .BeginAccumulation()
                .ReadString("<a>")
                .ReadUntil("<b>")
                .ReadString("<b>")
                .EndAccumulation(s => Assert.AreEqual("<a>bar<b>", s));
        }
    }
}
