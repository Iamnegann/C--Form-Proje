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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-9PJ75G5\SQLEXPRESS;Initial Catalog=bitirmepr;Integrated Security=True");
        private void UrunleriListele()
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
                string arananUrun = textBox1.Text.Trim();

                if (string.IsNullOrEmpty(arananUrun))
                {
                    UrunleriListele();
                    return;
                }

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Urunler WHERE UrunAdi LIKE @aranan", baglanti);
                da.SelectCommand.Parameters.AddWithValue("@aranan", "%" + arananUrun + "%");

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

        private void Form3_Load(object sender, EventArgs e)
        {
            UrunleriListele();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            AramaYap();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int kullaniciId = Form1.GirisYapanKullaniciId;
                int urunId = Convert.ToInt32(textBox2.Text);
                int adet = Convert.ToInt32(textBox3.Text);

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();
             
                SqlCommand sepetteVarMi = new SqlCommand(
                    "SELECT Adet FROM Sepet WHERE KullaniciId=@kullaniciId AND UrunId=@urunId",
                    baglanti);
                sepetteVarMi.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                sepetteVarMi.Parameters.AddWithValue("@urunId", urunId);

                object result = sepetteVarMi.ExecuteScalar();

                if (result != null) 
                {
                    int mevcutAdet = Convert.ToInt32(result);
                   
                    SqlCommand stokKontrol = new SqlCommand(
                        "SELECT Stok FROM Urunler WHERE Id=@urunId", baglanti);
                    stokKontrol.Parameters.AddWithValue("@urunId", urunId);
                    int mevcutStok = Convert.ToInt32(stokKontrol.ExecuteScalar());

                    if (mevcutStok >= (mevcutAdet + adet))
                    {
                        SqlCommand guncelle = new SqlCommand(
                            "UPDATE Sepet SET Adet=Adet+@adet WHERE KullaniciId=@kullaniciId AND UrunId=@urunId",
                            baglanti);
                        guncelle.Parameters.AddWithValue("@adet", adet);
                        guncelle.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                        guncelle.Parameters.AddWithValue("@urunId", urunId);
                        guncelle.ExecuteNonQuery();

                        MessageBox.Show($"Ürün adeti güncellendi! Yeni adet: {mevcutAdet + adet}");
                    }
                    else
                    {
                        MessageBox.Show($"Stok yetersiz! Mevcut stok: {mevcutStok}, Sepetteki: {mevcutAdet}, Eklenmek istenen: {adet}");
                    }
                }
                else 
                {                    
                    SqlCommand stokKontrol = new SqlCommand(
                        "SELECT Stok FROM Urunler WHERE Id=@urunId", baglanti);
                    stokKontrol.Parameters.AddWithValue("@urunId", urunId);
                    int mevcutStok = Convert.ToInt32(stokKontrol.ExecuteScalar());

                    if (mevcutStok >= adet)
                    {
                        SqlCommand ekle = new SqlCommand(
                            "INSERT INTO Sepet (KullaniciId, UrunId, Adet) VALUES (@kullaniciId, @urunId, @adet)",
                            baglanti);
                        ekle.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                        ekle.Parameters.AddWithValue("@urunId", urunId);
                        ekle.Parameters.AddWithValue("@adet", adet);
                        ekle.ExecuteNonQuery();

                        MessageBox.Show("Ürün sepete eklendi!");
                        textBox2.Clear();
                        textBox3.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Aradığınız Ürün Yok veya Yeterli stok bulunmamaktadır!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 frm5 = new Form4();
            frm5.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm5 = new Form1();
            frm5.Show();
            this.Hide();
        }
    }
}
