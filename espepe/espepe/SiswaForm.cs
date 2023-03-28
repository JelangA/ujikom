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
    public partial class SiswaForm : Form
    {

        private MySqlDataReader rd;
        private MySqlCommand cmd;

        public static string idsiswa;
        public static string nisn;
        public string sppdibayar;
        public int jumlahspp = 3000000;

        koneksi koneksi = new koneksi();

        private Form activeForm = null;

        public SiswaForm()
        {
            InitializeComponent();
        }

        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            ChildPanel.Controls.Add(childForm);
            ChildPanel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void getDataUser()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            cmd = new MySqlCommand("select siswa.nama, siswa.id_siswa, siswa.id_siswa, spp.nominal, siswa.nisn, kelas.kompetensi_keahlian from siswa LEFT JOIN spp on siswa.id_spp = spp.id_spp LEFT JOIN kelas on siswa.id_kelas = kelas.id_kelas where id_user='" + LoginForm.UserID + "'", conn);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                labelnama.Text = rd[0].ToString();
                labelid.Text = rd[1].ToString();
                idsiswa = rd[1].ToString();
                nisn = rd["nisn"].ToString();
                nisnlabel.Text = rd[2].ToString();
                SPP.Text = rd[3].ToString();
                sppdibayar = rd[3].ToString();
            }
            else
            {
                labelnama.Text = "error";
            }
            conn.Close();
            rd.Close();
        }

       

        private void SiswaForm_Load(object sender, EventArgs e)
        {
            getDataUser();
            if (Convert.ToInt32(sppdibayar) >= jumlahspp)
            {
                labelstatus.Text = "Belum Lunas";
            }
            else
            {
                labelstatus.Text = "Belum Lunas";
            }
            sisalabel.Text = (jumlahspp - Convert.ToInt32(sppdibayar)).ToString();

        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            openChildForm(new HistoryPembayaran());
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            this.Hide();
            new LoginForm().Show();
        }

        private void bunifuLabel1_Click(object sender, EventArgs e)
        {
            activeForm.Hide();
        }

        
        
    }
}
