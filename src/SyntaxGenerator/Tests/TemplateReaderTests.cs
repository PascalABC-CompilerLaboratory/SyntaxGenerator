using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxGenerator.Reading;
using TemplateParser;
using TemplateParser.Extensions;
using static TemplateParser.ParserHelper;
using SyntaxGenerator.TemplateNodes;

namespace Tests
{
    [TestClass]
    public class TemplateReaderTests
    {
        [TestMethod]
        public void ReadInterpolatedString()
        {
            var input = @"foo""bar{""a""}bar2{b25}\{ \}""42";

            var parser = new Parser(input)
                .ReadWhile(c => c != '"')
                .ReadInterpolatedString(s =>
                {
                    Assert.AreEqual("bar{0}bar2{1}{ }", s.Format);
                    Assert.AreEqual("a", (s.Arguments[0] as FormatString).Format);
                    Assert.AreEqual("b25", (s.Arguments[1] as FunctionCall).Name);
                })
                .ReadString("42");
            Assert.IsTrue(parser.IsSucceed);
        }

        [TestMethod]
        public void ReadAssignment()
        {
            var input = @"name = foo join ""\n""";
            var parser = new Parser(input)
                .ReadAssignment(
                assignment =>
                {
                    Assert.AreEqual("name", assignment.VariableName);
                    Assert.AreEqual("foo", (assignment.Value as FunctionCall).Name);
                    Assert.AreEqual(Environment.NewLine, assignment.Value.Separator);
                });

            Assert.IsTrue(parser.IsSucceed);
        }

        [TestMethod]
        public void ReadSetStatement()
        {
            var input = "set name = \"foo\"";
            var parser = new Parser(input)
                .ReadSetStatement(
                setStatement =>
                {
                    Assert.AreEqual("name", setStatement.VariableName);
                    Assert.AreEqual("foo", (setStatement.Value as FormatString).Format);
                });

            Assert.IsTrue(parser.IsSucceed);
        }

        [TestMethod]
        public void ReadTemplate()
        {
            var input =
@"<#set FileType = SimpleSyntaxNode#>
public <#NodeName#>(<#""{ FieldType() } _{ FieldName( ) }""#>)
{
    <#ConstructGen(separator: ""\n"")#>
}";
            var parser = new Parser(input).ReadTemplate(
                template =>
                {
                    var parts = template.Parts;
                    var settings = parts[0] as SetStatement;
                    Assert.AreEqual("FileType", settings.VariableName);
                    Assert.AreEqual("SimpleSyntaxNode", (settings.Value as FunctionCall).Name);

                    Assert.AreEqual("public ", (parts[1] as CSharpCode).Code);

                    var nodeName = (parts[2] as FunctionCall).Name;
                    Assert.AreEqual("NodeName", nodeName);

                    Assert.AreEqual("(", (parts[3] as CSharpCode).Code);

                    var format = (parts[4] as FormatString);
                    var parameters = (parts[4] as FormatString).Arguments;
                    Assert.AreEqual("{0} _{1}", format.Format);
                    var identifier = (format.Arguments[0] as FunctionCall).Name;
                    Assert.AreEqual("FieldType", identifier);
                    identifier = (format.Arguments[1] as FunctionCall).Name;
                    Assert.AreEqual("FieldName", identifier);

                    Assert.AreEqual(")\r\n{\r\n    ", (parts[5] as CSharpCode).Code);

                    var funcCall = parts[6] as FunctionCall;
                    var firstParam = funcCall.Parameters[0] as Parameter<string>;
                    Assert.AreEqual("ConstructGen", funcCall.Name);
                    Assert.AreEqual("separator", firstParam.Name);
                    Assert.AreEqual(Environment.NewLine, firstParam.Value);

                    Assert.AreEqual("\r\n}", (parts[7] as CSharpCode).Code);
                });

            Assert.IsTrue(parser.IsSucceed);
        }
    }
}
