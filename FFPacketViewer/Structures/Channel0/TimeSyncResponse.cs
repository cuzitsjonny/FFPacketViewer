using System;
using System.IO;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Structures.Channel0
{
    class TimeSyncResponse : Structure
    {
        public long ClientTimeUnix { get; set; }
        public long ServerTimeUnix { get; set; }

        public override void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            bool isBigEndian = bitStream.IsBigEndian();

            bitStream.SetBigEndian(false);

            ClientTimeUnix = bitStream.ReadInt64();
            ServerTimeUnix = bitStream.ReadInt64();

            bitStream.SetBigEndian(isBigEndian);
        }

        public override void WriteToTextWriter(TextWriter textWriter)
        {
            textWriter.WriteLine("Client Time Unix: " + ClientTimeUnix);
            textWriter.WriteLine("Server Time Unix: " + ServerTimeUnix);
        }
    }
}
