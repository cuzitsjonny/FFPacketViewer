using System;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Util
{
    class UInt11 : IBinaryUnserializable
    {
        private bool b1;
        private bool b2;
        private bool b3;
        private bool b4;
        private bool b5;
        private bool b6;
        private bool b7;
        private bool b8;
        private bool b9;
        private bool b10;
        private bool b11;

        public short GetValue()
        {
            short value = 0;

            if (b11)
            {
                value += 1;
            }

            if (b10)
            {
                value += 2;
            }

            if (b9)
            {
                value += 4;
            }

            if (b8)
            {
                value += 8;
            }

            if (b7)
            {
                value += 16;
            }

            if (b6)
            {
                value += 32;
            }

            if (b5)
            {
                value += 64;
            }

            if (b4)
            {
                value += 128;
            }

            if (b3)
            {
                value += 256;
            }

            if (b2)
            {
                value += 512;
            }

            if (b1)
            {
                value += 1024;
            }

            return value;
        }

        public void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            b1 = bitStream.ReadBit();
            b2 = bitStream.ReadBit();
            b3 = bitStream.ReadBit();
            b4 = bitStream.ReadBit();
            b5 = bitStream.ReadBit();
            b6 = bitStream.ReadBit();
            b7 = bitStream.ReadBit();
            b8 = bitStream.ReadBit();
            b9 = bitStream.ReadBit();
            b10 = bitStream.ReadBit();
            b11 = bitStream.ReadBit();
        }

        public override string ToString()
        {
            return GetValue().ToString();
        }
    }
}
