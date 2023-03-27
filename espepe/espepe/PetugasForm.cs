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
    public partial class PetugasForm : Form
    {
        private DataSet ds;
        private MySqlCommand cmd;
        private MySqlDataReader rd;

        koneksi koneksi = new koneksi();

        public static string idPetugas;
        
        public PetugasForm()
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

        private void costumeDesign()
        {
            subPanel3.Visible = false;
        }

        private void hideMenu()
        {
            if (subPanel3.Visible == true) { subPanel3.Visible = false; }
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

        private void bunifuButton7_Click(object sender, EventArgs e)
        {
            showSubmenu(subPanel3);
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            //code...

            hideMenu();
        }

        private void bunifuButton5_Click(object sender, EventArgs e)
        {
            //code...

            hideMenu();
        }

        private void bunifuButton8_Click(object sender, EventArgs e)
        {
            //code...

            hideMenu();
        }

        private void bunifuButton9_Click(object sender, EventArgs e)
        {
            //code...

            hideMenu();
        }

        private void bunifuButton6_Click(object sender, EventArgs e)
        {
            this.Hide();
            new LoginForm().Show();
        }

        private void PetugasForm_Load(object sender, EventArgs e)
        {
            hideMenu();
            getDataUser();
        }
    }
}
