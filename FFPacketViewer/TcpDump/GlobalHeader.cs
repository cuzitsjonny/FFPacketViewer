using System;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.TcpDump
{
    class GlobalHeader : IBinaryUnserializable
    {
        private uint magicNumber;       // 4
        private ushort versionMajor;    // 6
        private ushort versionMinor;    // 8
        private int timeZone;           // 12
        private uint timestampAccuracy; // 16
        private uint snapshotLength;    // 20
        private uint linkType;          // 24

        public GlobalHeader()
        {
            this.magicNumber = 0;
            this.versionMajor = 0;
            this.versionMinor = 0;
            this.timeZone = 0;
            this.timestampAccuracy = 0;
            this.snapshotLength = 0;
            this.linkType = 0;
        }

        public uint GetMagicNumber()
        {
            return magicNumber;
        }

        public ushort GetVersionMajor()
        {
            return versionMajor;
        }

        public ushort GetVersionMinor()
        {
            return versionMinor;
        }

        public int GetTimeZone()
        {
            return timeZone;
        }

        public uint GetTimestampAccuracy()
        {
            return timestampAccuracy;
        }

        public uint GetSnapshotLength()
        {
            return snapshotLength;
        }

        public uint GetLinkType()
        {
            return linkType;
        }

        public void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            magicNumber = bitStream.ReadUInt32();
            versionMajor = bitStream.ReadUInt16();
            versionMinor = bitStream.ReadUInt16();
            timeZone = bitStream.ReadInt32();
            timestampAccuracy = bitStream.ReadUInt32();
            snapshotLength = bitStream.ReadUInt32();
            linkType = bitStream.ReadUInt32();
        }
    }
}
