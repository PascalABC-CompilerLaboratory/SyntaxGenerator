using System;
using NodeGenerator;
using SyntaxConverter;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using SyntaxGenerator.SyntaxNodes.Model;
using System.Xml.Serialization;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string treePath = @"G:\MMCS\Programming\PABC.NET_GitRepo\SyntaxTree\tree\tree.nin";
            var generator = NodeGenerator.NodeGenerator.deserialize(treePath);
            var oldNodes = generator.all_nodes.Cast<node_info>();
            SyntaxTree tree = Converter.FromBinaryFormat(oldNodes);
            foreach (DictionaryEntry help in generator.help_storage.ht)
                if (help.Value != null && ((HelpContext)help.Value).help_context != "")
                    Console.WriteLine(help.Key + " = " + (help.Value as HelpContext).help_context);

            XmlSerializer serializer = new XmlSerializer(typeof(SyntaxTree));
            var xmlPath = @".\Tree.xml";
            //serializer.Serialize(File.OpenWrite(xmlPath), tree);
            tree = serializer.Deserialize(File.OpenRead(xmlPath)) as SyntaxTree;
            Console.ReadKey();
        }
    }
}
