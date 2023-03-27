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
    public partial class ManagePetugasForm : Form
    {
        private MySqlCommand cmd, cmd2;
        private MySqlDataAdapter da;
        private MySqlDataReader rd;
        private DataSet ds;
        

        koneksi koneksi = new koneksi();
        public ManagePetugasForm()
        {
            InitializeComponent();
        }

        private void ManagePetugasForm_Load(object sender, EventArgs e)
        {
            bersih();
        }



        private void bersih()
        {
            txt1.Text = "";
            txtIdLevel.Text = "";
            txt3.Text = "";
            txt2.Text = "";
            txt5.Text = "";
            txt4.Text = "";
            cmb1.Text = "Pilih Level";
            incrementidPetugas();
            incrementidLevel();
            tampildata();
        }

        //=================================================== manual increment =======================================================


        private void incrementidPetugas()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            cmd = new MySqlCommand("select id_petugas from petugas order by id_petugas desc", conn);
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

        //=================================================== Crud =======================================================


        private void tampildata()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("SELECT petugas.id_petugas, petugas.nama_petugas,petugas.jenis_kelamin, level_user.username, level_user.password, level_user.level_user, level_user.id_user FROM petugas LEFT JOIN level_user ON petugas.id_user = level_user.id_user", conn);
                ds = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(ds, "petugas");
                dataGridPetugas.DataSource = ds;
                dataGridPetugas.DataMember = "petugas";
            }
            catch(Exception)
            {
                MessageBox.Show("Gagal mendapat data petugas");
            }
            conn.Close();
        }

        private void insertdata()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("insert into petugas values('" +
                           txt1.Text + "','" +
                           txt2.Text + "','" +
                           txt3.Text + "','" +
                           txtIdLevel.Text + "')", conn);
                cmd2 = new MySqlCommand("insert into level_user values('" +
                           txtIdLevel.Text + "','" +
                           txt4.Text + "','" +
                           txt5.Text + "','" +
                           cmb1.Text + "')", conn);
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
                cmd = new MySqlCommand("UPDATE petugas SET nama_petugas='" +
                    txt2.Text + "',jenis_kelamin='" +
                    txt3.Text + "'where id_petugas='" +
                    txt1.Text + "'", conn);
                cmd2 = new MySqlCommand("UPDATE level_user SET username='" +
                    txt4.Text + "',password='" +
                    txt5.Text + "',level_user='" +
                    cmb1.Text + "'where id_user='" +
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

        private void delete()
        {
            MySqlConnection conn = koneksi.GetKon();
            conn.Open();
            try
            {
                cmd = new MySqlCommand("delete from petugas where id_petugas='" + txt1.Text + "'", conn);
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

        //=================================================== click event =======================================================

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
            txt4.Text = row.Cells[3].Value.ToString();
            txt5.Text = row.Cells[4].Value.ToString();
            cmb1.Text = row.Cells[5].Value.ToString();
            txtIdLevel.Text = row.Cells[6].Value.ToString();
        }

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            delete();
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            updateData();
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

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            insertdata();
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Data.SqlClient;

//namespace SMK_RESTAURANT
//{
//    public partial class Order : Form
//    {

//        private DataSet ds;
//        private SqlCommand cmd;
//        private SqlDataAdapter da;
//        private SqlDataReader rd;
//        string id;
//        string format;

//        Koneksi koneksi = new Koneksi();

//        public Order()
//        {
//            InitializeComponent();


//        }
//        void bersih()
//        {
//            textBox1.Text = "";
//            textBox2.Text = "";
//            textBox8.Text = "";
//            textBox9.Text = "";
//            txtId.Text = "";
//            txtMenuId.Text = "";
//            txtOrderId.Text = "";
//            txt.Text = "";
//            txtStatus.Text = "";

//        }

//        //make auto increment id with date format yyyymmdd and auto increment number 0001
//        //private string Autoid()
//        // {
//        //try
//        //{
//        //    SqlConnection conn = koneksi.getKon();
//        //    conn.Open();
//        //    cmd = new SqlCommand("select OrderId from MsMenu order by OrderId desc", conn);
//        //    rd = cmd.ExecuteReader();
//        //    rd.Read();
//        //    if (rd.HasRows)
//        //    {
//        //        string id = rd["OrderId"].ToString();
//        //        string angka = id.Substring(8, 4);
//        //        int num = Convert.ToInt32(angka) + 1;
//        //        string result = num.ToString();
//        //        if (result.Length == 1)
//        //        {
//        //            result = "000" + result;
//        //        }
//        //        else if (result.Length == 2)
//        //        {
//        //            result = "00" + result;
//        //        }
//        //        else if (result.Length == 3)
//        //        {
//        //            result = "0" + result;
//        //        }
//        //        else if (result.Length == 4)
//        //        {
//        //            result = "" + result;
//        //        }
//        //        string tanggal = DateTime.Now.ToString("yyyyMMdd");
//        //        string result2 = tanggal + result;
//        //        return result2;
//        //    }
//        //    else
//        //    {
//        //        string result = "0001";
//        //        string tanggal = DateTime.Now.ToString("yyyyMMdd");
//        //        string result2 = tanggal + result;
//        //        return result2;
//        //    }
//        //}
//        //catch (Exception ex)
//        //{
//        //    MessageBox.Show(ex.Message);
//        //}
//        //return "";

//        //string id = "";
//        //string format = "yyyyMMdd";
//        //DateTime now = DateTime.Now;
//        //id = now.ToString(format);
//        //id = id + "0001";
//        //return id;
//        //}

//        //make auto increment id with date format mmdd and auto increment number 0001
//        private string Autoid()
//        {
//            try
//            {
//                SqlConnection conn = koneksi.getKon();
//                conn.Open();
//                cmd = new SqlCommand("select OrderId from OrderDetail order by OrderId desc", conn);
//                rd = cmd.ExecuteReader();
//                rd.Read();
//                if (rd.HasRows)
//                {
//                    string id = rd["OrderId"].ToString();
//                    string angka = id.Substring(4, 4);
//                    int num = Convert.ToInt32(angka) + 1;
//                    string result = num.ToString();
//                    if (result.Length == 1)
//                    {
//                        result = "000" + result;
//                    }
//                    else if (result.Length == 2)
//                    {
//                        result = "00" + result;
//                    }
//                    else if (result.Length == 3)
//                    {
//                        result = "0" + result;
//                    }
//                    else if (result.Length == 4)
//                    {
//                        result = "" + result;
//                    }
//                    string tanggal = DateTime.Now.ToString("MMdd");
//                    string result2 = tanggal + result;
//                    return result2;
//                }
//                else
//                {
//                    string result = "0001";
//                    string tanggal = DateTime.Now.ToString("MMdd");
//                    string result2 = tanggal + result;
//                    return result2;
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//            return "";
//        }

//        //void sutoid()
//        //{
//        //    string id = "";
//        //    string format = "MMdd";
//        //    DateTime now = DateTime.Now;
//        //    id = now.ToString(format);
//        //    id = id + "001";
//        //    SqlConnection conn = koneksi.getKon();
//        //    conn.Open();
//        //    cmd = new SqlCommand("select Id from MsMenu order by Id desc", conn);
//        //    rd = cmd.ExecuteReader();
//        //    rd.Read();
//        //    if (rd.HasRows)
//        //    {
//        //        txtOrderId.Text = id + (Convert.ToInt64(rd[0].ToString()) + 001).ToString();
//        //    }
//        //    else
//        //    {
//        //        txtOrderId.Text = "001";
//        //    }
//        //    rd.Close();
//        //    conn.Close();
//        //}



//        //private string autoid()
//        //{

//        //    SqlConnection conn = koneksi.getKon();
//        //    conn.Open();
//        //    cmd = new SqlCommand("select IdOrder OrderDetaile where IdOrder order by IdOrder desc");
//        //    string id = "";
//        //    string format = "yyyyMMdd";
//        //    DateTime now = DateTime.Now;
//        //    id = now.ToString(format) ;


//        //    return id;
//        //}

//        void jumlah()
//        {
//            int jumlah = 0;
//            //sum between textbox2 and textbox3
//            for (int i = 0; i < dataGridView1.Rows.Count; i++)
//            {
//                jumlah += Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value.ToString());
//            }
//            //convert jumlah to string

//        }

//        void tampildata()
//        {
//            SqlConnection conn = koneksi.getKon();
//            conn.Open();
//            cmd = new SqlCommand("select * from MsMenu", conn);
//            ds = new DataSet();
//            da = new SqlDataAdapter(cmd);
//            da.Fill(ds, "MsMenu");
//            dataGridView1.DataSource = ds;
//            dataGridView1.DataMember = "MsMenu";
//        }

//        void tampildataOrder()
//        {
//            try
//            {
//                SqlConnection conn = koneksi.getKon();
//                conn.Open();
//                cmd = new SqlCommand("select MenuId,Name,Qty,Price,Carbo,Protein from MsMenu,OrderDetaile where MsMenu.Id = OrderDetaile.Id", conn);
//                ds = new DataSet();
//                da = new SqlDataAdapter(cmd);
//                da.Fill(ds, "MsMenu");
//                dataGridView2.DataSource = ds;
//                dataGridView2.DataMember = "MsMenu";
//            }
//            catch (Exception g)
//            {
//                MessageBox.Show(g.ToString());
//            }
//        }

//        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
//        {
//            DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
//            //textBox9.Text = row.Cells[0].Value.ToString();
//            textBox8.Text = row.Cells["Price"].Value.ToString();
//            textBox1.Text = row.Cells["Name"].Value.ToString();
//            //txtOrderId.Text = row.Cells["Id"].Value.ToString();
//            txtId.Text = row.Cells["Id"].Value.ToString();
//            txtMenuId.Text = row.Cells["Id"].Value.ToString();
//            pictureBox1.ImageLocation = row.Cells["Photo"].Value.ToString();


//            //scretch image 
//            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

//        }

//        private void Form1_Load(object sender, EventArgs e)
//        {
//            //sutoid();
//            tampildata();
//            tampildataOrder();
//            txtOrderId.Text = Autoid();

//            //hide photo and id column
//            //dataGridView1.Columns[3].Visible = false;
//            //dataGridView1.Columns[0].Visible = false;
//            //txtId.Visible = false;
//            //txtMenuId.Visible = false;
//            ////txtOrderId.Visible = false;
//            //txtStatus.Visible = false;
//            //txt.Visible = false;
//            //textBox8.Visible = false;
//            //textBox9.Visible = false;

//            //make extra column for data gridview name total
//            dataGridView2.Columns.Add("Total", "Total");
//            //makw a total for price and qty
//            for (int i = 0; i < dataGridView2.Rows.Count; i++)
//            {
//                dataGridView2.Rows[i].Cells[6].Value = Convert.ToInt32(dataGridView2.Rows[i].Cells["Price"].Value) * Convert.ToInt32(dataGridView2.Rows[i].Cells["Qty"].Value);
//            }


//            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
//            //{
//            //    dataGridView2.Rows[i].Cells["Total"].Value = Convert.ToInt32(dataGridView2.Rows[i].Cells["Price"].Value) * Convert.ToInt32(dataGridView2.Rows[i].Cells["Qty"].Value);
//            //}

//            //txtOrderId.Text = autoid();

//            //get total price from data gridview2 to label6
//            int total = 0;
//            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
//            {
//                total += Convert.ToInt32(dataGridView2.Rows[i].Cells["Total"].Value);
//            }
//            label6.Text = total.ToString();

//            //make sum of qty and carbo show to labelcarbo
//            int carbo = 0;
//            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
//            {
//                carbo += Convert.ToInt32(dataGridView1.Rows[i].Cells["Carbo"].Value) * Convert.ToInt32(dataGridView2.Rows[i].Cells["Qty"].Value);
//            }
//            labelcarbo.Text = carbo.ToString();

//            //make sum of qty and protein show to labelprotein



//            int protein = 0;
//            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
//            {
//                protein += Convert.ToInt32(dataGridView1.Rows[i].Cells["Protein"].Value) * Convert.ToInt32(dataGridView2.Rows[i].Cells["Qty"].Value);
//            }
//            totprot.Text = protein.ToString();



//        }

//        private void button1_Click(object sender, EventArgs e)
//        {
//            if (textBox2.Text.Trim() == "")
//            {
//                MessageBox.Show("data tidak boleh kosong");

//            }
//            else
//            {
//                try
//                {
//                    //insert data to OrderDetaile table MsMenu
//                    SqlConnection conn = koneksi.getKon();
//                    conn.Open();
//                    cmd = new SqlCommand("insert into OrderDetaile (Id, OrderId, MenuId, Qty, Status) values('"
//                        + txtId.Text + "','"
//                        + txtOrderId.Text + "','"
//                        + txtMenuId.Text + "','"
//                        + textBox2.Text + "','"
//                        + txtStatus.Text + "')", conn);
//                    cmd.ExecuteNonQuery();
//                    conn.Close();


//                }
//                catch (Exception x)
//                {
//                    MessageBox.Show(x.ToString());
//                }
//                //int jumlah = 0;
//                ////sum between textbox2 and textbox3 and show in datagridview jumlah
//                //for (int i = 0; i < dataGridView1.Rows.Count; i++)
//                //{
//                //    jumlah += Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
//                //}
//                //txtOrderId.Text = jumlah.ToString();
//                ////send textbox4 to datagridview jumlah

//                tampildataOrder();
//                bersih();
//            }


//        }

//        private void button3_Click(object sender, EventArgs e)
//        {
//            new FormPayment().Show();
//            this.Hide();
//        }

//        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
//        {
//            DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
//            txtId.Text = row.Cells["MenuId"].Value.ToString();
//            textBox1.Text = row.Cells["Name"].Value.ToString();




//        }

//        private void button2_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                //insert data to OrderDetaile table MsMenu
//                SqlConnection conn = koneksi.getKon();
//                conn.Open();
//                cmd = new SqlCommand("Delete OrderDetaile where MenuId='"
//                    + txtId.Text + "'", conn);
//                cmd.ExecuteNonQuery();
//                conn.Close();

//            }
//            catch (Exception x)
//            {
//                MessageBox.Show(x.ToString());
//            }


//            tampildataOrder();
//            bersih();

//        }

//        private void button4_Click(object sender, EventArgs e)
//        {

//        }

//        private void label3_Click(object sender, EventArgs e)
//        {

//        }

//        private void button5_Click(object sender, EventArgs e)
//        {
//            bersih();
//        }
//    }
//}
