using ABI.System.Collections;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Bluetooth.Advertisement;
using static System.Windows.Forms.DataFormats;

namespace GammaGear.Models.DML
{
    public class Message : Object
    {
        public MessageFormat Format { get; init; }
        public byte[][] Data { get; private set; }

        public Message(MessageFormat Format)
        {
            this.Format = Format;
            Data = new byte[Format.Properties.Count][];
        }

        /// <summary>
        /// Sets the named property to passed data. Returns true if the property exists on the message, false if not.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SetPropertyByName(string name, byte[] data)
        {
            for (int i = 0; i < Format.Properties.Count; i++)
            {
                if (Format.Properties[i].Name == name)
                {
                    Data[i] = new byte[data.Length];
                    data.CopyTo(Data[i], 0);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get the value of the named property.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public byte[] GetPropertyByName(string name)
        {
            for (int i = 0; i < Format.Properties.Count; i++)
            {
                if (Format.Properties[i].Name == name)
                {
                    return Data[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the network-compatible byte structure.
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>
            {
                // Frame
                0x0d, 0xf0, // Magic
                0x00, 0x00, // Will be set later to the whole length of the body

                // Header
                0x00,       // Is Control?
                0x00,       // opcode
                0x00, 0x00, // reserved

                // Data Message Header
                Format.ProtocolFormat.ServiceId,
                Format.Order,
                0x00, 0x00, // Length, to be set later
                // Message Body
            };

            // Data Message Body
            foreach (var datum in Data)
            {
                bytes.AddRange(datum);
            }

            // Set framing length
            ushort frameLength = (ushort)(bytes.Count - 4);

            // set data message length
            ushort dataLength = (ushort)(frameLength - 4);

            //frameLength = BinaryPrimitives.ReverseEndianness(frameLength);
            //dataLength = BinaryPrimitives.ReverseEndianness(dataLength);

            byte[] frameBytes = BitConverter.GetBytes(frameLength);
            byte[] dataBytes = BitConverter.GetBytes(dataLength);

            bytes[2] = frameBytes[0];
            bytes[3] = frameBytes[1];
            bytes[10] = dataBytes[0];
            bytes[11] = dataBytes[1];

            return bytes.ToArray();
        }

        public override string ToString()
        {
            string r = $"{Format}\n";
            for (int i = 0; i < Format.Properties.Count; i++)
            {
                if (Data[i] != null && Data[i].Length > 0)
                {
                    var prop = Format.Properties[i];
                    switch (prop.Type)
                    {
                        case "STR":
                            ushort stringLength = BitConverter.ToUInt16(Data[i]);
                            r += $"{prop}: \"{new DMLString(Data[i])}\"\n";
                            break;
                        case "UBYT":
                            r += $"{prop}: \"{Data[i][0]}\"\n";
                            break;
                        case "UINT":
                            r += $"{prop}: \"{BitConverter.ToUInt32(Data[i])}\"\n";
                            break;
                        default:
                            r += $"{prop}: cannot display\n";
                            break;
                    }
                }
            }
            return r;
        }
    }
}
