using System;
using System.Data;
using System.Collections.Generic;
using FFPacketViewer.Serialization;

namespace FFPacketViewer.TcpDump
{
    class File
    {
        private string filePath;
        private GlobalHeader header;
        private List<Packet> packets;
        private DataTable dataTable;

        public File(string filePath)
        {
            this.filePath = filePath;
            this.header = null;
            this.packets = null;
        }

        public GlobalHeader GetHeader()
        {
            return header;
        }

        public Packet[] GetPackets()
        {
            return packets.ToArray();
        }

        public DataTable GetDataTable()
        {
            return dataTable;
        }

        public void Load()
        {
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            ReadOnlyBitStream bitStream = new ReadOnlyBitStream(fileData);

            header = new GlobalHeader();
            packets = new List<Packet>();
            dataTable = new DataTable();

            header.ReadFromBitStream(bitStream);

            while (bitStream.GetBitsUnread() > 0)
            {
                Packet packet = new Packet();

                packet.ReadFromBitStream(bitStream);

                packets.Add(packet);
            }

            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.UInt32");
            column.ColumnName = "Packet No.";
            column.ReadOnly = true;
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Time";
            column.ReadOnly = true;
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Source Address";
            column.ReadOnly = true;
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.UInt16");
            column.ColumnName = "Source Port";
            column.ReadOnly = true;
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Destination Address";
            column.ReadOnly = true;
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.UInt16");
            column.ColumnName = "Destination Port";
            column.ReadOnly = true;
            dataTable.Columns.Add(column);

            int pno = 0;

            foreach (Packet packet in packets)
            {
                if (packet.GetEthernetFrame().GetPacket().GetDatagram() != null)
                {
                    byte[] udpData = packet.GetEthernetFrame().GetPacket().GetDatagram().GetData();
                    DataRow row = dataTable.NewRow();

                    DateTime time = new DateTime(packet.GetHeader().GetTimestampSeconds() * TimeSpan.TicksPerSecond);

                    time = time.AddMilliseconds((double)packet.GetHeader().GetTimestampMicroseconds() / 1000d / 1000d);

                    row["Packet No."] = pno++;
                    row["Time"] = time.ToString("HH:mm:ss.fff");
                    row["Source Address"] = packet.GetEthernetFrame().GetPacket().GetSourceAsString();
                    row["Destination Address"] = packet.GetEthernetFrame().GetPacket().GetDestinationAsString();
                    row["Source Port"] = packet.GetEthernetFrame().GetPacket().GetDatagram().GetSourcePort();
                    row["Destination Port"] = packet.GetEthernetFrame().GetPacket().GetDatagram().GetDestinationPort();

                    dataTable.Rows.Add(row);
                }
            }
        }
    }
}
