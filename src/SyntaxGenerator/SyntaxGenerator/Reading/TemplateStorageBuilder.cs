using SyntaxGenerator.CodeGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxGenerator.Reading
{
    public class TemplateStorageBuilder
    {
        public static TemplateStorage LoadFromFolder(DirectoryInfo directory)
        {
            return 
                new TemplateReader()
                .ReadTemplates(directory.EnumerateFiles("*.t")
                .Select(file => File.ReadAllText(file.FullName)));
        }
    }
}
