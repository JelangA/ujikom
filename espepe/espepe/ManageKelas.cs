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
    public partial class ManageKelas : Form
    {
        private DataSet ds;
        private MySqlDataAdapter da;
        private MySqlDataReader rd;
        private MySqlCommand cmd;

        koneksi koneksi = new koneksi();
        public ManageKelas()
        {
            InitializeComponent();
        }

        private void ManageKelas_Load(object sender, EventArgs e)
        {
            bersih();
        }
        private void incrementid()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("select id_kelas from kelas order by id_kelas desc", conn);
                rd = cmd.ExecuteReader();
                rd.Read();
                if (rd.HasRows)
                {
                    txt1.Text = (Convert.ToInt32(rd[0].ToString()) + 1).ToString();
                }
                else
                {
                    txt1.Text = "1";
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
                
            }
            
            conn.Close();
            rd.Close();
        }

        void bersih()
        {
            txt1.Text = "";
            txt2.Text = "";
            txt3.Text = "";
            incrementid();
            tampilData();

        }

        //============================== crud ========================================
        private void tampilData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("SELECT * from kelas", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "kelas");
                dataGridPetugas.DataSource = ds;
                dataGridPetugas.DataMember = "kelas";
            }
            catch (Exception g)
            {
                MessageBox.Show("Gagal mendapat data error[" + g.Message + "]!!!");
            }
            conn.Close();
        }

        private void insertData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("insert into kelas values('" +
                           txt1.Text + "','" +
                           txt2.Text + "','" +
                           txt3.Text + "')", conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("berhasil di simpan");
                bersih();
            }
            catch (Exception x)
            {
                MessageBox.Show("Gagal menambah data error[" + x.Message + "]!!!");
            }
            conn.Close();
        }

        private void updateData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("UPDATE kelas SET nama_kelas='" +
                    txt2.Text + "',kompetensi_keahlian='" +
                    txt3.Text + "'where id_kelas='" +
                    txt1.Text + "'", conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("berhasil di simpan");
                bersih();
            }
            catch (Exception x)
            {
                MessageBox.Show("Data Gagal Dipudate [error:" + x.Message + "]!!!");
            }
            conn.Close();
        }

        private void deleteData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("delete from kelas where id_kelas='" + txt1.Text + "'", conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Berhasil Di Hapus");
                conn.Close();
            }
            catch (Exception x)
            {
                MessageBox.Show("Gagal Dihapus [Error" + x.Message + "]!!!");
            }
            conn.Close();
            bersih();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            insertData();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            updateData();
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            deleteData();
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            bersih();
        }

        private void dataGridPetugas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridPetugas.Rows[e.RowIndex];
            txt1.Text = row.Cells[0].Value.ToString();
            txt2.Text = row.Cells[1].Value.ToString();
            txt3.Text = row.Cells[2].Value.ToString();
        }
    }
}
