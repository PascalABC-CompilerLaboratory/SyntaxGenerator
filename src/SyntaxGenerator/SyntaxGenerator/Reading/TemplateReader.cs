using SyntaxGenerator.TemplateNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TemplateParser;
using TemplateParser.Extensions;
using static System.Char;
using static TemplateParser.ParserHelper;

namespace SyntaxGenerator.Reading
{
    public class TemplateReader : IReader
    {
        public Template ParseTemplate(string source)
        {
            Template template = null;
            new Parser(source).ReadTemplate(t => template = t);
            return template;
        }
    }
}
