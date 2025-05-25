using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;

namespace GammaGear.Models.DML
{
    public class DMLString : object
    {
        public ushort Length = 0;
        public byte[] Data = null;

        public DMLString(byte[] data)
        {
            Data = data.Take(Range.StartAt(2)).ToArray();
            Length = (ushort)Data.Length;
        }

        public DMLString(string data)
        {
            Data = Encoding.ASCII.GetBytes(data);
            Length = (ushort)data.Length;
        }

        public byte[] ToBytes()
        {
            //var l = BitConverter.GetBytes(BinaryPrimitives.ReverseEndianness(Length));
            var l = BitConverter.GetBytes(Length);
            return l.Concat(Data).ToArray();
        }

        public override string ToString()
        {
            if (Length == 0 || Data == null)
            {
                return string.Empty;
            }

            string r = "";
            for (int i = 0; i < Length; i++)
            {
                r += (char)Data[i];
            }

            return r;
        }

        public static implicit operator DMLString(string s)
        {
            return new DMLString(s);
        }

        public static implicit operator string(DMLString s)
        {
            return s.ToString();
        }
    }
}
