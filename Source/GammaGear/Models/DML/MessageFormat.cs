using ABI.System.Collections;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace GammaGear.Models.DML
{
    public class MessageFormat : Object
    {
        public ProtocolFormat ProtocolFormat { get; set; }
        public byte Order;
        public DMLString Name;
        public DMLString Description;
        public DMLString Handler;
        public byte AccessLevel;
        public IReadOnlyList<PropertyFormat> Properties;

        public MessageFormat(IXmlNode xmlNode)
        {
            Order = byte.Parse(xmlNode.SelectSingleNode("_MsgOrder").InnerText);
            Name = xmlNode.ParentNode.NodeName;
            Description = xmlNode.SelectSingleNode("_MsgDescription").InnerText;
            Handler = xmlNode.SelectSingleNode("_MsgHandler").InnerText;
            AccessLevel = byte.Parse(xmlNode.SelectSingleNode("_MsgAccessLvl").InnerText);

            List<PropertyFormat> properties = new List<PropertyFormat>();

            var propertyNodes = xmlNode.SelectNodes("*[not(starts-with(name(), \"_\"))]");

            foreach (var propertyNode in propertyNodes)
            {
                string type = propertyNode.Attributes.GetNamedItem("TYPE").InnerText;
                string name = propertyNode.NodeName;
                properties.Add(new PropertyFormat(type, name));
            }

            Properties = properties;
        }

        public override string ToString()
        {
            return $"{Name}<{Order}>";
        }
    }
}
