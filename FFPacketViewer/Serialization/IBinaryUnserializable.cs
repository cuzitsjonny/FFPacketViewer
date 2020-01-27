using System;

namespace FFPacketViewer.Serialization
{
    interface IBinaryUnserializable
    {
        void ReadFromBitStream(ReadOnlyBitStream bitStream);
    }
}
