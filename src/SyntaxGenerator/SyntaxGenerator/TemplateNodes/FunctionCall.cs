using SyntaxGenerator.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public class FunctionCall : Expression
    {
        public string Name { get; set; }

        public List<IParameter> Parameters { get; set; } = new List<IParameter>();

        public FunctionCall() : base(separator: "") { }

        /// <summary>
        /// Шаблон для генерации кода
        /// </summary>
        /// <param name="name">Выражение, по которому генерируется код</param>
        /// <param name="parameters">Параметры генерации</param>
        public FunctionCall(string name, List<IParameter> parameters, string separator = "") :
            base(separator)
        {
            Name = name;
            Parameters = parameters;
        }

        public void AddParameter(IParameter parameter) => Parameters.Add(parameter);

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
