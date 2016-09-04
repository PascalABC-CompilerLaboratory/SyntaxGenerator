using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public interface IExpression : ITemplateCode
    {
        string Separator { get; set; }
    }
}
