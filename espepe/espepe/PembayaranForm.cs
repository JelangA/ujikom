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
using System.Globalization;

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
        string jumlahnominal = "";
        string nominalasal = "";

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
            txt2.Text = LoginForm.UserID;
            txt3.Text = "Pilih Nisn";
            txt5.Text = "";
            txt6.Text = "";
            txt7.Text = "";
            txt8.Text = "";
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
                    txt3.Items.Add(nisn);
                }
            }
            catch (Exception g)
            {
                MessageBox.Show(g.Message);
            }
            conn.Close();
            rd.Close();

        }

        private void setnisn()
        {

        }

        private void getnisn()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try

            {
                cmd = new MySqlCommand("select * from siswa where nisn='" + txt3.Text + "'" , conn);
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

        private void tambahNominal()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            cmd = new MySqlCommand("select nominal from spp where id_spp='" + txt7.Text + "'", conn);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                jumlahnominal = (Convert.ToInt32(rd[0].ToString()) + Convert.ToInt32(txt8.Text)).ToString();
            }

            conn.Close();
            rd.Close();
        }

        //==================================== crud =============================

        private void tampilData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("SELECT a.id_pembayaran, c.nama_petugas, b.nisn, b.nama, a.tgl_bayar, a.bulan_dibayar, a.tahun_dibayar, a.id_spp, a.jumlah_bayar FROM pembayaran as a LEFT JOIN siswa as b ON a.nisn = b.nisn LEFT JOIN petugas as c ON a.id_petugas = c.id_petugas", conn);
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
                           DateNow.Text + "','" +
                           txt5.Text + "','" +
                           txt6.Text + "','" +
                           txt7.Text + "','" +
                           txt8.Text + "')", conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("berhasil di simpan");
                tambahNominal();
                updatenominal();
                bersih();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
            conn.Close();
        }

        private void updatenominal()
        {
            tambahNominal();
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("UPDATE spp SET nominal='" +
                    jumlahnominal + "'where id_spp='" +
                    txt7.Text + "'", conn);
                cmd.ExecuteNonQuery();
                bersih();
            }
            catch (Exception x)
            {
                MessageBox.Show("Data Gagal Dipudate [error:" + x.Message + "]!!!");
            }
            conn.Close();
        }



        private void updateData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("UPDATE pembayaran SET bulan_dibayar='" +
                    txt5.Text + "',tahun_dibayar='" +
                    txt6.Text + "'where id_pembayaran='" +
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
                cmd = new MySqlCommand("delete from pembayaran where id_pembayaran='" + txt1.Text + "'", conn);
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
            if (txt1.Text == null ||
               txt2.Text == null ||
               txt3.Text == "Pilih Nisn" ||
               DateNow.Text == null ||
               txt5.Text == null ||
               txt6.Text == null ||
               txt7.Text == null ||
               txt8.Text == null)
            {
                MessageBox.Show("Lengkapi Data");
            }
            else
            {
                insertData();
            }
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            bersih();
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
            printer.PrintPreviewDataGridView(dataGridPetugas);
            printer.PrintMargins.Top = 20;
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            deleteData();
        }

        private void dataGridPetugas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridPetugas.Rows[e.RowIndex];
            txt1.Text = row.Cells[0].Value.ToString();
            txt3.Text = row.Cells[2].Value.ToString();
            txt5.Text = row.Cells[5].Value.ToString();
            txt6.Text = row.Cells[6].Value.ToString();
            txt7.Text = row.Cells[7].Value.ToString();
            txt8.Text = row.Cells[8].Value.ToString();
            nominalasal = row.Cells[8].Value.ToString();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            updateData();
        }
    }
}
