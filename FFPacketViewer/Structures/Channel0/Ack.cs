using System;
using System.IO;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Structures.Channel0
{
    class Ack : Structure
    {
        public ushort SequenceNumber1 { get; set; }
        public ushort SequenceNumber2 { get; set; }

        public override void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            SequenceNumber1 = bitStream.ReadUInt16();
            SequenceNumber2 = bitStream.ReadUInt16();
        }

        public override void WriteToTextWriter(TextWriter textWriter)
        {
            textWriter.WriteLine("Sequence Number 1: " + SequenceNumber1);
            textWriter.WriteLine("Sequence Number 2: " + SequenceNumber2);
        }
    }
}
