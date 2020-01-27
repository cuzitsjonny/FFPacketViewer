using System;
using System.Windows.Forms;
using System.ComponentModel;
using FFPacketViewer.Serialization;

namespace FFPacketViewer
{
    public partial class Viewer : Form
    {
        private Matrix.Reader matrixReader;
        private TcpDump.File captureFile;

        public Viewer()
        {
            InitializeComponent();

            this.matrixReader = new Matrix.Reader();

            CaptureFileLoader.DoWork += CaptureFileLoader_LoadCaptureFile;
            CaptureFileLoader.RunWorkerCompleted += CaptureFileLoader_CaptureFileLoaded;
        }

        private void OpenFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                OpenFileButton.Enabled = false;
                OpenFileButton.Text = "Loading...";

                captureFile = new TcpDump.File(dialog.FileName);

                CaptureFileLoader.RunWorkerAsync();
            }
        }

        private void CaptureFileLoader_LoadCaptureFile(object sender, DoWorkEventArgs e)
        {
            captureFile.Load();
        }

        private void CaptureFileLoader_CaptureFileLoaded(object sender, RunWorkerCompletedEventArgs e)
        {
            DataGrid.DataSource = captureFile.GetDataTable();

            OpenFileButton.Text = "Open";
            OpenFileButton.Enabled = true;

            DataGrid_CellClick(DataGrid, new DataGridViewCellEventArgs(0, 0));
        }

        private void DataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DataGrid.Rows[e.RowIndex];
                int pno = Int32.Parse(row.Cells["Packet No."].Value.ToString());
                byte[] data = captureFile.GetPackets()[pno].GetEthernetFrame().GetPacket().GetDatagram().GetData();
                ReadOnlyBitStream bitStream = new ReadOnlyBitStream(data, true);

                DescriptionLabel.Text = matrixReader.Read(bitStream);
            }
        }
    }
}
