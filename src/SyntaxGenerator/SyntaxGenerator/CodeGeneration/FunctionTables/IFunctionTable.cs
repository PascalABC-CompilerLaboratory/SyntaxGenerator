using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    public interface IFunctionTable
    {
        /// <summary>
        /// Вызывает функцию с указанными параметрами
        /// </summary>
        /// <param name="name">Имя функции</param>
        /// <param name="parameters">параметры</param>
        /// <returns>Резулитат функции</returns>
        IEnumerable<string> CallFunction(string name, IFunctionParameters parameters = null);
    }
}
