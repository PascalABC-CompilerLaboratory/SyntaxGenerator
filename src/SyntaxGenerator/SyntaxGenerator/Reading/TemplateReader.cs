using SyntaxGenerator.CodeGeneration;
using SyntaxGenerator.CodeGeneration.Visitors;
using SyntaxGenerator.TemplateNodes;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class TemplateReader : ITemplateReader
    {
        public Template ParseTemplate(string source)
        {
            Template template = null;
            var parser = new Parser(source).ReadTemplate(t => template = t);
            return TemplateCleaner.Visit(template);
        }

        public TemplateStorage ReadTemplates(IEnumerable<string> templateSources)
        {
            TemplateStorage templateStorage = new TemplateStorage();
            
            foreach (Template template in templateSources.Select(s => ParseTemplate(s)))
            {
                templateStorage.AddTemplate(template.Type, template);
            }

            return templateStorage;
        }
    }
}
