using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prrbitirme
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-9PJ75G5\SQLEXPRESS;Initial Catalog=bitirmepr;Integrated Security=True");
        private void UrunlerListele()
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Urunler", baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.Columns.Clear();
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Listeleme hatası: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }

        private void AramaYap()
        {
            try
            {
                string arananurun = textBox1.Text.Trim();

                if (string.IsNullOrEmpty(arananurun))
                {
                    UrunlerListele();
                    return;
                }

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Urunler WHERE UrunAdi LIKE @aranan", baglanti);
                da.SelectCommand.Parameters.AddWithValue("@aranan", "%" + arananurun + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama hatası: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            UrunlerListele();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            AramaYap();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("ID bilgisi girmenize gerek yok! Ürün otomatik ID alacaktır." +
                    "Boş bırakarak devam ediniz.",
                                "Uyarı",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                textBox3.Clear(); 
                textBox3.Focus(); 
                return;
            }
            if (
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                SqlCommand komut = new SqlCommand("INSERT INTO Urunler (UrunAdi, Fiyat, Stok) VALUES (@UrunAdi, @Fiyat, @Stok)", baglanti);
                komut.Parameters.AddWithValue("@UrunAdi", textBox4.Text);
                komut.Parameters.AddWithValue("@Fiyat", Convert.ToDecimal(textBox5.Text));
                komut.Parameters.AddWithValue("@Stok", Convert.ToInt32(textBox6.Text));

                komut.ExecuteNonQuery();
                MessageBox.Show("Ürün başarıyla eklendi!");

                UrunlerListele();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ekleme hatası: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                int silinecekId = Convert.ToInt32(textBox2.Text);
                SqlCommand komut = new SqlCommand("DELETE FROM Urunler WHERE Id = @Id", baglanti);
                komut.Parameters.AddWithValue("@Id", silinecekId);

                int sonuc = komut.ExecuteNonQuery();

                if (sonuc > 0)
                {
                    MessageBox.Show("Ürün başarıyla silindi.");
                    UrunlerListele();
                    textBox2.Clear();
                }
                else
                {
                    MessageBox.Show("Belirtilen ID ile ürün bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme hatası: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                baglanti.Open();

                SqlCommand komut = new SqlCommand("UPDATE Urunler SET UrunAdi = @UrunAdi, Fiyat = @Fiyat, Stok = @Stok WHERE Id = @Id", baglanti);
                komut.Parameters.AddWithValue("@UrunAdi", textBox4.Text);
                komut.Parameters.AddWithValue("@Fiyat", Convert.ToDecimal(textBox5.Text));
                komut.Parameters.AddWithValue("@Stok", Convert.ToInt32(textBox6.Text));
                komut.Parameters.AddWithValue("@Id", Convert.ToInt32(textBox3.Text));

                int sonuc = komut.ExecuteNonQuery();

                if (sonuc > 0)
                {
                    MessageBox.Show("Güncelleme başarılı!");
                    textBox3.Clear();
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();

                }
                else
                {
                    MessageBox.Show("Belirtilen ID'ye sahip ürün bulunamadı.");
                }

                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Urunler", baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme hatası: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5 frm5 = new Form5();
            frm5.Show();
            this.Hide();
        }
    }
  }

