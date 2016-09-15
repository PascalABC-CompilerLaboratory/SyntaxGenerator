using SyntaxGenerator.TemplateNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    public class FunctionParameters : IFunctionParameters
    {
        Dictionary<string, IParameter> _variables;

        public FunctionParameters(IEnumerable<IParameter> parameters)
        {
            _variables = new Dictionary<string, IParameter>(
                parameters.ToDictionary(param => param.Name));
        }

        public T GetValue<T>(string name)
        {
            return (_variables[name] as Parameter<T>).Value;
        }
    }
}
