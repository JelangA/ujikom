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
    public partial class AdminForm : Form
    {
        private MySqlDataReader rd;
        private MySqlCommand cmd;

        public static string idPetugas;

        koneksi koneksi = new koneksi();

        private Form activeForm = null;

        public AdminForm()
        {
            InitializeComponent();
           
        }

        private void getDataUser()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            cmd = new MySqlCommand("select nama_petugas, id_petugas from petugas where id_user='" + LoginForm.UserID + "'", conn);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                labelnama.Text = rd[0].ToString().ToString();
                labelid.Text = rd[1].ToString().ToString();
                idPetugas = rd[1].ToString().ToString();
            }
            else
            {
                labelnama.Text = "error";
            }
            conn.Close();
            rd.Close();
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
        private void costumeDesign()
        {
            subPanel1.Visible = false;
            //subPanel2.Visible = false;
            subPanel3.Visible = false;
        }

        private void hideMenu()
        {
            if (subPanel1.Visible == true){ subPanel1.Visible = false; }
            //if (subPanel2.Visible == true){ subPanel2.Visible = false; }
            if (subPanel3.Visible == true){ subPanel3.Visible = false; }
            

        }

        private void showSubmenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                hideMenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            costumeDesign();
            getDataUser();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            showSubmenu(subPanel1);
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            //code..
            openChildForm(new ManagePetugasForm());
            hideMenu();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            //code..
            openChildForm(new ManageSiswaForm());
            hideMenu();
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            //code..
            openChildForm(new ManageKelas());
            hideMenu();
        }

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            showSubmenu(subPanel3);
        }

        private void bunifuButton9_Click(object sender, EventArgs e)
        {
            //code..
            openChildForm(new ManageSPPForm());
            hideMenu();
        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            //code..
            openChildForm(new PembayaranForm());
            hideMenu();
        }

        private void bunifuButton5_Click_1(object sender, EventArgs e)
        {
            //code..
            openChildForm(new ReportForm());
            hideMenu();
        }

        private void bunifuButton6_Click_2(object sender, EventArgs e)
        {
            this.Hide();
            new LoginForm().Show();
        }

        private void LogoPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuLabel1_Click(object sender, EventArgs e)
        {
            activeForm.Hide();
        }

        private void bunifuButton10_Click(object sender, EventArgs e)
        {
            openChildForm(new HostoryPembayaranAll());
            hideMenu();
        }
    }
}




