﻿using System;
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
                cmd = new MySqlCommand("SELECT * from pembayaran where nisn='" + SiswaForm.nisn + "'", conn);
                //cmd = new MySqlCommand("SELECT a.id_pembayaran, c.nama_petugas, b.nisn, b.nama, a.tgl_bayar, a.bulan_dibayar, a.tahun_dibayar, a.id_spp, a.jumlah_bayar FROM pembayaran as a LEFT JOIN siswa as b ON a.nisn = b.nisn LEFT JOIN petugas as c ON a.id_petugas = c.id_petugas where a.nisn='" + SiswaForm.nisn + "'", conn);
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

        private void HistoryPembayaran_Load(object sender, EventArgs e)
        {
            tampildata();
        }
    }
}
