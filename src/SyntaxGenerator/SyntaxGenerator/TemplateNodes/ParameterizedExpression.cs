using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public class ParameterizedExpression : TemplateCode
    {
        public Expression Expression { get; set; }

        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        public ParameterizedExpression() { }

        /// <summary>
        /// Шаблон для генерации кода
        /// </summary>
        /// <param name="core">Выражение, по которому генерируется код</param>
        /// <param name="parameters">Параметры генерации</param>
        public ParameterizedExpression(Expression core, List<Parameter> parameters)
        {
            Expression = core;
            Parameters = parameters;
        }

        public void AddParameter(Parameter parameter) => Parameters.Add(parameter);
    }
}
