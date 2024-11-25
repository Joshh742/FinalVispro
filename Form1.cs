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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem.ToString();
            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;
            int totalHari = (endDate - startDate).Days;

            var alamat = "server=localhost;database=db_carrent;username=root;password=;";
            var koneksi = new MySqlConnection(alamat);
            bool stockAvailable = false;
            double hargaPerHari = 0;
            double totalHarga = 0;
            int stok = 0;

            try
            {
                koneksi.Open();
                string query = "SELECT stok, Harga FROM tb_mobil WHERE nama = @itemName";
                var cmd = new MySqlCommand(query, koneksi);
                cmd.Parameters.AddWithValue("@itemName", selectedItem);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        stok = reader.GetInt32("stok");
                        hargaPerHari = reader.GetDouble("Harga");

                        if (stok > 0)
                        {
                            stockAvailable = true;
                            totalHarga = totalHari * hargaPerHari;

                            // Decrement the stock
                            stok--;
                        }
                    }
                }

                // Update the stock in the database
                if (stockAvailable)
                {
                    query = "UPDATE tb_mobil SET stok = @newStock WHERE nama = @itemName";
                    cmd = new MySqlCommand(query, koneksi);
                    cmd.Parameters.AddWithValue("@newStock", stok);
                    cmd.Parameters.AddWithValue("@itemName", selectedItem);
                    cmd.ExecuteNonQuery();
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

            if (stockAvailable)
            {
                FrmTransaksi frmTransaksi = new FrmTransaksi();
                frmTransaksi.CarName = selectedItem;
                frmTransaksi.StartDate = startDate;
                frmTransaksi.EndDate = endDate;
                frmTransaksi.TotalHarga = totalHarga;
                frmTransaksi.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Stok tidak tersedia.");
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            FrmDaftarTransaksi frmDaftarTransaksi = new FrmDaftarTransaksi();
            frmDaftarTransaksi.Show();
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCarStock();
        }

        private void LoadCarStock()
        {
            var alamat = "server=localhost;database=db_carrent;username=root;password=;";
            var koneksi = new MySqlConnection(alamat);

            try
            {
                koneksi.Open();
                string query = "SELECT id_mobil, nama, stok, Harga FROM tb_mobil";
                var cmd = new MySqlCommand(query, koneksi);
                var adapter = new MySqlDataAdapter(cmd);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridViewstok.DataSource = dataTable; // Updated to use dataGridViewstok
                koneksi.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
