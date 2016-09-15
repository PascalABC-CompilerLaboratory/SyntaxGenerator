using SyntaxGenerator.SyntaxNodes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    public class SyntaxStreamFunctionTable : SyntaxFunctionTable
    {
        protected readonly string writerFieldName = "bw";
        protected readonly string readerFieldName = "br";

        protected HashSet<string> valueTypes = new HashSet<string>(
            new string[]
            {
                "Int32",
                "Int64",
                "UInt64",
                "bool",
                "char",
                "double"
            });

        protected Dictionary<string, string> valueTypeReadNames = new Dictionary<string, string>
        {
            { "Int32", "ReadInt32" },
            { "Int64", "ReadInt64" },
            { "UInt64", "ReadUInt64" },
            { "bool", "ReadBoolean" },
            { "char", "ReadChar" },
            { "double", "ReadDouble" }
        };

        public SyntaxStreamFunctionTable(SyntaxNodeInfo node) : base(node)
        {
            functions["WriteFields"] = WriteFields;
            functions["NodeNumber"] = NodeNumber;
            functions["WriteSyntaxTreeNode"] = WriteSyntaxTreeNode;
            functions["ReadSyntaxTreeNode"] = ReadSyntaxTreeNode;
            functions["WriterName"] = WriterName;
            functions["ReaderName"] = ReaderName;
            functions["ReadFields"] = ReadFields;
        }

        private IEnumerable<string> WriterName(IFunctionParameters p)
        {
            yield return writerFieldName;
        }

        private string WriteListCode(Field field)
        {
            var qualifiedFieldName = $"{node.Name}.{field.Name}";
            return 
$@"if (_{qualifiedFieldName} == null)
{{
    {writerFieldName}.Write((byte)0);
}}
else
{{
    {writerFieldName}.Write((byte)1);
    {writerFieldName}.Write(_{qualifiedFieldName}.Count);
    for (int ssyy_i = 0; ssyy_i < _{qualifiedFieldName}.Count; ssyy_i++)
    {{
        if (_{qualifiedFieldName}[ssyy_i] == null)
        {{
            {writerFieldName}.Write((byte)0);
        }}
        else
        {{
            {writerFieldName}.Write((byte)1);
            _{qualifiedFieldName}[ssyy_i].visit(this);
        }}
    }}
}}";
        }

        private string WriteReferenceTypeCode(Field field)
        {
            return
$@"
if (_{node.Name}.{field.Name} == null)
{{
    {writerFieldName}.Write((byte)0);
}}
else
{{
    {writerFieldName}.Write((byte)1);
    {writerFieldName}.Write(_{node.Name}.{field.Name});
}}";
        }

        private string WriteValueTypeCode(Field field)
        {
            return $@"{writerFieldName}.Write(_{node.Name}.{field.Name});";
        }

        private string WriteByteCode(Field field)
        {
            return $"{writerFieldName}.Write((byte)_{node.Name}.{field.Name});";
        }

        private string WriteSyntaxFieldCode(Field field)
        {
            return
$@"if (_{node.Name}.{field.Name} == null)
{{
      {writerFieldName}.Write((byte)0);
}}
else
{{
    {writerFieldName}.Write((byte)1);
    _{node.Name}.{field.Name}.visit(this);
}}";
        }

        private IEnumerable<string> WriteFields(IFunctionParameters parameters)
        {
            foreach (Field field in node.Fields)
            {
                if (IsSyntaxNode(field.Type))
                    yield return WriteSyntaxFieldCode(field);
                else
                if (field.IsList)
                    yield return WriteListCode(field);
                else
                if (field.Type == "string")
                    yield return WriteReferenceTypeCode(field);
                else
                if (valueTypes.Contains(field.Type))
                    yield return WriteValueTypeCode(field);
                else
                    yield return WriteByteCode(field);
            }
        }

        private IEnumerable<string> NodeNumber(IFunctionParameters parameters)
        {
            yield return node.SyntaxInfo.Nodes.IndexOf(node).ToString();
        }

        private IEnumerable<string> WriteSyntaxTreeNode(IFunctionParameters parameters)
        {
            yield return
$@"public void write_syntax_tree_node(syntax_tree_node _syntax_tree_node)
{{
    if (_syntax_tree_node.source_context == null)
    {{
        {writerFieldName}.Write((byte)0);
    }}
    else
    {{
        {writerFieldName}.Write((byte)1);
        if (_syntax_tree_node.source_context.begin_position == null)
        {{
            {writerFieldName}.Write((byte)0);
        }}
        else
        {{
            {writerFieldName}.Write((byte)1);
            {writerFieldName}.Write(_syntax_tree_node.source_context.begin_position.line_num);
            {writerFieldName}.Write(_syntax_tree_node.source_context.begin_position.column_num);
        }}
        if (_syntax_tree_node.source_context.end_position == null)
        {{
            {writerFieldName}.Write((byte)0);
        }}
        else
        {{
            {writerFieldName}.Write((byte)1);
            {writerFieldName}.Write(_syntax_tree_node.source_context.end_position.line_num);
            {writerFieldName}.Write(_syntax_tree_node.source_context.end_position.column_num);
        }}
    }}
}}";
        }

        private IEnumerable<string> ReaderName(IFunctionParameters p)
        {
            yield return readerFieldName;
        }

        private IEnumerable<string> ReadSyntaxTreeNode(IFunctionParameters p)
        {
            yield return
$@"public void read_syntax_tree_node(syntax_tree_node _syntax_tree_node)
{{
    if ({readerFieldName}.ReadByte() == 0)
    {{
        _syntax_tree_node.source_context = null;
    }}
    else
    {{
        SourceContext ssyy_beg = null;
        SourceContext ssyy_end = null;
        if ({readerFieldName}.ReadByte() == 1)
        {{
            ssyy_beg = new SourceContext({readerFieldName}.ReadInt32(), {readerFieldName}.ReadInt32(), 0, 0);
        }}
        if ({readerFieldName}.ReadByte() == 1)
        {{
            ssyy_end = new SourceContext(0, 0, {readerFieldName}.ReadInt32(), {readerFieldName}.ReadInt32());
        }}
        _syntax_tree_node.source_context = new SourceContext(ssyy_beg, ssyy_end);
    }}
}}";

        }

        private string ReadList(string fieldName, string fieldType, string elementType)
        {
            return
$@"if (br.ReadByte() == 0)
{{
    {fieldName} = null;
}}
else
{{
    {fieldName} = new {fieldType}();
    Int32 ssyy_count = br.ReadInt32();
    for(int ssyy_i = 0; ssyy_i < ssyy_count; ssyy_i++)
    {{
        {fieldName}.Add(_read_node() as {elementType});
    }}
}}";
        }

        private string ReadReferenceType(string fieldName)
        {
            return
$@"if ({readerFieldName}.ReadByte() == 0)
{{
    {fieldName} = null;
}}
else
{{
    {fieldName} = {readerFieldName}.ReadString();
}}";
        }

        private string ReadValueType(string fieldName, string fieldType)
        {
            return $"{fieldName} = {readerFieldName}.{valueTypeReadNames[fieldType]}();";
        }

        private string ReadByte(string fieldName, string fieldType)
        {
            return $"{fieldName} = ({fieldType}){readerFieldName}.ReadByte();";
        }

        private string ReadSyntaxField(string fieldName, string fieldType)
        {
            return $"{fieldName} = _read_node() as {fieldType};";
        }

        private IEnumerable<string> ReadFields(IFunctionParameters parameters)
        {
            foreach (Field field in node.Fields)
            {
                var fieldName = $"_{node.Name}.{field.Name}";
                if (IsSyntaxNode(field.Type))
                    yield return ReadSyntaxField(fieldName, field.Type);
                else
                if (field.IsList)
                    yield return ReadList(fieldName, field.Type, field.ListElementType);
                else
                if (field.Type == "string")
                    yield return ReadReferenceType(fieldName);
                else
                if (valueTypes.Contains(field.Type))
                    yield return ReadValueType(fieldName, field.Type);
                else
                    yield return ReadByte(fieldName, field.Type);
            }
        }
    }
}
