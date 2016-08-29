using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public class FunctionCall : TemplateCode
    {
        public string Name { get; set; }

        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

        public FunctionCall() { }

        /// <summary>
        /// Шаблон для генерации кода
        /// </summary>
        /// <param name="name">Выражение, по которому генерируется код</param>
        /// <param name="parameters">Параметры генерации</param>
        public FunctionCall(string name, List<Parameter> parameters)
        {
            Name = name;
            Parameters = parameters;
        }

        public void AddParameter(Parameter parameter) => Parameters.Add(parameter);
    }
}
