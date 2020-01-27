using System;
using System.IO;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Structures.Channel0
{
    class CloseConnection : Structure
    {
        public ulong Zeros { get; set; }

        public override void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            Zeros = bitStream.ReadUInt64();
        }

        public override void WriteToTextWriter(TextWriter textWriter)
        {
            textWriter.WriteLine("Zeros: " + Zeros);
        }
    }
}
