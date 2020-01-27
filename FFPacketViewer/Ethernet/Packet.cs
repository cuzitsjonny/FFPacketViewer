using System;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Ethernet
{
    public class Packet : IBinaryUnserializable
    {
        private byte version;           // 1
        private byte headerLength;      // 2
        private byte dsf;               // 3
        private ushort totalLength;     // 5
        private ushort identifier;      // 7
        private ushort flags;           // 9
        private byte ttl;               // 10
        private byte protocol;          // 11
        private ushort headerChecksum;  // 13
        private byte[] source;
        private byte[] destination;
        private Datagram datagram;

        public Packet()
        {
            this.version = 0;
            this.headerLength = 0;
            this.dsf = 0;
            this.totalLength = 0;
            this.identifier = 0;
            this.flags = 0;
            this.ttl = 0;
            this.protocol = 0;
            this.headerChecksum = 0;
            this.source = null;
            this.destination = null;
        }

        public byte GetVersion()
        {
            return version;
        }

        public byte GetHeaderLength()
        {
            return headerLength;
        }

        public byte GetDsf()
        {
            return dsf;
        }

        public ushort GetTotalLength()
        {
            return totalLength;
        }

        public ushort GetIdentifier()
        {
            return identifier;
        }

        public ushort GetFlags()
        {
            return flags;
        }

        public byte GetTtl()
        {
            return ttl;
        }

        public byte GetProtocol()
        {
            return protocol;
        }

        public ushort GetHeaderChecksum()
        {
            return headerChecksum;
        }

        public byte[] GetSource()
        {
            return source;
        }

        public string GetSourceAsString()
        {
            return source[0] + "." + source[1] + "." + source[2] + "." + source[3];
        }

        public byte[] GetDestination()
        {
            return destination;
        }

        public string GetDestinationAsString()
        {
            return destination[0] + "." + destination[1] + "." + destination[2] + "." + destination[3];
        }

        public Datagram GetDatagram()
        {
            return datagram;
        }

        public void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            bool isBigEndian = bitStream.IsBigEndian();

            bitStream.SetBigEndian(true);

            version = bitStream.ReadNibble();
            headerLength = bitStream.ReadNibble();
            headerLength *= 4;
            dsf = bitStream.ReadByte();
            totalLength = bitStream.ReadUInt16();
            identifier = bitStream.ReadUInt16();
            flags = bitStream.ReadUInt16();
            ttl = bitStream.ReadByte();
            protocol = bitStream.ReadByte();
            headerChecksum = bitStream.ReadUInt16();
            source = bitStream.ReadBytes(4);
            destination = bitStream.ReadBytes(4);

            bitStream.SetBigEndian(isBigEndian);

            if (protocol == 17)
            {
                datagram = new Datagram();
                datagram.ReadFromBitStream(bitStream);
            }
            else
            {
                bitStream.ReadBytes(totalLength - headerLength); // Skip if not UDP.
            }
        }
    }
}
