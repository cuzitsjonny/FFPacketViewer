using System;
using System.IO;
using System.Collections.Generic;
using FFPacketViewer.Serialization;
using FFPacketViewer.Structures;

namespace FFPacketViewer.Matrix
{
    class Reader
    {
        private Dictionary<string, Structure> handshakeStructs;
        private Dictionary<byte, Structure> channel0Structs;

        public Reader()
        {
            this.handshakeStructs = new Dictionary<string, Structure>();
            this.channel0Structs = new Dictionary<byte, Structure>();

            LoadHandshakeStructs();
            LoadChannel0Structs();
        }

        public string Read(ReadOnlyBitStream bitStream)
        {
            StringWriter output = new StringWriter();
            uint socketIdentifier = bitStream.ReadUInt32();
            bool isHandshake = socketIdentifier == 0;

            output.WriteLine("Is Handshake: " + isHandshake);
            output.WriteLine("Socket ID: " + socketIdentifier);

            if (isHandshake)
            {
                string messageIdentifier = "";

                for (int i = 0; i < 4; i++)
                {
                    messageIdentifier += (char)bitStream.ReadByte();
                }

                output.WriteLine("Message ID: " + messageIdentifier);

                if (handshakeStructs.ContainsKey(messageIdentifier))
                {
                    handshakeStructs[messageIdentifier].ReadFromBitStream(bitStream);
                    handshakeStructs[messageIdentifier].WriteToTextWriter(output);
                }
            }
            else
            {
                byte channel = bitStream.ReadCrumb();
                byte resendCount = bitStream.ReadCrumb();
                bool isSplit = bitStream.ReadBit();
                Util.UInt11 length = new Util.UInt11();

                length.ReadFromBitStream(bitStream);

                output.WriteLine("Channel: " + channel);
                output.WriteLine("Resend Count: " + resendCount);
                output.WriteLine("Is Split: " + isSplit);
                output.WriteLine("Length: " + length);

                if (channel == 0)
                {
                    byte messageIdentifier = bitStream.ReadByte();

                    output.Write("Message ID: " + messageIdentifier);

                    if (channel0Structs.ContainsKey(messageIdentifier))
                    {
                        Structure srct = channel0Structs[messageIdentifier];

                        output.WriteLine(" (" + srct.GetType().Name + ")");

                        srct.ReadFromBitStream(bitStream);
                        srct.WriteToTextWriter(output);
                    }
                }
            }

            if (bitStream.GetBitsUnread() > 0)
            {
                output.WriteLine();

                if (bitStream.GetBitsUnread() % 8 == 0)
                {
                    output.WriteLine("Unread Bytes:");
                    byte[] bytes = bitStream.ReadBytes(bitStream.GetBitsUnread() / 8);

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        output.WriteLine(bytes[i]);
                    }
                }
                else
                {
                    output.WriteLine("Unread Bits:");

                    for (int i = 0; i < bitStream.GetBitsUnread(); i++)
                    {
                        output.WriteLine(bitStream.ReadBit() ? 1 : 0);
                    }
                }
            }

            return output.ToString();
        }

        private void LoadHandshakeStructs()
        {
            handshakeStructs["POKE"] = new Structures.Handshake.POKE();
            handshakeStructs["HEHE"] = new Structures.Handshake.HEHE();
            handshakeStructs["KISS"] = new Structures.Handshake.KISS();
            handshakeStructs["HUGG"] = new Structures.Handshake.HUGG();
        }
        private void LoadChannel0Structs()
        {
            channel0Structs[0] = new Structures.Channel0.CloseConnection();
            channel0Structs[2] = new Structures.Channel0.Ack();
            channel0Structs[4] = new Structures.Channel0.TimeSyncRequest();
            channel0Structs[5] = new Structures.Channel0.TimeSyncResponse();
        }
    }
}
