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
    public partial class UrunSiparis : Form
    {
        public UrunSiparis()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=ZBmarket; User Id=postgres; Password=134105");

        private void UrunSiparis_Load(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            string sql = "SELECT t.yenisiparisid,t1.urunad,t1.urunmiktar,t.tarih,t.siparistarih FROM yenisiparis t  inner join urun t1 on (t.urunid=t1.urunid) where siparistarih is null";

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, baglanti);
            adapter.Fill(dataSet);

            dataGridViewListe.DataSource = dataSet.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MudurGiris mudurGiris = new MudurGiris();
            mudurGiris.Show();
            this.Hide();
        }

        private void btnSiparis_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand personelEkle = new NpgsqlCommand("update yenisiparis set siparistarih=@p1 where yenisiparisid=@p2", baglanti);

            personelEkle.Parameters.AddWithValue("@p1", DateTime.Parse(DateTime.Now.ToShortDateString()));
            personelEkle.Parameters.AddWithValue("@p2", int.Parse(txtSiparis.Text));
            personelEkle.ExecuteNonQuery();
            baglanti.Close();

            SiparisFatura siparisFatura = new SiparisFatura();
            siparisFatura.Show();
            this.Hide();
        }
    }
}
