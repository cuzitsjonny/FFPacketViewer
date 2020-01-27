using System;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Ethernet
{
    public class Frame : IBinaryUnserializable
    {
        private byte[] destination;
        private byte[] source;
        private ushort protocol;
        private Packet packet;

        public Frame()
        {
            this.destination = null;
            this.source = null;
            this.protocol = 0;
            this.packet = null;
        }

        public byte[] GetDestination()
        {
            return destination;
        }

        public string GetDestinationAsString()
        {
            return BitConverter.ToString(destination).Replace("-", ":");
        }

        public byte[] GetSource()
        {
            return source;
        }

        public string GetSourceAsString()
        {
            return BitConverter.ToString(source).Replace("-", ":");
        }

        public ushort GetProtocol()
        {
            return protocol;
        }

        public Packet GetPacket()
        {
            return packet;
        }

        public void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            destination = bitStream.ReadBytes(6);
            source = bitStream.ReadBytes(6);
            protocol = bitStream.ReadUInt16();
            packet = new Packet();
            packet.ReadFromBitStream(bitStream);
        }
    }
}
