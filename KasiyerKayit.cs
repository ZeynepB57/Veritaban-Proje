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
    public partial class KasiyerKayit : Form
    {
        public KasiyerKayit()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=ZBmarket; User Id=postgres; Password=134105");

        private void btnEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            
            NpgsqlCommand personelEkle = new NpgsqlCommand("insert into kasiyer(kasiyerad,kasiyersoyad,bastarih,kasiyersifre) values(@p1,@p2,@p3,@p4)", baglanti);

            personelEkle.Parameters.AddWithValue("@p1",txtAd.Text );
            personelEkle.Parameters.AddWithValue("@p2",txtSoyad.Text );
            personelEkle.Parameters.AddWithValue("@p3", DateTime.Parse(DateTime.Now.ToShortDateString()));
            personelEkle.Parameters.AddWithValue("@p4", txtKsifre.Text);


            personelEkle.ExecuteNonQuery();

            MessageBox.Show("Kasiyer Kasa Seçimi Başarılı");
            baglanti.Close();
        }

        private void btnListe_Click(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            string sql = "SELECT *FROM kasiyer";

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, baglanti);
            adapter.Fill(dataSet);

            dataGridViewListe.DataSource = dataSet.Tables[0];
        }
    }
}
