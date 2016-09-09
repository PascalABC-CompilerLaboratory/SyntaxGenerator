using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxGenerator.TemplateNodes;

namespace SyntaxGenerator.CodeGeneration
{
    public abstract class AbstractSyntaxFunctionTable : IFunctionTable
    {
        /// <summary>
        /// Информация о текущем узле
        /// </summary>
        protected SyntaxNodeInfo node;

        /// <summary>
        /// Сопоставляет имя функции из текста шаблона делегату функции
        /// </summary>
        protected Dictionary<string, Func<IFunctionParameters, IEnumerable<string>>> functions = 
            new Dictionary<string, Func<IFunctionParameters, IEnumerable<string>>>();

        protected Dictionary<string, Func<IFunctionParameters, bool>> conditions =
            new Dictionary<string, Func<IFunctionParameters, bool>>();

        /// <summary>
        /// Вычисленные aliased-выражения
        /// </summary>
        private Dictionary<string, IEvaluatedExpression> _cachedExpressions =
            new Dictionary<string, IEvaluatedExpression>();

        public IEnumerable<string> FunctionNames
        {
            get
            {
                return 
                    functions.Keys.Concat(
                    _cachedExpressions
                    .Where(pair => pair.Value is TextExpression)
                    .Select(pair => pair.Key));
            }
        }

        public IEnumerable<string> ConditionNames
        {
            get
            {
                return
                    conditions.Keys.Concat(
                    _cachedExpressions
                    .Where(pair => pair.Value is ConditionExpression)
                    .Select(pair => pair.Key));
            }
        }

        public AbstractSyntaxFunctionTable(SyntaxNodeInfo node)
        {
            this.node = node;
        }

        public virtual IEnumerable<string> CallFunction(string name, IFunctionParameters parameters)
        {
            if (_cachedExpressions.ContainsKey(name))
                return (_cachedExpressions[name] as TextExpression).Values;
            else
                return functions[name](parameters);
        }

        public virtual bool CheckCondition(string name, IFunctionParameters parameters)
        {
            if (_cachedExpressions.ContainsKey(name))
                return (_cachedExpressions[name] as ConditionExpression).Value;
            else
                return conditions[name](parameters);
        }

        public void AddExpressionAlias(string name, IExpression expression)
        {
            _cachedExpressions[name] = expression.Evaluate(this);
        }
    }
}
