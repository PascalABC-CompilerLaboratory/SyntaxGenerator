using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.NodeBuilding.PartBuilders.TemplateBuilders
{
    public class TemplateVariableInfo : IEquatable<TemplateVariableInfo>
    {
        /// <summary>
        /// Имя переменной
        /// </summary>
        private string _name;
        /// <summary>
        /// Индекс переменной в шаблоне
        /// </summary>
        private int _index;
        /// <summary>
        /// Значение переменной
        /// </summary>
        private string _value;

        public TemplateVariableInfo(string name, int index = 0)
        {
            _name = name;
            _index = index;
            _value = null;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public bool Equals(TemplateVariableInfo other)
        {
            return other != null && _name == other._name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TemplateVariableInfo);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }
    }

    public class CodeTemplate
    {
        private const string VarBegin = "<$";
        private const string VarEnd = "$>";
        /// <summary>
        /// На эту строку будет заменена переменная с подстановкой соответствующего ей индекса
        /// </summary>
        private const string VarIndexFormatStr = "<{0}>";

        /// <summary>
        /// Ключ - имя переменной, значение - информация о ней
        /// </summary>
        private Dictionary<string, TemplateVariableInfo> _templateVariables;
        /// <summary>
        /// Текст шаблона
        /// </summary>
        private string _templateText;

        public CodeTemplate(string templateText)
        {
            _templateText = templateText;
            _templateVariables = new Dictionary<string, TemplateVariableInfo>();
            ExtractVariables();
        }

        /// <summary>
        /// Извлекает переменные из шаблона, заменяя их на индексы, и заполняет словарь <see cref="_templateVariables"/>
        /// </summary>
        private void ExtractVariables()
        {
            int prevVariablePosition = 0;
            int variablesCount = 0;

            while (true)
            {
                // находим следующее вхождение переменной
                var variableIndices = FindNextVariable(prevVariablePosition);
                // если больше переменных нет - выходим
                if (variableIndices == null)
                    break;

                int variableExprLength = variableIndices.Item2 - (variableIndices.Item1 + VarBegin.Length);
                // Извлекаем выражение, представляющее собой переменную
                string variableExpression = _templateText.Substring(variableIndices.Item1 + VarBegin.Length,
                                                                    variableExprLength);

                TemplateVariableInfo variable = new TemplateVariableInfo(variableExpression);

                // если такая переменная уже встречалась, то присваиваем ей готовый индекс
                // иначе добавляем переменную с новым индексом
                if (_templateVariables.ContainsKey(variable.Name))
                    variable.Index = _templateVariables[variable.Name].Index;
                else
                {
                    variable.Index = variablesCount++;
                    _templateVariables.Add(variable.Name, variable);
                }

                // заменяем описание переменной в шаблоне на форматную строку с ее индексом
                _templateText = _templateText.Remove(variableIndices.Item1, 
                                                     variableIndices.Item2 - variableIndices.Item1 + VarEnd.Length);
                _templateText = _templateText.Insert(variableIndices.Item1, string.Format(VarIndexFormatStr, variable.Index));
            }
        }

        /// <summary>
        /// Находит следующее вхождение переменной в <see cref="_templateText"/>
        /// </summary>
        /// <param name="startIndex">Позиция начала поиска</param>
        /// <returns>Индексы <see cref="VarBegin"/> и <see cref="VarEnd"/></returns>
        private Tuple<int, int> FindNextVariable(int startIndex)
        {
            int start = _templateText.IndexOf(VarBegin);
            return start == -1 ? null : new Tuple<int, int>(start, _templateText.IndexOf(VarEnd, start + VarBegin.Length));
        }

        /// <summary>
        /// Возвращает шаблон с подставленными в него значениями переменных
        /// </summary>
        /// <returns></returns>
        private string GetCode()
        {
            string result = _templateText;
            foreach (TemplateVariableInfo varInfo in _templateVariables.Values)
                result = result.Replace(string.Format(VarIndexFormatStr, varInfo.Index), 
                                        varInfo.Value == null ? string.Format("<!unresolved_name: {0}!>", varInfo.Name) : varInfo.Value);

            return result;
        }

        /// <summary>
        /// Устанавливает значение пременной
        /// </summary>
        /// <param name="varName">Имя переменной</param>
        /// <param name="value">Значение</param>
        public void SetValue(string varName, string value)
        {
            if (_templateVariables.ContainsKey(varName))
                _templateVariables[varName].Value = value;
        }

        public override string ToString()
        {
            return GetCode();
        }
    }
}
