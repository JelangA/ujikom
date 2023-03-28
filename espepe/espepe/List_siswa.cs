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
    public partial class List_siswa : Form
    {
        private DataSet ds;
        private MySqlDataAdapter da;
        private MySqlDataReader rd;
        private MySqlCommand cmd;

       
        public List_siswa()
        {
            InitializeComponent();
        }

        private void tampildata()
        {
            //MySqlConnection conn = koneksi.GetKon();
            //conn.Open();
            //try
            //{
            //    cmd = new MySqlCommand("SELECT siswa.id_siswa, siswa.nisn, siswa.nis, siswa.nama, siswa.alamat, siswa.no_telp, siswa.id_kelas, siswa.id_spp, siswa.id_user FROM `siswa`", conn);
            //    ds = new DataSet();
            //    da = new MySqlDataAdapter(cmd);
            //    da.Fill(ds, "siswa");
            //    dataGridSiswa.DataSource = ds;
            //    dataGridSiswa.DataMember = "siswa";
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Gagal mendapat data");
            //}
            //conn.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
