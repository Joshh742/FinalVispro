using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace FinalProject
{
    public partial class FrmDaftarTransaksi : Form
    {
        private MySqlConnection koneksi;
        private string alamat;
        private DataSet ds;
        private MySqlCommand perintah;
        private MySqlDataAdapter adapter;

        public FrmDaftarTransaksi()
        {
            InitializeComponent();
            alamat = "server=localhost;database=db_carrent;username=root;password=;";
            koneksi = new MySqlConnection(alamat);
            ds = new DataSet();
        }

        private void FrmDaftarTransaksi_Load(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();
                string query = "SELECT Penyewa, No_Ktp, No_hp, Tanggal_ambil, Tanggal_akhir, Pembayaran, Nama_mobil FROM tb_transaksi";
                MySqlCommand cmd = new MySqlCommand(query, koneksi);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                koneksi.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    string query = string.Format("SELECT * FROM tb_transaksi WHERE No_Ktp = '{0}'", textBox1.Text);
                    ds.Clear();
                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);
                    adapter = new MySqlDataAdapter(perintah);
                    perintah.ExecuteNonQuery();
                    adapter.Fill(ds);
                    koneksi.Close();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow kolom in ds.Tables[0].Rows)
                        {
                            textBox1.Text = kolom["Penyewa"].ToString();
                        }
                        textBox1.Enabled = false;
                        dataGridView1.DataSource = ds.Tables[0];
                    }
                    else
                    {
                        MessageBox.Show("Data Tidak Ada!!");
                        FrmDaftarTransaksi_Load(null, null);
                    }
                }
                else
                {
                    MessageBox.Show("Data Yang Anda Pilih Tidak Ada!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
               
                textBox1.Enabled = true;
                textBox1.Clear();

                
                dataGridView1.DataSource = null;

                
                FrmDaftarTransaksi_Load(null, null);

                
                button1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmPengembalian frmPengembalian = new FrmPengembalian();
            frmPengembalian.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmUpdate frmUpdate = new FrmUpdate();
            frmUpdate.Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            FrmTransaksi frmTransaksi = new FrmTransaksi();
            frmTransaksi.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Formtransaksi formtransaksi = new Formtransaksi();
            formtransaksi.Show();
        }
    }
}
