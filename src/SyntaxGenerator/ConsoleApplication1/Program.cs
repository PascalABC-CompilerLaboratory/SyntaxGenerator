using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateParser;
using TemplateParser.Extensions;
using SyntaxGenerator.Reading;
using SyntaxGenerator.TemplateNodes;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(1000);

            var input = @"foo""bar{""asd""}\{ \}""42";

            var parser = new Parser(input)
                .ReadWhile(c => c != '"')
                .ReadInterpolatedString(s =>
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
