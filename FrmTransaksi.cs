using FinalProject;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class FrmTransaksi : Form
    {
        
        public string CarName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalHarga { get; set; }

        private readonly MySqlConnection koneksi;
        private string alamat, query;

        public FrmTransaksi()
        {
            alamat = "server=localhost;database=db_carrent;username=root;password=;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
            button1.Click += new EventHandler(button1_Click); 
        }

        private void FrmTransaksi_Load(object sender, EventArgs e)
        {
            textBox5.Text = CarName; 
            textBox6.Text = StartDate.ToString("dd-MM-yyyy"); 
            textBox7.Text = EndDate.ToString("dd-MM-yyyy"); 
            textBox8.Text = TotalHarga.ToString("C"); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsInputValid())
                {
                    MessageBox.Show("Input berhasil. Melanjutkan penyimpanan data...");

                    query = "INSERT INTO tb_transaksi (Penyewa, No_Ktp, No_hp, Tanggal_ambil, Tanggal_akhir, Pembayaran, Nama_mobil) " +
                            "VALUES (@NamaPenyewa, @NomorKtp, @NomorHp, @TanggalDiambil, @TanggalDikembalikan, @Pembayaran, @NamaMobil)";

                    using (MySqlCommand perintah = new MySqlCommand(query, koneksi))
                    {
                        
                        koneksi.Open();

                        
                        perintah.Parameters.AddWithValue("@NamaPenyewa", textBox1.Text);
                        perintah.Parameters.AddWithValue("@NomorKtp", textBox2.Text);
                        perintah.Parameters.AddWithValue("@NomorHp", textBox3.Text);
                        perintah.Parameters.AddWithValue("@TanggalDiambil", StartDate.ToString("yyyy-MM-dd"));
                        perintah.Parameters.AddWithValue("@TanggalDikembalikan", EndDate.ToString("yyyy-MM-dd"));
                        perintah.Parameters.AddWithValue("@Pembayaran", TotalHarga);
                        perintah.Parameters.AddWithValue("@NamaMobil", CarName);

                        
                        perintah.ExecuteNonQuery();
                    }

                    
                    MessageBox.Show("Data berhasil disimpan.");

                    
                    FrmDaftarTransaksi frmDaftarTransaksi = new FrmDaftarTransaksi(); 
                    frmDaftarTransaksi.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Silakan isi semua data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
            finally
            {
                
                if (koneksi.State == System.Data.ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            FrmDaftarTransaksi frmDaftarTransaksi = new FrmDaftarTransaksi(); 
            frmDaftarTransaksi.Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        
        private bool IsInputValid()
        {
            if (string.IsNullOrEmpty(textBox1.Text) ||
                string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text))
            {
                return false;
            }

            
            if (textBox2.Text.Length != 16 || !long.TryParse(textBox2.Text, out _))
            {
                MessageBox.Show("Nomor KTP tidak valid.");
                return false;
            }

            
            if (textBox3.Text.Length < 10 || !long.TryParse(textBox3.Text, out _))
            {
                MessageBox.Show("Nomor HP tidak valid.");
                return false;
            }

            return true;
        }
    }
}
