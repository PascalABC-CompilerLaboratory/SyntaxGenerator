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
        public void ReadString()
        {
            var input = @"foo""bar{""a""}bar2{b25}\{ \}""42";

            var parser = new Parser(input)
                .ReadWhile(c => c != '"')
                .ReadInterpolatedString(s =>
                {
                    Assert.AreEqual("bar{0}bar2{1}{ }", s.Format);
                    Assert.AreEqual("a", (s.Arguments[0] as FormatString).Format);
                    Assert.AreEqual("b25", (s.Arguments[1] as Identifier).Value);
                })
                .ReadString("42");
            Assert.IsTrue(parser.IsSucceed);
        }

        [TestMethod]
        public void ReadQualifiedIdentifier()
        {
            var input = "foo.bar";
            var parser = new Parser(input)
                .ReadQualifiedIdentifier(
                id =>
                {
                    Assert.AreEqual("foo", id.Qualifier.Value);
                    Assert.AreEqual("bar", id.Value);
                });

            Assert.IsTrue(parser.IsSucceed);
        }

        [TestMethod]
        public void ReadAssignment()
        {
            var input = "name = \"foo\"";
            var parser = new Parser(input)
                .ReadAssignment(
                assignment =>
                {
                    Assert.AreEqual("name", assignment.VariableName.Value);
                    Assert.AreEqual("foo", (assignment.Value as FormatString).Format);
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
                    Assert.AreEqual("name", setStatement.VariableName.Value);
                    Assert.AreEqual("foo", (setStatement.Value as FormatString).Format);
                });

            Assert.IsTrue(parser.IsSucceed);
        }

        [TestMethod]
        public void ReadTemplate()
        {
            var input =
@"<#set FileType = SimpleSyntaxNode#>
public <#NodeName#>(<#""{ Field.Type } _{ Field.Name }""#>)
{
    <#ConstructGen separator ""\n""#>
}";
            var parser = new Parser(input).ReadTemplate(
                template =>
                {
                    var parts = template.Parts;
                    var settings = parts[0] as SetStatement;
                    Assert.AreEqual("FileType", settings.VariableName.Value);
                    Assert.AreEqual("SimpleSyntaxNode", (settings.Value as Identifier).Value);

                    Assert.AreEqual("public ", (parts[1] as CSharpCode).Code);

                    var nodeName = (parts[2] as Identifier).Value;
                    Assert.AreEqual("NodeName", nodeName);

                    Assert.AreEqual("(", (parts[3] as CSharpCode).Code);

                    var format = (parts[4] as FormatString);
                    var parameters = (parts[4] as FormatString).Arguments;
                    Assert.AreEqual("{0} _{1}", format.Format);
                    var identifier = format.Arguments[0] as QualifiedIdentifier;
                    Assert.AreEqual("Field", identifier.Qualifier.Value);
                    Assert.AreEqual("Type", identifier.Value);
                    identifier = format.Arguments[1] as QualifiedIdentifier;
                    Assert.AreEqual("Field", identifier.Qualifier.Value);
                    Assert.AreEqual("Name", identifier.Value);

                    Assert.AreEqual(")\r\n{\r\n    ", (parts[5] as CSharpCode).Code);

                    var funcCall = parts[6] as FunctionCall;
                    var firstParam = funcCall.Parameters[0] as Parameter<string>;
                    Assert.AreEqual("ConstructGen", funcCall.Name);
                    Assert.AreEqual("separator", firstParam.Name);
                    Assert.AreEqual(@"\n", firstParam.Value);

                    Assert.AreEqual("\r\n}", (parts[7] as CSharpCode).Code);
                });

            Assert.IsTrue(parser.IsSucceed);
        }
    }
}
