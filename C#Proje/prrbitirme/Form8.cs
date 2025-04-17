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
using System.IO;
namespace prrbitirme
{
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-9PJ75G5\SQLEXPRESS;Initial Catalog=bitirmepr;Integrated Security=True");

        private void listeleme()
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                string sorgu = @"SELECT 
                            s.Id AS SiparisNo,
                            u.UrunAdi,
                            s.Adet,
                            s.ToplamFiyat,
                            s.SiparisTarihi,
                            k.KullaniciAdi
                         FROM Siparisler s
                         INNER JOIN Urunler u ON s.UrunId = u.Id
                         INNER JOIN Kullanicilar k ON s.KullaniciId = k.Id
                         ORDER BY s.SiparisTarihi DESC";

                SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["SiparisNo"].HeaderText = "Sipariş No";
                    dataGridView1.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                    dataGridView1.Columns["Adet"].HeaderText = "Adet";
                    dataGridView1.Columns["ToplamFiyat"].HeaderText = "Toplam Fiyat";
                    dataGridView1.Columns["SiparisTarihi"].HeaderText = "Sipariş Tarihi";
                    dataGridView1.Columns["KullaniciAdi"].HeaderText = "Kullanıcı";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sipariş geçmişi yüklenirken hata: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }

        private void KullaniciyaGoreAra(string kullaniciAdi)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                string sorgu = @"SELECT 
                    s.Id AS SiparisNo,
                    u.UrunAdi,
                    s.Adet,
                    s.ToplamFiyat,
                    s.SiparisTarihi,
                    k.KullaniciAdi
                 FROM Siparisler s
                 INNER JOIN Urunler u ON s.UrunId = u.Id
                 INNER JOIN Kullanicilar k ON s.KullaniciId = k.Id
                 WHERE k.KullaniciAdi LIKE @kullaniciAdi
                 ORDER BY s.SiparisTarihi DESC";

                SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                da.SelectCommand.Parameters.AddWithValue("@kullaniciAdi", "%" + kullaniciAdi + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["SiparisNo"].HeaderText = "Sipariş No";
                    dataGridView1.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                    dataGridView1.Columns["Adet"].HeaderText = "Adet";
                    dataGridView1.Columns["ToplamFiyat"].HeaderText = "Toplam Fiyat";
                    dataGridView1.Columns["SiparisTarihi"].HeaderText = "Sipariş Tarihi";
                    dataGridView1.Columns["KullaniciAdi"].HeaderText = "Kullanıcı";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kullanıcı adına göre arama yaparken hata: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }
        private void TariheGoreAra(string tarih)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                string sorgu = @"SELECT 
                    s.Id AS SiparisNo,
                    u.UrunAdi,
                    s.Adet,
                    s.ToplamFiyat,
                    s.SiparisTarihi,
                    k.KullaniciAdi
                 FROM Siparisler s
                 INNER JOIN Urunler u ON s.UrunId = u.Id
                 INNER JOIN Kullanicilar k ON s.KullaniciId = k.Id
                 WHERE CONVERT(VARCHAR, s.SiparisTarihi, 104) LIKE @tarih
                 ORDER BY s.SiparisTarihi DESC";

                SqlDataAdapter da = new SqlDataAdapter(sorgu, baglanti);
                da.SelectCommand.Parameters.AddWithValue("@tarih", "%" + tarih + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["SiparisNo"].HeaderText = "Sipariş No";
                    dataGridView1.Columns["UrunAdi"].HeaderText = "Ürün Adı";
                    dataGridView1.Columns["Adet"].HeaderText = "Adet";
                    dataGridView1.Columns["ToplamFiyat"].HeaderText = "Toplam Fiyat";
                    dataGridView1.Columns["SiparisTarihi"].HeaderText = "Sipariş Tarihi";
                    dataGridView1.Columns["KullaniciAdi"].HeaderText = "Kullanıcı";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tarih araması yaparken hata: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
        }
        private void Form8_Load(object sender, EventArgs e)
        {
            listeleme();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            KullaniciyaGoreAra(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TariheGoreAra(textBox2.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5 frm5 = new Form5();
            frm5.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV dosyası (*.csv)|*.csv";
                saveFileDialog.Title = "CSV Olarak Kaydet";
                saveFileDialog.FileName = "Siparişler_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = (DataTable)dataGridView1.DataSource;

                    StringBuilder sb = new StringBuilder();

                    var columnNames = dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName);
                    sb.AppendLine(string.Join(",", columnNames));

                    foreach (DataRow row in dt.Rows)
                    {
                        var fields = row.ItemArray.Select(field =>
                            $"\"{field.ToString().Replace("\"", "\"\"")}\"");
                        sb.AppendLine(string.Join(",", fields));
                    }

                    File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);

                    MessageBox.Show("CSV başarıyla kaydedildi: " + saveFileDialog.FileName, "Bilgi",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
