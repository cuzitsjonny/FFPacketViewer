using System;
using System.IO;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.Structures
{
    abstract class Structure : IBinaryUnserializable, ITextSerializable
    {
        public abstract void ReadFromBitStream(ReadOnlyBitStream bitStream);
        public abstract void WriteToTextWriter(TextWriter textWriter);
    }
}
