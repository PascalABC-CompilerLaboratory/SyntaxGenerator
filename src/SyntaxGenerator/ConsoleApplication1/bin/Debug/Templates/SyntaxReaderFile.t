<# set TemplateType = "SyntaxReaderFile" #>

using System;
using System.IO;
using System.Collections.Generic;

namespace PascalABCCompiler.SyntaxTree
{
    public class SyntaxTreeStreamReader : IVisitor
    {
        public BinaryReader br;
        
        public syntax_tree_node _construct_node(Int16 node_class_number)
		{
			switch(node_class_number)
			{
                <#"case {IntSequence}:\n    return new {NodeName}();" join "\n"#>
            }
            return null;
        }
        
        public syntax_tree_node _read_node()
        {
            if (br.ReadByte() == 1)
            {
                syntax_tree_node ssyy_tmp = _construct_node(br.ReadInt16());
                ssyy_tmp.visit(this);
                return ssyy_tmp;
            }
            else
            {
                return null;
            }
        }
        
        <#ReaderNodesCode join "\n"#>
    }
}