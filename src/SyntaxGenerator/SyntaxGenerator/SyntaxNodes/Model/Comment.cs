﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SyntaxGenerator.SyntaxNodes.Model
{
    public class Comment
    {
        [XmlAnyElement]
        public XmlElement[] Body;
    }
}
