using System;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Ethernet
{
    public class Datagram : IBinaryUnserializable
    {
        private ushort sourcePort;      // 2
        private ushort destinationPort; // 4
        private ushort totalLength;     // 6
        private ushort checksum;        // 8
        private byte[] data;

        public Datagram()
        {
            this.sourcePort = 0;
            this.destinationPort = 0;
            this.totalLength = 0;
            this.checksum = 0;
            this.data = null;
        }

        public ushort GetSourcePort()
        {
            return sourcePort;
        }

        public ushort GetDestinationPort()
        {
            return destinationPort;
        }

        public ushort GetTotalLength()
        {
            return totalLength;
        }

        public ushort GetChecksum()
        {
            return checksum;
        }

        public byte[] GetData()
        {
            return data;
        }

        public void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            bool isBigEndian = bitStream.IsBigEndian();

            bitStream.SetBigEndian(true);

            sourcePort = bitStream.ReadUInt16();
            destinationPort = bitStream.ReadUInt16();
            totalLength = bitStream.ReadUInt16();
            checksum = bitStream.ReadUInt16();
            data = bitStream.ReadBytes(totalLength - 8);

            bitStream.SetBigEndian(isBigEndian);
        }
    }
}
