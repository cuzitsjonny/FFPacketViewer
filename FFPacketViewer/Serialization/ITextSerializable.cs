using System;
using System.IO;

namespace FFPacketViewer.Serialization
{
    interface ITextSerializable
    {
        void WriteToTextWriter(TextWriter textWriter);
    }
}
