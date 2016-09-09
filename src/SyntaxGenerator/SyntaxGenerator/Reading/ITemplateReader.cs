using SyntaxGenerator.CodeGeneration;
using SyntaxGenerator.TemplateNodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.Reading
{
    public interface ITemplateReader
    {
        Template ParseTemplate(string source);
        TemplateStorage ReadTemplates(IEnumerable<string> templateSources);
    }
}
