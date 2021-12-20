using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZBmarket
{
    public partial class MusteriKayit : Form
    {
        public MusteriKayit()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=ZBmarket; User Id=postgres; Password=134105");

        private void btnEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand mEkle = new NpgsqlCommand("insert into musteri(mad,msoyad,mtel) values(@p1,@p2,@p3)", baglanti);

            mEkle.Parameters.AddWithValue("@p1", txtAd.Text);
            mEkle.Parameters.AddWithValue("@p2", txtSoyad.Text);
            mEkle.Parameters.AddWithValue("@p3", txtTel.Text);
            
            mEkle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Müşteri ekleme işlemi başarılı bir şekilde gerçekleşti");

            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}
