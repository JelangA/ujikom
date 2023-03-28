using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DGVPrinterHelper;

namespace espepe
{
    public partial class ReportForm : Form
    {

        public ReportForm()
        {
            InitializeComponent();
        }

        private MySqlDataAdapter da;
        private DataSet ds;
        private MySqlCommand cmd;

        koneksi koneksi = new koneksi();

        private void ReportForm_Load(object sender, EventArgs e)
        {
            tampildata(); 
        }

        void bersih()
        {
            tampildata();
            TextBox1.Text = "";
        }

        private void tampildata()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("SELECT a.id_pembayaran, c.nama_petugas, b.nisn, b.nama, a.tgl_bayar, a.bulan_dibayar, a.tahun_dibayar, a.id_spp, a.jumlah_bayar FROM pembayaran as a LEFT JOIN siswa as b ON a.nisn = b.nisn LEFT JOIN petugas as c ON a.id_petugas = c.id_petugas", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "pembayaran");
                bunifuDataGridView1.DataSource = ds;
                bunifuDataGridView1.DataMember = "pembayaran";
            }
            catch (Exception)
            {
                MessageBox.Show("Gagal mendapat data pembayaran");
            }
            conn.Close();
        }

        private void seleksiData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("SELECT a.id_pembayaran, c.nama_petugas, b.nisn, b.nama, a.tgl_bayar, a.bulan_dibayar, a.tahun_dibayar, a.id_spp, a.jumlah_bayar FROM pembayaran as a LEFT JOIN siswa as b ON a.nisn = b.nisn LEFT JOIN petugas as c ON a.id_petugas = c.id_petugas where a.tgl_bayar like'%" + TextBox1.Text + "%'", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "pembayaran");
                bunifuDataGridView1.DataSource = ds;
                bunifuDataGridView1.DataMember = "pembayaran";
            }
            catch (Exception)
            {
                MessageBox.Show("Gagal mendapat data pembayaran");
            }
            conn.Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Report Data Pembayaran SPP";

            printer.SubTitle = string.Format(
                "tanggal {0}", DateTime.Now.Date.ToString("dddd-MMMM-yyyy")
                );
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.TitleSpacing = 12;
            printer.FooterSpacing = 15;
            printer.PrintMargins.Top = 20;
            printer.PrintPreviewDataGridView(bunifuDataGridView1);
            printer.PrintMargins.Top = 20;
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            bersih();
        }

        private void bunifuDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bunifuButton2_Click_1(object sender, EventArgs e)
        {
            seleksiData();
        }
    }
}
