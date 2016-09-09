using SyntaxGenerator.TemplateNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    public interface IFunctionTable
    {
        IEnumerable<string> FunctionNames { get; }

        IEnumerable<string> ConditionNames { get; }

        /// <summary>
        /// Вызывает функцию с указанными параметрами
        /// </summary>
        /// <param name="name">Имя функции</param>
        /// <param name="parameters">Параметры</param>
        /// <returns>Результат функции</returns>
        IEnumerable<string> CallFunction(string name, IFunctionParameters parameters = null);

        /// <summary>
        /// Возвращает значение условия
        /// </summary>
        /// <param name="name">Имя условия</param>
        /// <param name="parameters">Параметры</param>
        /// <returns></returns>
        bool CheckCondition(string name, IFunctionParameters parameters = null);

        /// <summary>
        /// Позволяет сохранить выражение
        /// </summary>
        /// <param name="name">Имя(alias)</param>
        /// <param name="expression">Выражение</param>
        void AddExpressionAlias(string name, IExpression expression);
    }
}
