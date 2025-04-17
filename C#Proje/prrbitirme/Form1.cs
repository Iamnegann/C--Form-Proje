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
    public partial class Form1 : Form
    {
        public static int GirisYapanKullaniciId;
        public Form1()
        {
            InitializeComponent();
        }
        DateTime stSaat = new DateTime();
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-9PJ75G5\SQLEXPRESS;Initial Catalog=bitirmepr;Integrated Security=True");
        private void button1_Click(object sender, EventArgs e)
        {  }
        private void button1_Click_1(object sender, EventArgs e)
        {
            string kullaniciAdi = textBox1.Text;
            string sifre = textBox2.Text;

            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand(
                    "SELECT Id, Rol FROM Kullanicilar WHERE KullaniciAdi=@kullaniciAdi AND Sifre=@sifre",
                    baglanti);

                komut.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                komut.Parameters.AddWithValue("@sifre", sifre);

                SqlDataReader dr = komut.ExecuteReader();

                if (dr.Read()) 
                {
                    GirisYapanKullaniciId = Convert.ToInt32(dr["Id"]);
                    object rolValue = dr["Rol"]; 

                    Form3 userForm = new Form3();
                    userForm.Show();
                    this.Hide();
                    
                    if (rolValue != DBNull.Value && rolValue.ToString() == "admin")
                    {
                        userForm.Close(); 
                        Form5 adminForm = new Form5();
                        adminForm.Show();
                    }
                }
                else 
                {
                    MessageBox.Show("Kullanıcı Adı veya Şifre Yanlış!", "Hata",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();
                    textBox2.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Sistem Hatası",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
            }
        }
    
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            stSaat = DateTime.Now;
            label2.Text = stSaat.Hour.ToString("00");
            label4.Text = stSaat.Minute.ToString("00");
            label6.Text = stSaat.Second.ToString("00");
        }
    }
}
