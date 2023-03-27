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

namespace espepe
{
    public partial class HistoryPembayaran : Form
    {
        private MySqlDataAdapter da;
        private DataSet ds;
        private MySqlCommand cmd;

        koneksi koneksi = new koneksi();
        public HistoryPembayaran()
        {
            InitializeComponent();
        }

        private void tampildata()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try 
            {
                
                
                cmd = new MySqlCommand("SELECT pembayaran.nisn, petugas.nama_petugas, pembayaran.tgl_bayar, pembayaran.bulan_dibayar, pembayaran.tahun_dibayar, pembayaran.jumlah_bayar FROM pembayaran LEFT JOIN petugas ON pembayaran.id_petugas = petugas.id_petugas where nisn='" + SiswaForm.nisn + "'", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "pembayaran");
                bunifuDataGridView1.DataSource = ds;
                bunifuDataGridView1.DataMember = "pembayaran";
            }
            catch (Exception)
            {
                MessageBox.Show("Gagal mendapat data petugas");
            }
            conn.Close();
        }

        private void HistoryPembayaran_Load(object sender, EventArgs e)
        {
            tampildata();
        }
    }
}
