using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SyntaxGenerator.NodeInfo;

namespace SyntaxGenerator.NodeBuilding.PartBuilders.TemplateBuilders
{
    public class ConstructorBuilder : AbstractPartBuilder
    {
        /// <summary>
        /// Построчный текст шаблона
        /// </summary>
        private string[] _constructorTemplate;
        private string _constructorParams;
        private string _constructorFieldsInit;

        public ConstructorBuilder(SyntaxNodeInfo nodeInfo, int indent, string[] template) : 
            base (nodeInfo, indent)
        {
            _constructorTemplate = template;
            InitVariables();
        }

        public override void AppendCode(StreamWriter writer)
        {
            CodeTemplate indentedTemplate = CreateTemplateWithIndent();
            // устанавливаем значения переменных для шаблона
            indentedTemplate.SetValue("NodeName", nodeInfo.NodeName);
            indentedTemplate.SetValue("ConstructorFieldsParams", _constructorParams);
            indentedTemplate.SetValue("ConstructorFieldsInit", _constructorFieldsInit);

            writer.Write(indentedTemplate.ToString());
        }

        private void InitVariables()
        {
            // построение параметров конструктора
            _constructorParams = "";
            List<string> namesWithTypes = new List<string>();
            foreach (FieldInfo fieldInfo in nodeInfo.Fields)
                namesWithTypes.Add(fieldInfo.Type + " " + fieldInfo.Name);
            _constructorParams = string.Join(", ", namesWithTypes);

            // построение инициализации полей класса
            _constructorFieldsInit = "";
            List<string> fieldInitStrings = new List<string>();
            foreach (FieldInfo fieldInfo in nodeInfo.Fields)
                fieldInitStrings.Add(string.Format("this.{0} = _{1};", fieldInfo.Name, fieldInfo.Name));
            _constructorFieldsInit = string.Join(Environment.NewLine, fieldInitStrings);

        }

        /// <summary>
        /// Создает экземпляр шаблона, учитывая сдвиг
        /// </summary>
        /// <returns></returns>
        private CodeTemplate CreateTemplateWithIndent()
        {
            StringBuilder templateStringBuilder = new StringBuilder();
            string indentString = new string(' ', indent);
            foreach (string s in _constructorTemplate)
            {
                templateStringBuilder.Append(indentString);
                templateStringBuilder.AppendLine(s);
            }

            return new CodeTemplate(templateStringBuilder.ToString());
        }
    }
}
