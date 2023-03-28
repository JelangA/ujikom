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
    public partial class ManageSiswaForm : Form
    {
        private DataSet ds;
        private MySqlDataAdapter da;
        private MySqlDataReader rd;
        private MySqlCommand cmd, cmd2, cmd3;

        string tahun = DateTime.Now.ToString("yyyy");

        public string idkelas;

        koneksi koneksi = new koneksi();
        public ManageSiswaForm()
        {
            InitializeComponent();
        }

        void bersih()
        {
            txt1.Text = "";
            txt2.Text = "";
            txt3.Text = "";
            txt4.Text = "";
            txt5.Text = "";
            txt6.Text = "";
            txt7.Text = "Pilih Kelas";
            txt8.Text = "";
            txtIdLevel.Text = "";
            idkelas = "";
            textBox1.Text = "";
            incrementid();
            incrementidLevel();
            tampilData();
            incrementidSiswa();
        }

        void cmbKelas()
        {
            //get data IdOrder from database to combobox1 
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try

            {
                cmd = new MySqlCommand("select nama_kelas from kelas", conn);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    string sName = rd.GetString(0);
                    txt7.Items.Add(sName);
                }
            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message);
            }
            conn.Close();
            rd.Close();

        }

        //========================================================== manual increment ================================================

        private void incrementid()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            cmd = new MySqlCommand("select id_spp from spp order by id_spp desc", conn);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                txt8.Text = (Convert.ToInt32(rd[0].ToString()) + 1).ToString();
            }
            else
            {
                txt8.Text = "1";
            }
            conn.Close();
            rd.Close();
        }

        private void incrementidLevel()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            cmd = new MySqlCommand("select id_user from level_user order by id_user desc", conn);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                txtIdLevel.Text = (Convert.ToInt32(rd[0].ToString()) + 1).ToString();
            }
            else
            {
                txtIdLevel.Text = "1";
            }
            conn.Close();
            rd.Close();
        }

        private void incrementidSiswa()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            cmd = new MySqlCommand("select id_siswa from siswa order by id_siswa desc", conn);
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
            conn.Close();
            rd.Close();
        }

        private void getidKelas()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try

            {
                cmd = new MySqlCommand("select id_kelas from kelas where nama_kelas='"  + txt7.Text +"'", conn);
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    textBox1.Text = rd.GetString(0);
                }
            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message);
            }
            conn.Close();
            rd.Close();
        }

        //======================================================= crud ======================================================

        private void tampilData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("SELECT siswa.id_siswa, siswa.nisn, siswa.nis, siswa.nama, siswa.alamat, siswa.no_telp, siswa.id_kelas, siswa.id_spp, siswa.id_user FROM `siswa`", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "siswa");
                dataGridSiswa.DataSource = ds;
                dataGridSiswa.DataMember = "siswa";
            }
            catch (Exception)
            {
                MessageBox.Show("Gagal mendapat data");
            }
            conn.Close();
        }

        private void insertData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("insert into siswa values('" +
                           txt1.Text + "','" +
                           txt2.Text + "','" +
                           txt3.Text + "','" +
                           txt4.Text + "','" +
                           txt5.Text + "','" +
                           txt6.Text + "','" +
                           textBox1.Text + "','" +
                           txt8.Text + "','" +
                           txtIdLevel.Text + "')", conn);
                cmd2 = new MySqlCommand("insert into level_user values('" +
                           txtIdLevel.Text + "','" +
                           txt2.Text + "','" +
                           txt3.Text + "','siswa')", conn);
                cmd3 = new MySqlCommand("insert into spp values('" +
                           txt8.Text + "','" +
                           tahun + "','0')", conn);
                cmd3.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
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
                cmd = new MySqlCommand("UPDATE siswa SET nisn='" +
                    txt2.Text + "',nis='" +
                    txt3.Text + "',nama='" +
                    txt4.Text + "',alamat='" +
                    txt5.Text + "',no_telp='" +
                    txt6.Text + "',id_kelas='" +
                    textBox1.Text + "',id_spp='" +
                    txt8.Text + "',id_user='" +
                    txtIdLevel.Text + "'where id_siswa='" +
                    txt1.Text + "'", conn);
                cmd2 = new MySqlCommand("UPDATE level_user SET username='" +
                    txt2.Text + "',password='" +
                    txt3.Text + "'where id_user='" +
                    txtIdLevel.Text + "'", conn);
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
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
                cmd2 = new MySqlCommand("delete from level_user where id_user='" + txtIdLevel.Text + "'", conn);
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
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


        //========================================================== click event ===========================================

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (txt1.Text == null ||
                txt4.Text == null ||
                txt5.Text == null ||
                txt6.Text == null ||
                txt7.Text == "Pilih Kelas" ||
                txt8.Text == null ||
                textBox1.Text == null ||
                txtIdLevel == null)
            {
                MessageBox.Show("Lengkapi Data");
            }
            else if (txt2.Text.Length < 10)
            {
                MessageBox.Show("Data nisn harus 10 karakter ");
            }
            else if(txt3.Text.Length < 8)
            {
                MessageBox.Show("Data nisn harus 8 karakter ");
            }
            else if (txt2.Text.Length < 10)
            {
                MessageBox.Show("Data nisn harus 10 karakter ");
            }
            else if (txt3.Text.Length < 8)
            {
                MessageBox.Show("Data nisn harus 8 karakter ");
            }
            else
            {
                insertData();
            }
            
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            if (txt1 == null ||
                txt2 == null ||
                txt3 == null ||
                txt4 == null ||
                txt5 == null ||
                txt6 == null ||
                textBox1.Text == "" || 
                txt8 == null ||
                txtIdLevel == null)
            {
                MessageBox.Show("Lengkapi Data");
            }
            else
            {
                updateData();
            }
            
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            deleteData();
        }

        private void dataGridPetugas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridSiswa.Rows[e.RowIndex];
            txt1.Text = row.Cells[0].Value.ToString();
            txt2.Text = row.Cells[1].Value.ToString();
            txt3.Text = row.Cells[2].Value.ToString();
            txt4.Text = row.Cells[3].Value.ToString();
            txt5.Text = row.Cells[4].Value.ToString();
            txt6.Text = row.Cells[5].Value.ToString();
            textBox1.Text = row.Cells[6].Value.ToString();
            txt8.Text = row.Cells[7].Value.ToString();
            txtIdLevel.Text = row.Cells[8].Value.ToString();
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Daftar User";
            
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
            printer.PrintPreviewDataGridView(dataGridSiswa);
            printer.PrintMargins.Top = 20;
        }

        private void txt7_SelectedIndexChanged(object sender, EventArgs e)
        {
            getidKelas();
        }

        private void bunifuLabel7_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            bersih();
        }

        private void ManageSiswaForm_Load(object sender, EventArgs e)
        {
            bersih();
            cmbKelas();

        }
    }
}
