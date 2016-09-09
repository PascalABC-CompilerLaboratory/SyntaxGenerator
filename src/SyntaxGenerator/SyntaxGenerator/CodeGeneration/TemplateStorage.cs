using SyntaxGenerator.TemplateNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.CodeGeneration
{
    public class TemplateStorage
    {
        private Dictionary<string, Template> _templates = new Dictionary<string, Template>();

        public void AddTemplate(string type, Template template)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (template == null)
                throw new ArgumentNullException(nameof(template));

            _templates[type] = template;
        }

        public Template this[string name]
        {
            get { return _templates[name]; }
        }
    }
}
