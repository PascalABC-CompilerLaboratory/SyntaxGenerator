using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.TemplateNodes
{
    public class QualifiedIdentifier : Identifier
    {
        public Identifier Qualifier { get; set; }

        public QualifiedIdentifier() { }

        public QualifiedIdentifier(string qualifier, string identifier)
        {
            Qualifier = new Identifier(qualifier);
            Value = identifier;
        }

        public QualifiedIdentifier(Identifier qualifier, string identifier)
        {
            Qualifier = qualifier;
            Value = identifier;
        }
    }
}
