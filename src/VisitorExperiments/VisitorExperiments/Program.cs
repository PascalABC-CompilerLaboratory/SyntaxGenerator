using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PascalABCCompiler.PascalABCNewParser;
using PascalABCCompiler.SyntaxTree;
using PascalABCSavParser;
using SyntaxVisitors;

namespace VisitorExperiments
{
    class Program
    {
        static void Main(string[] args)
        {
            HashSet<string> s = new HashSet<string>();
            s.Add(null);
            string program =
@"
begin
  var a: List<integer> = new List<integer>();
  a.RemoveAll(x -> begin Assert(false); result := false end);
end.
";
            PascalABCNewLanguageParser parser = new PascalABCNewLanguageParser();
            syntax_tree_node root = parser.BuildTreeInNormalMode("NoName", program);

            //Console.WriteLine("Original tree: ");
            //SimplePrettyPrinterVisitor prettyPrinter = new SimplePrettyPrinterVisitor();
            //root.visit(prettyPrinter);

            AssertDeleter deleteVisitor = new AssertDeleter();
            root.visit(deleteVisitor);

            Console.WriteLine("---------------------");
            //Console.WriteLine("Visitor result: ");
            //root.visit(prettyPrinter);

            Console.ReadKey();
        }

        static void GroupNodesTest()
        {
            string program =
            @"procedure A(b: char);
            begin
            end;

            procedure A(b: string);
            begin
            end;

            procedure C(c: string);
            begin
            end;

            procedure B(a: char);
            begin
            end;

            procedure A(b: integer);
            begin
            end;

            procedure B(a: integer);
            begin
            end;

            begin
            end.";

            PascalABCNewLanguageParser parser = new PascalABCNewLanguageParser();
            syntax_tree_node root = parser.BuildTreeInNormalMode("NoName", program);

            GroupNodesVisitor groupVisitor = new GroupNodesVisitor();
            root.visit(groupVisitor);

            SimplePrettyPrinterVisitor prettyPrinter = new SimplePrettyPrinterVisitor();
            root.visit(prettyPrinter);
        }
    }
}
