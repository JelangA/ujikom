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
    public partial class LoginForm : Form
    {

        private MySqlCommand cmd;
        private MySqlDataReader rd;

        koneksi koneksi = new koneksi();

        public static string UserID;

        public LoginForm()
        {
            InitializeComponent();
       
        }

        void login()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("select * from level_user where username='" + bunifuTextBox1.Text + "' and password='" + bunifuTextBox2.Text + "'", conn);
                rd = cmd.ExecuteReader();
                rd.Read();

                if (rd.HasRows)
                {
                    if (rd[3].ToString() == "admin")
                    {
                        this.Hide();
                        UserID = rd[0].ToString();
                        new AdminForm().Show();
                    }
                    else if (rd[3].ToString() == "petugas")
                    {
                        this.Hide();
                        UserID = rd[0].ToString();
                        new PetugasForm().Show();
                    }
                    else if (rd[3].ToString() == "siswa")
                    {
                        this.Hide();
                        UserID = rd[0].ToString();
                        new SiswaForm().Show();
                    }
                    else
                    {
                        MessageBox.Show("username atau password");
                    }

                }
                else
                {
                    MessageBox.Show("Username atau password salah");
                }
                rd.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("gagal");
            }
            conn.Close();
        }


        private void bersih()
        {
            bunifuTextBox1.Text = "";
            bunifuTextBox2.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel.BackColor = Color.FromArgb(50, 0, 0, 0);

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            login();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            bersih();
        }

    }
}
