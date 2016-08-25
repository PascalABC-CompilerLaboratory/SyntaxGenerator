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
    public class Template : AbstractNode
    {
        /// <summary>
        /// Список участков кода на C# и языке генерации
        /// </summary>
        public List<CodePart> Parts { get; set; } = new List<CodePart>();

        public Template() { }

        public Template(List<CodePart> parts)
        {
            Parts = parts;
        }

        public void AddPart(CodePart part) => Parts.Add(part);
    }
}
