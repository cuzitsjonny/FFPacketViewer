using System;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.TcpDump
{
    class PacketHeader : IBinaryUnserializable
    {
        private uint timestampSeconds;      // 4
        private uint timestampMicroseconds; // 8
        private uint length;                // 16
        private uint originalLength;        // 32

        public PacketHeader()
        {
            this.timestampSeconds = 0;
            this.timestampMicroseconds = 0;
            this.length = 0;
            this.originalLength = 0;
        }

        public uint GetTimestampSeconds()
        {
            return timestampSeconds;
        }

        public uint GetTimestampMicroseconds()
        {
            return timestampMicroseconds;
        }

        public uint GetLength()
        {
            return length;
        }

        public uint GetOriginalLength()
        {
            return originalLength;
        }

        public void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            timestampSeconds = bitStream.ReadUInt32();
            timestampMicroseconds = bitStream.ReadUInt32();
            length = bitStream.ReadUInt32();
            originalLength = bitStream.ReadUInt32();
        }
    }
}
