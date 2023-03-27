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
            cmd = new MySqlCommand("select nama, id_siswa, nisn from siswa where id_user='" + LoginForm.UserID + "'", conn);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                labelnama.Text = rd[0].ToString();
                labelid.Text = rd[1].ToString();
                idsiswa = rd[1].ToString();
                nisn = rd[2].ToString();
                nisnlabel.Text = rd[2].ToString();
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
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            openChildForm(new HistoryPembayaran());
        }
    }
}
