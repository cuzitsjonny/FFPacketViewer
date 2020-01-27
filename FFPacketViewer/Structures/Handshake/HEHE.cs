using System;
using System.IO;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Structures.Handshake
{
    class HEHE : Structure
    {
        public uint AssignedSocketIdentifier { get; set; }

        public override void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            AssignedSocketIdentifier = bitStream.ReadUInt32();
        }

        public override void WriteToTextWriter(TextWriter textWriter)
        {
            textWriter.WriteLine("Assigned Socket ID: " + AssignedSocketIdentifier);
        }
    }
}
