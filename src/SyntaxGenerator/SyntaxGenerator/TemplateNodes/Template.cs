using SyntaxGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    /// <summary>
    /// Шаблон генерации кода
    /// </summary>
    public class Template : IAbstractNode
    {
        /// <summary>
        /// Список участков кода на C# и языке генерации
        /// </summary>
        public List<ICodePart> Parts { get; set; } = new List<ICodePart>();

        public Template() { }

        public Template(IEnumerable<ICodePart> parts)
        {
            Parts = new List<ICodePart>(parts);
        }

        public string Type =>
            ((Parts?
            .Find(part => (part is SetStatement) && (part as SetStatement).VariableName == "TemplateType")
            as SetStatement).Value as FormatString).Format;

        public void AddPart(ICodePart part) => Parts.Add(part);

        public void Accept(IVisitor visitor)
        {
            visitor.VisitTemplate(this);
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitTemplate(this);
        }
    }
}
