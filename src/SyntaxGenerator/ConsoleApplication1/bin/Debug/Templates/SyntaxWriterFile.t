<# set TemplateType = "SyntaxWriterFile" #>

using System;
using System.IO;

namespace PascalABCCompiler.SyntaxTree
{
    public class SyntaxTreeStreamWriter : IVisitor
    {
        public BinaryWriter bw;
        
        <#WriterNodesCode join "\n"#>
    }
}