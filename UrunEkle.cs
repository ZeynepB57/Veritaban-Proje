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
    public partial class UrunEkle : Form
    {
        public UrunEkle()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=ZBmarket; User Id=postgres; Password=134105");

        private void btnEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand urunEkle = new NpgsqlCommand("insert into urun(urunad,urunfiyat,urunkar,urunmiktar,urunmiktartur,urunlimit,urunkategori) values(@p1,@p2,@p3,@p4,@p5,@p6,@p7)", baglanti);

            urunEkle.Parameters.AddWithValue("@p1", txtAd.Text);
            urunEkle.Parameters.AddWithValue("@p2", int.Parse(txtFiyat.Text));
            urunEkle.Parameters.AddWithValue("@p3", int.Parse(txtKar.Text));
            urunEkle.Parameters.AddWithValue("@p4", int.Parse(txtMiktar.Text));
            urunEkle.Parameters.AddWithValue("@p5", int.Parse(txtMTur.Text));
            urunEkle.Parameters.AddWithValue("@p6", int.Parse(txtLimit.Text));
            urunEkle.Parameters.AddWithValue("@p7", int.Parse(txtKategori.Text));
            urunEkle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün ekleme işlemi başarılı bir şekilde gerçekleşti");
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand urunSil = new NpgsqlCommand("DElete From urun where urunid=@p1 ", baglanti);
            urunSil.Parameters.AddWithValue("@p1", int.Parse(txtUrunId.Text));
            urunSil.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürünn silme işlemi başarılı bir şekilde gerçekleşti");
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand urunGuncelle = new NpgsqlCommand("update urun set urunad=@p1 , urunfiyat=@p2 , urunkar=@p3 , urunmiktar=@p4 , urunmiktartur=@p5 , urunlimit=@p6 , urunkategori=@p7 where urunid=@p8", baglanti);

            urunGuncelle.Parameters.AddWithValue("@p1", txtAd.Text);
            urunGuncelle.Parameters.AddWithValue("@p2", int.Parse(txtFiyat.Text));
            urunGuncelle.Parameters.AddWithValue("@p3", int.Parse(txtKar.Text));
            urunGuncelle.Parameters.AddWithValue("@p4", int.Parse(txtMiktar.Text));
            urunGuncelle.Parameters.AddWithValue("@p5", int.Parse(txtMTur.Text));
            urunGuncelle.Parameters.AddWithValue("@p6", int.Parse(txtLimit.Text));
            urunGuncelle.Parameters.AddWithValue("@p7", int.Parse(txtKategori.Text));
            urunGuncelle.Parameters.AddWithValue("@p8", int.Parse(txtUrunId.Text));
            urunGuncelle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Personel güncelleme işlemi başarılı bir şekilde gerçekleşti");
        }

        private void btnListe_Click(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            string sql = "SELECT *FROM urun";

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, baglanti);
            adapter.Fill(dataSet);

            dataGridViewUrun.DataSource = dataSet.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MudurGiris mudurGiris = new MudurGiris();
            mudurGiris.Show();
            this.Hide();
        }
    }
}
