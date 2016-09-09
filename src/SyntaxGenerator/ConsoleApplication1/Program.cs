using System;
using NodeGenerator;
using SyntaxConverter;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using SyntaxGenerator.SyntaxNodes.Model;
using System.Xml.Serialization;
using System.IO;
using SyntaxGenerator.CodeGeneration;
using SyntaxGenerator.Reading;

namespace ConsoleApplication1
{
    class Program
    {
        static void Generate()
        {
            var xmlPath = @".\Tree.xml";
            var outputPath = @".\Tree.cs";
            var templatesPath = @".\Templates";
            SyntaxTree tree = SyntaxTree.Deserialize(xmlPath);
            var templateStorage = TemplateStorageBuilder.LoadFromFolder(new DirectoryInfo(templatesPath));
            SyntaxTreeGenerator generator = new SyntaxTreeGenerator(templateStorage);
            File.WriteAllText(outputPath, generator.Generate(tree));
        }

        static void Convert()
        {
            var xmlPath = @".\Tree.xml";
            string treePath = @"G:\MMCS\Programming\PABC.NET_GitRepo\SyntaxTree\tree\tree.nin";
            var generator = NodeGenerator.NodeGenerator.deserialize(treePath);
            var oldNodes = generator.all_nodes.Cast<node_info>();
            SyntaxTree tree = Converter.FromBinaryFormat(oldNodes);

            //foreach (DictionaryEntry help in generator.help_storage.ht)
            //    if (help.Value != null && ((HelpContext)help.Value).help_context != "")
            //        Console.WriteLine(help.Key + " = " + (help.Value as HelpContext).help_context);

            XmlSerializer serializer = new XmlSerializer(typeof(SyntaxTree));
            serializer.Serialize(File.OpenWrite(xmlPath), tree);
        }

        static void Main(string[] args)
        {
            Generate();
        }
    }
}
