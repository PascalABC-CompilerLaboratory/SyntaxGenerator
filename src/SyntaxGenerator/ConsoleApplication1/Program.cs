using System;
using NodeGenerator;
using SyntaxConverter;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string treePath = @"G:\MMCS\Programming\PABC.NET_GitRepo\SyntaxTree\tree\tree.nin";
            var generator = NodeGenerator.NodeGenerator.deserialize(treePath);
            Converter.FromBinaryFormat(generator.all_nodes.Cast<node_info>());

            Console.ReadKey();
        }
    }
}
