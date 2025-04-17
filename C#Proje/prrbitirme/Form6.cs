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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
namespace prrbitirme
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-9PJ75G5\SQLEXPRESS;Initial Catalog=bitirmepr;Integrated Security=True");
        private void KullanicilarListele()
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Kullanicilar", baglanti);
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
                string aranankul = textBox1.Text.Trim();

                if (string.IsNullOrEmpty(aranankul))
                {
                    KullanicilarListele();
                    return;
                }

                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT * FROM Kullanicilar WHERE KullaniciAdi LIKE @aranan", baglanti);
                da.SelectCommand.Parameters.AddWithValue("@aranan", "%" + aranankul + "%");

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

        private void Form6_Load(object sender, EventArgs e)
        {
            KullanicilarListele();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                    baglanti.Open();

                int silinecekId = Convert.ToInt32(textBox2.Text);
                SqlCommand komut = new SqlCommand("DELETE FROM Kullanicilar WHERE Id = @Id", baglanti);
                komut.Parameters.AddWithValue("@Id", silinecekId);

                int sonuc = komut.ExecuteNonQuery();

                if (sonuc > 0)
                {
                    MessageBox.Show("Ürün başarıyla silindi.");
                    KullanicilarListele();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            AramaYap();
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
                saveFileDialog.FileName = "Urunler_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";

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
