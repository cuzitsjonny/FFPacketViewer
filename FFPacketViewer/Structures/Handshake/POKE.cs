using System;
using System.IO;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Structures.Handshake
{
    class POKE : Structure
    {
        public uint ProtcolVersion { get; set; }

        public override void ReadFromBitStream(ReadOnlyBitStream bitStream)
        {
            ProtcolVersion = bitStream.ReadUInt32();
        }

        public override void WriteToTextWriter(TextWriter textWriter)
        {
            textWriter.WriteLine("Protocol Version: " + ProtcolVersion);
        }
    }
}
