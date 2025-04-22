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
    public partial class Form4 : Form
    {
        public Form4()
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

                string sorgu = @"SELECT s.Id AS SepetId, 
                                       k.KullaniciAdi, 
                                       u.UrunAdi, 
                                       u.Fiyat, 
                                       s.Adet, 
                                       (u.Fiyat * s.Adet) AS ToplamFiyat
                                FROM Sepet s
                                INNER JOIN Kullanicilar k ON s.KullaniciId = k.Id
                                INNER JOIN Urunler u ON s.UrunId = u.Id
                                WHERE s.KullaniciId = @kullaniciId";

                SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                da.SelectCommand.Parameters.AddWithValue("@kullaniciId", Form1.GirisYapanKullaniciId);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.Columns.Clear();
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["SepetId"].HeaderText = "Sepet No";
                    dataGridView1.Columns["KullaniciAdi"].HeaderText = "Kullanıcı";
                    dataGridView1.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                    dataGridView1.Columns["Fiyat"].HeaderText = "Birim Fiyat";
                    dataGridView1.Columns["Adet"].HeaderText = "Adet";
                    dataGridView1.Columns["ToplamFiyat"].HeaderText = "Toplam Fiyat";
                }
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
        private int GetLastOrderId(SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT MAX(Id) FROM Siparisler", connection))
            {
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            UrunleriListele();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 frm5 = new Form3();
            frm5.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                using (SqlTransaction transaction = baglanti.BeginTransaction())
                {
                    try
                    {
                        SqlCommand toplamSorgu = new SqlCommand(
                            @"SELECT SUM(u.Fiyat * s.Adet) 
                      FROM Sepet s
                      INNER JOIN Urunler u ON s.UrunId = u.Id
                      WHERE s.KullaniciId = @kullaniciId",
                            baglanti, transaction);
                        toplamSorgu.Parameters.AddWithValue("@kullaniciId", Form1.GirisYapanKullaniciId);
                        object toplamResult = toplamSorgu.ExecuteScalar();
                        decimal toplamFiyat = toplamResult != DBNull.Value ? Convert.ToDecimal(toplamResult) : 0;

                        DialogResult result = MessageBox.Show(
                            $"Toplam Tutar: {toplamFiyat:C}\n\nSatın almayı onaylıyor musunuz?",
                            "Onay",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result != DialogResult.Yes)
                        {
                            transaction.Rollback();
                            MessageBox.Show("İşlem iptal edildi.");
                            return;
                        }

                        DataTable sepetUrunleri = new DataTable();
                        using (SqlCommand cmd = new SqlCommand(
                            "SELECT UrunId, Adet FROM Sepet WHERE KullaniciId = @kullaniciId",
                            baglanti, transaction))
                        {
                            cmd.Parameters.AddWithValue("@kullaniciId", Form1.GirisYapanKullaniciId);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                sepetUrunleri.Load(dr);
                            }
                        }

                        foreach (DataRow row in sepetUrunleri.Rows)
                        {
                            int urunId = Convert.ToInt32(row["UrunId"]);
                            int adet = Convert.ToInt32(row["Adet"]);

                            SqlCommand stokKontrol = new SqlCommand(
                                "SELECT Stok FROM Urunler WHERE Id = @urunId",
                                baglanti, transaction);
                            stokKontrol.Parameters.AddWithValue("@urunId", urunId);
                            int mevcutStok = Convert.ToInt32(stokKontrol.ExecuteScalar());

                            if (mevcutStok < adet)
                            {
                                transaction.Rollback();
                                MessageBox.Show($"ID: {urunId} - Yeterli stok yok! (Stok: {mevcutStok}, İstenen: {adet})");
                                return;
                            }

                            SqlCommand siparisEkle = new SqlCommand(
                                @"INSERT INTO Siparisler (KullaniciId, UrunId, Adet, ToplamFiyat) 
                          SELECT @kullaniciId, @urunId, @adet, (Fiyat * @adet) 
                          FROM Urunler WHERE Id = @urunId",
                                baglanti, transaction);
                            siparisEkle.Parameters.AddWithValue("@kullaniciId", Form1.GirisYapanKullaniciId);
                            siparisEkle.Parameters.AddWithValue("@urunId", urunId);
                            siparisEkle.Parameters.AddWithValue("@adet", adet);
                            siparisEkle.ExecuteNonQuery();

                            SqlCommand stokGuncelle = new SqlCommand(
                                "UPDATE Urunler SET Stok = Stok - @adet WHERE Id = @urunId",
                                baglanti, transaction);
                            stokGuncelle.Parameters.AddWithValue("@adet", adet);
                            stokGuncelle.Parameters.AddWithValue("@urunId", urunId);
                            stokGuncelle.ExecuteNonQuery();
                        }

                        SqlCommand sepetTemizle = new SqlCommand(
                            "DELETE FROM Sepet WHERE KullaniciId = @kullaniciId",
                            baglanti, transaction);
                        sepetTemizle.Parameters.AddWithValue("@kullaniciId", Form1.GirisYapanKullaniciId);
                        sepetTemizle.ExecuteNonQuery();

                        transaction.Commit();
                        MessageBox.Show($"Satın alma başarılı!\nSipariş No: {GetLastOrderId(baglanti)}\nToplam: {toplamFiyat:C}");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Hata: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sistem hatası: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
                UrunleriListele();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show("Lütfen silmek için bir ID giriniz.");
                    return;
                }

                int sepetId = Convert.ToInt32(textBox2.Text);

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                SqlCommand kontrolCmd = new SqlCommand("SELECT COUNT(*) FROM Sepet WHERE Id = @sepetId", baglanti);
                kontrolCmd.Parameters.AddWithValue("@sepetId", sepetId);
                int kayitSayisi = Convert.ToInt32(kontrolCmd.ExecuteScalar());

                if (kayitSayisi == 0)
                {
                    MessageBox.Show("Belirtilen ID'de sepet kaydı bulunamadı!");
                    return;
                }

                SqlCommand cmd = new SqlCommand("DELETE FROM Sepet WHERE Id = @sepetId", baglanti);
                cmd.Parameters.AddWithValue("@sepetId", sepetId);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Ürün sepetten silindi.");
                UrunleriListele();
                textBox2.Clear();
            }
            catch (FormatException)
            {
                MessageBox.Show("Geçersiz ID formatı! Lütfen sayısal bir değer girin.");
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Sepetinizdeki TÜM ürünler silinecek. Emin misiniz?",
                    "Sepeti Boşalt",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    MessageBox.Show("İşlem iptal edildi.");
                    return;
                }

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Sepet WHERE KullaniciId = @kullaniciId",
                    baglanti);
                cmd.Parameters.AddWithValue("@kullaniciId", Form1.GirisYapanKullaniciId);

                int silinenAdet = cmd.ExecuteNonQuery();

                MessageBox.Show($"{silinenAdet} adet ürün sepetten silindi.",
                              "Başarılı",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);

                UrunleriListele(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message,
                              "Hata",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }
    }
}
