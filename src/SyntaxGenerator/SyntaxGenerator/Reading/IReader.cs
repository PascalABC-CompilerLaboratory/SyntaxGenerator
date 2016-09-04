using SyntaxGenerator.TemplateNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.Reading
{
    public interface IReader
    {
        Template ParseTemplate(string source);
    }
}
