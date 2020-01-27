using System;
using System.IO;

namespace FFPacketViewer.Serialization
{
    public class ReadOnlyBitStream
    {
        private static readonly byte[] ByteBitMasks = new byte[] { 128, 64, 32, 16, 8, 4, 2, 1 };

        private byte[] stream;
        private int bitsAllocated;
        private int bitsRead;
        private bool isBigEndian;

        public ReadOnlyBitStream(byte[] byteArray, bool isBigEndian = false)
        {
            this.stream = byteArray;
            this.bitsAllocated = byteArray.Length * 8;
            this.bitsRead = 0;
            this.isBigEndian = isBigEndian;
        }

        public void CheckForEndOfStream(int bitsNeeded)
        {
            int bitsUnread = bitsAllocated - bitsRead;

            if (bitsUnread < bitsNeeded)
            {
                throw new EndOfStreamException("Tried to read " + bitsNeeded + " bits but only " + bitsUnread + " unread bits left.");
            }
        }

        public int GetBitsRead()
        {
            return bitsRead;
        }

        public int GetBitsUnread()
        {
            return bitsAllocated - bitsRead;
        }

        public bool IsBigEndian()
        {
            return isBigEndian;
        }

        public void SetBigEndian(bool isBigEndian)
        {
            this.isBigEndian = isBigEndian;
        }

        public void SkipBits(int bitsToSkip)
        {
            CheckForEndOfStream(bitsToSkip);

            bitsRead += bitsToSkip;
        }

        public void SkipBytes(int bytesToSkip)
        {
            CheckForEndOfStream(bytesToSkip * 8);

            bitsRead += bytesToSkip * 8;
        }

        public byte ReadByte()
        {
            CheckForEndOfStream(8);

            byte value = 0;

            for (int i = 0; i < 8; i++)
            {
                int readIndex = (int)Math.Floor(bitsRead / 8.0);

                int readValue = stream[readIndex];
                int writeValue = value;

                int readMask = ByteBitMasks[bitsRead % 8];
                int writeMask = ByteBitMasks[i];

                if ((readValue & readMask) == readMask)
                {
                    writeValue = (writeValue | writeMask);
                    value = (byte)writeValue;
                }

                bitsRead++;
            }

            return value;
        }

        public byte[] ReadBytes(int length, bool considerEndianness = false)
        {
            CheckForEndOfStream(length * 8);

            byte[] value = new byte[length];

            for (int writeIndex = 0; writeIndex < value.Length; writeIndex++)
            {
                for (int i = 0; i < 8; i++)
                {
                    int readIndex = (int)Math.Floor(bitsRead / 8.0);

                    int readValue = stream[readIndex];
                    int writeValue = value[writeIndex];

                    int readMask = ByteBitMasks[bitsRead % 8];
                    int writeMask = ByteBitMasks[i];

                    if ((readValue & readMask) == readMask)
                    {
                        writeValue = (writeValue | writeMask);
                        value[writeIndex] = (byte)writeValue;
                    }

                    bitsRead++;
                }
            }

            if (considerEndianness)
            {
                if (isBigEndian)
                {
                    if (value.Length > 0)
                    {
                        Array.Reverse(value);
                    }
                }
            }

            return value;
        }

        public sbyte ReadSByte()
        {
            return (sbyte)ReadByte();
        }

        public short ReadInt16()
        {
            return BitConverter.ToInt16(ReadBytes(2, true), 0);
        }

        public ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(ReadBytes(2, true), 0);
        }

        public int ReadInt32()
        {
            return BitConverter.ToInt32(ReadBytes(4, true), 0);
        }

        public uint ReadUInt32()
        {
            return BitConverter.ToUInt32(ReadBytes(4, true), 0);
        }

        public long ReadInt64()
        {
            return BitConverter.ToInt64(ReadBytes(8, true), 0);
        }

        public ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(ReadBytes(8, true), 0);
        }

        public float ReadSingle()
        {
            return BitConverter.ToSingle(ReadBytes(4, true), 0);
        }

        public bool ReadBit()
        {
            CheckForEndOfStream(1);

            bool value = false;

            int readIndex = (int)Math.Floor(bitsRead / 8.0);
            int readValue = stream[readIndex];
            int readMask = ByteBitMasks[bitsRead % 8];

            if ((readValue & readMask) == readMask)
            {
                value = true;
            }

            bitsRead++;

            return value;
        }

        public byte ReadNibble()
        {
            bool b1 = ReadBit();
            bool b2 = ReadBit();
            bool b3 = ReadBit();
            bool b4 = ReadBit();
            byte value = 0;

            if (b4)
            {
                value += 1;
            }

            if (b3)
            {
                value += 2;
            }

            if (b2)
            {
                value += 4;
            }

            if (b1)
            {
                value += 8;
            }

            return value;
        }

        public byte ReadCrumb()
        {
            bool b1 = ReadBit();
            bool b2 = ReadBit();
            byte value = 0;

            if (b2)
            {
                value += 1;
            }

            if (b1)
            {
                value += 2;
            }

            return value;
        }
    }
}
