using MySql.Data.MySqlClient;
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

namespace FinalProject
{
    public partial class FrmPengembalian : Form
    {
        private MySqlConnection koneksi;
        private string alamat;
        private DataSet ds;
        private MySqlCommand perintah;
        private MySqlDataAdapter adapter;

        public FrmPengembalian()
        {
            InitializeComponent();
            alamat = "server=localhost;database=db_carrent;username=root;password=;";
            koneksi = new MySqlConnection(alamat);
            ds = new DataSet();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void FrmPengembalian_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text)) 
                {
                    if (MessageBox.Show("Anda yakin pesanan telah selesai? ??", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        string query = string.Format("DELETE FROM tb_transaksi WHERE No_Ktp = '{0}'", textBox1.Text);
                        ds.Clear();
                        koneksi.Open();
                        perintah = new MySqlCommand(query, koneksi);
                        adapter = new MySqlDataAdapter(perintah);
                        int res = perintah.ExecuteNonQuery();
                        koneksi.Close();

                        if (res == 1)
                        {
                            MessageBox.Show("Pesanan telah selesai ...");

                            // Memperbarui stok mobil
                            UpdateCarStock();

                            // Beralih ke form berikutnya
                            FrmDaftarTransaksi frmDaftarTransaksi = new FrmDaftarTransaksi();
                            frmDaftarTransaksi.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Gagal menyelesaikan pesanan, silahkan coba lagi");
                        }
                    }
                    FrmPengembalian_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Data Yang Anda Pilih Tidak Ada !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UpdateCarStock()
        {
            try
            {
                koneksi.Open();
                string query = "UPDATE tb_mobil SET stok = stok + 1 WHERE stok = 0";
                perintah = new MySqlCommand(query, koneksi);
                int res = perintah.ExecuteNonQuery();
                koneksi.Close();

                if (res > 0)
                {
                    MessageBox.Show("Stok mobil telah diperbarui.");
                }
                else
                {
                    MessageBox.Show("Tidak ada stok mobil yang perlu diperbarui.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                koneksi.Close();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            FrmDaftarTransaksi frmDaftarTransaksi = new FrmDaftarTransaksi();
            frmDaftarTransaksi.Show();
            this.Hide();
        }
    }
}
