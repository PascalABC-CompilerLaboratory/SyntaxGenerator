using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    public interface IFunctionParameters
    {
        /// <summary>
        /// Возвращает значение параметра по имени
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T GetValue<T>(string name);
    }
}
