using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    public class ConditionExpression : IEvaluatedExpression
    {
        public bool Value { get; private set; }

        public ConditionExpression(bool value)
        {
            Value = value;
        }
    }
}
