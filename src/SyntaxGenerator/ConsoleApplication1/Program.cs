using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateParser;
using TemplateParser.Extensions;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var length = 1000000;
            var str = new string('a', length);
            var p = new Parser(str).BeginAccumulation();
            for (int i = 0; i < length; i++)
            {
                p = p
                    .Optional()
                        .ReadChar(c => c == 'b')
                    .Or()
                        .ReadChar(c => c == 'c')
                    .Or()
                        .ReadChar(c => c == 'd')
                    .Or()
                        .ReadChar(c => c == 'a')
                    .Merge();
            }
            p.EndAccumulation(s => Assert.AreEqual(str, s));

            
        }
    }
}
