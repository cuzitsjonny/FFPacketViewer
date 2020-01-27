using System;
using System.IO;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Structures.Handshake
{
    class HUGG : Structure
    {
        public ushort SequenceStart { get; set; }
        public ushort GameServerPort { get; set; }

        public override void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            SequenceStart = bitStream.ReadUInt16();
            GameServerPort = bitStream.ReadUInt16();
        }

        public override void WriteToTextWriter(TextWriter textWriter)
        {
            textWriter.WriteLine("Sequence Start: " + SequenceStart);
            textWriter.WriteLine("Game Server Port: " + GameServerPort);
        }
    }
}
