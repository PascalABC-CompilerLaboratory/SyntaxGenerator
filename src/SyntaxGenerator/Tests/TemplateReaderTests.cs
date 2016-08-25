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
                .ReadString(s =>
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
        public void ReadJoin()
        {
            var input = "join \"\n\"";
            var parser = new Parser(input)
                .ReadJoinParameter(
                separator =>
                {
                    Assert.IsInstanceOfType(separator.Value, typeof(FormatString));
                    Assert.AreEqual("\n", (separator.Value as FormatString).Format);
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
public <#NodeName#>(<#""{ Field.Type } _{ Field.Name }"" join "", ""#>)
{
}";
            var parser = new Parser(input).ReadTemplate(
                template =>
                {
                    var parts = template.Parts;
                    var settings = parts[0] as SetStatement;
                    Assert.AreEqual("FileType", settings.VariableName.Value);
                    Assert.AreEqual("SimpleSyntaxNode", (settings.Value as Identifier).Value);

                    Assert.AreEqual("public ", (parts[1] as CSharpCode).Code);

                    var nodeName = ((parts[2] as ParameterizedExpression).Expression as Identifier).Value;
                    Assert.AreEqual("NodeName", nodeName);

                    Assert.AreEqual("(", (parts[3] as CSharpCode).Code);

                    var core = (parts[4] as ParameterizedExpression).Expression as FormatString;
                    var parameters = (parts[4] as ParameterizedExpression).Parameters;
                    Assert.AreEqual("{0} _{1}", core.Format);
                    var identifier = core.Arguments[0] as QualifiedIdentifier;
                    Assert.AreEqual("Field", identifier.Qualifier.Value);
                    Assert.AreEqual("Type", identifier.Value);
                    identifier = core.Arguments[1] as QualifiedIdentifier;
                    Assert.AreEqual("Field", identifier.Qualifier.Value);
                    Assert.AreEqual("Name", identifier.Value);
                    Assert.AreEqual(", ", ((parameters[0] as Separator).Value as FormatString).Format);

                    Assert.AreEqual(")\r\n{\r\n}", (parts[5] as CSharpCode).Code);
                });

            Assert.IsTrue(parser.IsSucceed);
        }
    }
}
