using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using SyntaxGenerator.NodeBuilding.PartBuilders.TemplateBuilders;
using SyntaxGenerator.NodeBuilding;
using SyntaxGenerator.NodeInfo;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //StringBuilder someImpl = new StringBuilder();
            //someImpl.AppendLine("<$implementation ISyntaxIndex$>");
            //someImpl.AppendLine("public STN this[int index]");
            //someImpl.AppendLine("{");
            //someImpl.AppendLine("    get");
            //someImpl.AppendLine("    {");
            //someImpl.AppendLine("        return <$listName$>[index]");
            //someImpl.AppendLine("    }");
            //someImpl.AppendLine("}");
            //someImpl.AppendLine("<$listName$>");
            //someImpl.AppendLine("<$listNames$>");

            string constructorTemplatePath = Environment.CurrentDirectory + "/../../../BuilderTemplates/constructor.template";
            string[] constructorTemplateText = File.ReadAllLines(constructorTemplatePath);

            SyntaxNodeInfo identNodeInfo = new SyntaxNodeInfo("ident", "addressed_value_funcname");
            identNodeInfo.AddField(new FieldInfo("protected", "string", "name"));

            ConstructorBuilder constructorBuilder = new ConstructorBuilder(identNodeInfo, 4, constructorTemplateText);

            PartNodeBuilder nodeBuilder = new PartNodeBuilder(identNodeInfo);
            nodeBuilder.AddBuilder(constructorBuilder);
            StreamWriter consoleWriter = new StreamWriter(Console.OpenStandardOutput());
            nodeBuilder.AppendNode(consoleWriter);
            consoleWriter.Flush();

            //Console.Write(typeof(List<int>).IsValueType + " " + Type.GetType("System.Collections.Generic.List").ToString());
            Console.ReadKey();
        }
    }
}
