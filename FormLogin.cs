using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FinalProject
{
    public partial class FormLogin : Form
    {
        private MySqlConnection koneksi;
        private string alamat;
        private DataSet ds;
        private MySqlCommand perintah;
        private MySqlDataAdapter adapter;
        public FormLogin()
        {
            alamat = "server=localhost;database=db_carrent;username=root;password=;";
            koneksi = new MySqlConnection(alamat);
            ds = new DataSet();

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string query = string.Format("select * from tb_user where pass = '{0}'", textBox1.Text);
                ds.Clear();
                koneksi.Open();
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);
                perintah.ExecuteNonQuery();
                adapter.Fill(ds);
                koneksi.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Form1 form1 = new Form1();
                    form1.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Password tidak ditemukan");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
