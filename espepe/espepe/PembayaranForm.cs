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
    public partial class PembayaranForm : Form
    {

        private DataSet ds;
        private MySqlDataAdapter da;
        private MySqlDataReader rd;
        private MySqlCommand cmd;

        koneksi koneksi = new koneksi();

        string nisnS = "";

        public PembayaranForm()
        {
            InitializeComponent();
        }

        private void PembayaranForm_Load(object sender, EventArgs e)
        {
            cmbNis();
            bersih();
        }

        void bersih()
        {
            txt1.Text = Autoid(); ;
            txt2.Text = AdminForm.idPetugas;
            txt3.Text = "Pilih Nisn";
            txt4.Text = DateTime.Now.ToString("dd");
            txt5.Text = "";
            txt6.Text = "";
            txt7.Text = "";
            txt8.Text = "";
            txtIdLevel.Text = "";
            //incrementidLevel();
            tampilData();
            //incrementidSiswa();


        }

        void cmbNis()
        {
            //get data IdOrder from database to combobox1 
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try

            {
                cmd = new MySqlCommand("select * from siswa", conn);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    string nisn = rd.GetString(1);
                    string sName = rd.GetString(3);
                    txt3.Items.Add(nisn + " | " + sName);
                    nisnS = nisn;
                }
            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message);
            }
            conn.Close();
            rd.Close();

        }

        private void getnisn()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try

            {
                cmd = new MySqlCommand("select * from siswa where nisn='" + nisnS + "'" , conn);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    string id = rd.GetString(7);

                    txt7.Text = id;
                }
            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message);
            }
            conn.Close();
            rd.Close();
        }

        //==================================== increment ==============================

        private string Autoid()
        {
            try
            {
                MySqlConnection conn = koneksi.GetKon   ();
                conn.Open();
                cmd = new MySqlCommand("select id_pembayaran from pembayaran order by id_pembayaran desc", conn);
                rd = cmd.ExecuteReader();
                rd.Read();
                if (rd.HasRows)
                {
                    string id = (Convert.ToInt32(rd["id_pembayaran"].ToString()) + 1).ToString();
                    return id;
                }
                else
                {
                    string result = "001";
                    string tanggal = DateTime.Now.ToString("MMdd");
                    string result2 = tanggal + result;
                    return result2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return "";
        }

        //==================================== crud =============================

        private void tampilData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("SELECT * FROM `pembayaran`", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "pembayaran");
                dataGridPetugas.DataSource = ds;
                dataGridPetugas.DataMember = "pembayaran";
            }
            catch (Exception)
            {
                MessageBox.Show("Gagal mendapat data ");
            }
            conn.Close();
        }

        private void insertData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("insert into pembayaran values('" +
                           txt1.Text + "','" +
                           txt2.Text + "','" +
                           txt3.Text + "','" +
                           txt4.Text + "','" +
                           txt5.Text + "','" +
                           txt6.Text + "','" +
                           txt7.Text + "','" +
                           txt8.Text + "')", conn);
                //cmd2 = new MySqlCommand("insert into level_user values('" +
                //           txtIdLevel.Text + "','" +
                //           txt2.Text + "','" +
                //           txt3.Text + "','siswa')", conn);
                //cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                MessageBox.Show("berhasil di simpan");
                bersih();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
            conn.Close();
        }

        private void updateData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("UPDATE siswa SET bulan_dibayar='" +
                    txt2.Text + "',tahun_dibayar='" +
                    txt8.Text + "',jumlah_dibayar='" +
                    txtIdLevel.Text + "'where id_siswa='" +
                    txt1.Text + "'", conn);
                //cmd2 = new MySqlCommand("UPDATE level_user SET username='" +
                //    txt2.Text + "',password='" +
                //    txt3.Text + "'where id_user='" +
                //    txtIdLevel.Text + "'", conn);
                //cmd2.ExecuteNonQuery();
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
                cmd = new MySqlCommand("delete from siswa where id_siswa='" + txt1.Text + "'", conn);
                //cmd2 = new MySqlCommand("delete from level_user where id_user='" + txtIdLevel.Text + "'", conn);
                //cmd2.ExecuteNonQuery();
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

        private void txt3_SelectedIndexChanged(object sender, EventArgs e)
        {
            getnisn();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            insertData();
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            bersih();
        }
    }
}
