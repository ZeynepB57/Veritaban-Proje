using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ZBmarket
{
    public partial class Form1 : Form
    {   
        public Form1()
        {
            InitializeComponent();
        
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=ZBmarket; User Id=postgres; Password=134105");

        bool personel = false;
        

        public static class DataContainer
        {
            public static int kullaniciid;
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {

            string username = txtLogAd.Text;
            string password = txtLogSifre.Text;
            

            baglanti.Open();

            if (personel)
            {


                NpgsqlCommand kontrol = new NpgsqlCommand("Select * from personel", baglanti);
                NpgsqlDataReader reader = kontrol.ExecuteReader();

                while (reader.Read())
                {
                    DataContainer.kullaniciid= int.Parse(reader["perid"].ToString());
                    int tmp = int.Parse(reader["gorev"].ToString());
                    switch (tmp)
                    {
                        case 1:
                            if (username == reader["perad"].ToString() && password == reader["sifre"].ToString())
                            {

                                MessageBox.Show("Müdür Girişi Başarılı");
                                MudurGiris mudurGiris = new MudurGiris();
                                mudurGiris.Show();
                                this.Hide();
                                break;
                            }
                            break;
                        case 2:
                            if (username == reader["perad"].ToString() && password == reader["sifre"].ToString())
                            {
                                MessageBox.Show("Kasiyer Girişi Başarılı");
                                KasiyerGiris kasiyerGiris = new KasiyerGiris();
                                kasiyerGiris.Show();
                                this.Hide();
                                break;
                            }
                            break;
                        case 3:
                            if (username == reader["perad"].ToString() && password == reader["sifre"].ToString())
                            {
                                MessageBox.Show("Depo Elemanı Girişi Başarılı");
                                DepoElemani depoElemani = new DepoElemani();
                                depoElemani.Show();
                                this.Hide();
                                break;
                            }
                            break;
                        case 4:
                            if (username == reader["perad"].ToString() && password == reader["sifre"].ToString())
                            {
                                MessageBox.Show("Temizlik Görevlisi giriş yapamaz");
                                break;
                            }
                            break;
                        case 5:
                            if (username == reader["perad"].ToString() && password == reader["sifre"].ToString())
                            {
                                MessageBox.Show("Muhasebeci Girişi Başarılı");
                                KasiyerGiris kasiyerGiris = new KasiyerGiris();
                                kasiyerGiris.Show();
                                this.Hide();
                                break;
                            }
                            break;
                    }

                }
            }
            else
            {
                NpgsqlCommand kontrol = new NpgsqlCommand("Select * from musteri ", baglanti);
                NpgsqlDataReader reader = kontrol.ExecuteReader();
                
                

                if (!reader.Read())
                {
                    MessageBox.Show("Müşteri Veritabanı Boş");

                }
                else
                {
                    bool musvar=false;
                    while (reader.Read())

                    {
                        if (username == reader["mad"].ToString())
                        {
                            
                            MessageBox.Show("Müşteri Girişi Başarılı"+username);

                            DataContainer.kullaniciid = int.Parse(reader["musteriid"].ToString());
                            MusteriGiris musteriGiris = new MusteriGiris();
                            musteriGiris.Show();
                            this.Hide();
                            musvar = true;
                        }
                        
                    }
                    if(!musvar)
                    {
                        MessageBox.Show("Bu isimli Müşteri Yoktur Tekrar Deneyiniz Veya Kayıt Yaptırınız!!");
                        button4.Visible = true;
                    }
                }

            }
            baglanti.Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtLogAd.Visible = false;
            txtLogSifre.Visible = false;
            label8.Visible = false;
            label7.Visible = false;
            btnGiris.Visible = false;
            button4.Visible = false; 

        }

        private void button1_Click(object sender, EventArgs e)
        {
            personel = false;
            button1.Visible = false;
            txtLogAd.Visible = true;
            txtLogSifre.Visible = false;
            label8.Visible = true;
            label7.Visible = false;
            btnGiris.Visible = true;
            button2.Visible = false;
            button4.Visible = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            personel = true;
            button1.Visible = false;
            txtLogAd.Visible = true;
            txtLogSifre.Visible = true;
            label8.Visible = true;
            label7.Visible = true;
            btnGiris.Visible = true;
            button2.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            personel = false;
            button1.Visible = true;
            txtLogAd.Visible = false;
            txtLogSifre.Visible = false;
            label8.Visible = false;
            label7.Visible = false;
            btnGiris.Visible = false;
            button2.Visible = true;
            button4.Visible = false;
            
        }

        private void button4_Click(object sender, EventArgs e)
        {

            MusteriKayit musteriKayit = new MusteriKayit();
            musteriKayit.Show();
            this.Hide();
        }
    }
}
