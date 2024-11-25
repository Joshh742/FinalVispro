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
    public partial class FrmUpdate : Form
    {
        private MySqlConnection koneksi;
        private string alamat;
        private MySqlCommand perintah;
        private MySqlDataAdapter adapter;
        private DateTime startDate;
        private double hargaPerHari;

        public FrmUpdate()
        {
            InitializeComponent();
            alamat = "server=localhost;database=db_carrent;username=root;password=;";
            koneksi = new MySqlConnection(alamat);
        }

        private void LoadInitialData(string noKtp)
        {
            try
            {
                koneksi.Open();
                string query = "SELECT Tanggal_ambil, Pembayaran, Tanggal_akhir FROM tb_transaksi WHERE No_Ktp = @NoKtp";
                perintah = new MySqlCommand(query, koneksi);
                perintah.Parameters.AddWithValue("@NoKtp", noKtp);

                using (var reader = perintah.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        startDate = reader.GetDateTime("Tanggal_ambil");
                        double totalPembayaran = reader.GetDouble("Pembayaran");
                        DateTime endDate = reader.GetDateTime("Tanggal_akhir");
                        int totalHariSewa = (endDate - startDate).Days;

                        
                        if (totalHariSewa > 0)
                        {
                            hargaPerHari = totalPembayaran / totalHariSewa;
                        }
                        else
                        {
                            hargaPerHari = 0; 
                        }
                    }
                }
                koneksi.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    LoadInitialData(textBox1.Text); 
                    DateTime endDate = dateTimePicker1.Value; 
                    int totalHari = (endDate - startDate).Days;

                    
                    if (hargaPerHari == 0 || totalHari <= 0)
                    {
                        MessageBox.Show("Harga per hari tidak valid atau total hari tidak valid. Periksa data transaksi.");
                        return;
                    }

                    double totalHarga = totalHari * hargaPerHari;

                    string query = string.Format("UPDATE tb_transaksi SET Tanggal_akhir = '{0}', Pembayaran = '{1}' WHERE No_Ktp = '{2}'",
                                                  endDate.ToString("yyyy-MM-dd"), totalHarga, textBox1.Text);

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);
                    adapter = new MySqlDataAdapter(perintah);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Update Data Sukses" +
                            "...");
                        FrmDaftarTransaksi frmDaftarTransaksi = new FrmDaftarTransaksi();
                        frmDaftarTransaksi.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Gagal Update Data . . . ");
                    }
                }
                else
                {
                    MessageBox.Show("Data Tidak lengkap !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FrmUpdate_Load(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {
            FrmDaftarTransaksi frmDaftarTransaksi = new FrmDaftarTransaksi();
            frmDaftarTransaksi.Show();
            this.Hide();
        }
    }
}
