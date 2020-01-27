using System;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.TcpDump
{
    class Packet : IBinaryUnserializable
    {
        private PacketHeader header;
        private Ethernet.Frame data;

        public Packet()
        {
            this.header = null;
            this.data = null;
        }

        public PacketHeader GetHeader()
        {
            return header;
        }

        public Ethernet.Frame GetEthernetFrame()
        {
            return data;
        }

        public void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            header = new PacketHeader();
            header.ReadFromBitStream(bitStream);

            int position = bitStream.GetBitsRead() / 8;

            data = new Ethernet.Frame();
            data.ReadFromBitStream(bitStream);

            int dataSize = bitStream.GetBitsRead() / 8 - position;
            int difference = (int)header.GetLength() - dataSize;

            bitStream.ReadBytes(difference); // Skip padding.
        }
    }
}
