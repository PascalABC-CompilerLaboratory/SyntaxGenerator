using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorExperiments.Utility
{
    public class Namespace : IEquatable<Namespace>
    {
        Namespace parentNamespace;
        string name;
        string fullName;

        public Namespace(string name)
        {
            parentNamespace = null;
            fullName = null;
            this.name = name;
        }

        public Namespace(Namespace parentNamespace, string name)
        {
            this.parentNamespace = parentNamespace;
            fullName = null;
            this.name = name;
        }

        public Namespace ParentNamespace { get { return parentNamespace; } }

        public string Name { get { return name; } }

        public string FullName
        {
            get
            {
                if (fullName == null)
                    if (parentNamespace == null)
                        fullName = name;
                    else
                        fullName = parentNamespace.FullName + '.' + name;

                return fullName;
            }
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is Namespace)
                return Equals(obj as Namespace);
            else
                return false;
        }

        public bool Equals(Namespace other)
        {
            if (other != null)
                return this.FullName == other.FullName;
            else
                return false;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
