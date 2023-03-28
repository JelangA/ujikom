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
    public partial class ManageSPPForm : Form
    {
        private DataSet ds;
        private MySqlDataAdapter da;
        private MySqlDataReader rd;
        private MySqlCommand cmd;

        koneksi koneksi = new koneksi();

        public ManageSPPForm()
        {
            InitializeComponent();
        }

        void bersih()
        {
            txt1.Text = "";
            txt2.Text = "";
            txt3.Text = "";
            incrementid();
            tampilData();

        }

        private void manageSPPForm_Load(object sender, EventArgs e)
        {
            bersih();
        }



        //================================================== crud =========================================

        private void incrementid()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            cmd = new MySqlCommand("select id_spp from spp order by id_spp desc", conn);
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

        private void tampilData()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("SELECT * from spp", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "spp");
                dataGridSPP.DataSource = ds;
                dataGridSPP.DataMember = "spp";
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
                cmd = new MySqlCommand("insert into spp values('" +
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
                cmd = new MySqlCommand("UPDATE spp SET tahun='" +
                    txt2.Text + "',nominal='" +
                    txt3.Text + "'where id_spp='" +
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
                cmd = new MySqlCommand("delete from spp where id_spp='" + txt1.Text + "'", conn);
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

        //====================================================== click event =========================================

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (txt2.Text == "" || txt3.Text =="")
            {
                MessageBox.Show(" Tolong Lengkapi Data");
            }
            else
            {
                insertData();
            }
            
        }
        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            if (txt1.Text == "" || txt2.Text == "" || txt3.Text == "")
            {
                MessageBox.Show(" Tolong Lengkapi Data");
            }
            else
            {
                updateData();
            }
            
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            if (txt1.Text == "")
            {
                MessageBox.Show("Tolong Lengkapi Data");
            }
            {
                deleteData();
            }
            
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            bersih();
        }

        private void dataGridPetugas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataGridSPP.Rows[e.RowIndex];
            txt1.Text = row.Cells[0].Value.ToString();
            txt2.Text = row.Cells[1].Value.ToString();
            txt3.Text = row.Cells[2].Value.ToString();
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
            printer.PrintPreviewDataGridView(dataGridSPP);
            printer.PrintMargins.Top = 20;
        }
    }
}
