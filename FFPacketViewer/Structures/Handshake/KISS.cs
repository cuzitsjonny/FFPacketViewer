using System;
using System.IO;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Structures.Handshake
{
    class KISS : Structure
    {
        public uint AssignedSocketIdentifier { get; set; }
        public ushort StreamingProtocolVersion { get; set; }

        public override void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            AssignedSocketIdentifier = bitStream.ReadUInt32();
            StreamingProtocolVersion = bitStream.ReadUInt16();
        }

        public override void WriteToTextWriter(TextWriter textWriter)
        {
            textWriter.WriteLine("Assigned Socket ID: " + AssignedSocketIdentifier);
            textWriter.WriteLine("Streaming Protocol Version: " + StreamingProtocolVersion);
        }
    }
}
