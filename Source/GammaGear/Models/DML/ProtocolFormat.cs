using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Windows.Data.Xml.Dom;
using Windows.Media.Audio;
using Windows.Storage;
using Windows.UI.WebUI;

namespace GammaGear.Models.DML
{
    public class ProtocolFormat
    {
        public DMLString Name { get; init; }
        public byte ServiceId { get; init; }
        public DMLString Type { get; init; }
        public int Version { get; init; }
        public DMLString Description { get; init; }
        public IReadOnlyList<MessageFormat> Messages { get; init; }

        public ProtocolFormat(byte serviceId, DMLString type, int version, DMLString description, MessageFormat[] messages)
        {
            ServiceId = serviceId;
            Type = type;
            Version = version;
            Description = description;
            Messages = messages;
        }

        public ProtocolFormat(string xmlFilePath)
        {
            StorageFile xmlFile = Task.Run(() => StorageFile.GetFileFromPathAsync(xmlFilePath)).GetAwaiter().GetResult().Get();
            //Task.Run(async () => {
            //    xmlFile = await StorageFile.GetFileFromPathAsync(xmlFilePath);
            //}).Wait();
            XmlDocument xmlDoc = Task.Run(() => XmlDocument.LoadFromFileAsync(xmlFile)).GetAwaiter().GetResult().Get();

            Name = xmlDoc.DocumentElement.TagName;

            string xpath = "/" + Name + "/_ProtocolInfo/RECORD";

            IXmlNode protoInfo = xmlDoc.DocumentElement.SelectSingleNode(xpath);

            if (protoInfo != null)
            {
                ServiceId = byte.Parse(protoInfo.SelectSingleNode("ServiceID").InnerText);
                Type = protoInfo.SelectSingleNode("ProtocolType").InnerText;
                Version = int.Parse(protoInfo.SelectSingleNode("ProtocolVersion").InnerText);
                Description = protoInfo.SelectSingleNode("ProtocolDescription").InnerText;
            }

            var messageNodeList = xmlDoc.DocumentElement.SelectNodes("*[position() > 1]/RECORD");

            List<MessageFormat> messages = new List<MessageFormat>();
            foreach (var messageNode in messageNodeList)
            {
                var mf = new MessageFormat(messageNode);
                mf.ProtocolFormat = this;
                messages.Add(mf);
            }

            Messages = messages;
        }

        public Message ParseMessage(byte[] data)
        {
            // TODO: Could use some error passing...
            Message message = null;

            // Check if magic is correct
            if (data[0] != 0x0d ||  data[1] != 0xf0)
            {
                return null;
            }

            // Get and verify length
            ushort length = BitConverter.ToUInt16(data, 2);

            if (length > data.Length)
            {
                // Might be a long message but I can't handle those yet.
                return null;
            }

            // Check if control message
            if (data[4] == 0x01)
            {
                return null; // Cannot handle system messages at this point.
            }

            // opcode and reserved space verification at some point...

            // Parse body header
            byte serviceID = data[8];
            byte order = data[9];
            ushort messageLength = (ushort)(BitConverter.ToUInt16(data, 10) - 4);
            var format = Messages.First(x => x.Order == order && x.ProtocolFormat.ServiceId == serviceID);
            message = new Message(format);
            ushort current = 12;

            foreach (var prop in format.Properties)
            {
                switch (prop.Type)
                {
                    case "STR":
                        ushort stringLength = BitConverter.ToUInt16(data, current);
                        message.SetPropertyByName(prop.Name, data.Take(new Range(current, current + stringLength + 2)).ToArray());
                        current += (ushort)(stringLength + 2);
                        break;
                    case "UBYT":
                        message.SetPropertyByName(prop.Name, new byte[] { data[current] });
                        current++;
                        break;
                    case "UINT":
                        message.SetPropertyByName(prop.Name, data.Take(new Range(current, current + 4)).ToArray());
                        current += 4;
                        break;
                    default:
                        break;
                }
            }

            return message;
        }
    }
}
