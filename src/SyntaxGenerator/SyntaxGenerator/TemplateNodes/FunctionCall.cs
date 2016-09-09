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

        public FunctionCall() : base(separator: null) { }

        /// <summary>
        /// Шаблон для генерации кода
        /// </summary>
        /// <param name="name">Выражение, по которому генерируется код</param>
        /// <param name="parameters">Параметры генерации</param>
        public FunctionCall(string name, List<IParameter> parameters, string separator = null) :
            base(separator)
        {
            Name = name;
            Parameters = parameters;
        }

        public void AddParameter(IParameter parameter) => Parameters.Add(parameter);

        public override void Accept(IVisitor visitor)
        {
            visitor.VisitFunctionalCall(this);
        }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitFunctionCall(this);
        }
    }
}
