using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public class CSharpCode : CodePart
    {
        public string Code { get; set; }

        public CSharpCode() { }

        public CSharpCode(string code)
        {
            Code = code;
        }
    }
}
