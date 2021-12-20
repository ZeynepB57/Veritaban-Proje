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
using static ZBmarket.Form1;

namespace ZBmarket
{
    public partial class KasiyerSatis : Form
    {
        public KasiyerSatis()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=ZBmarket; User Id=postgres; Password=134105");

        private void KasiyerSatis_Load(object sender, EventArgs e)
        {

            label6.Text = DataContainer.kullaniciid.ToString();

            baglanti.Open();

            DataSet dataSet3= new DataSet();
            string sql3 = "SELECT kasaad,kasaid FROM kasa where kasadurum is true and kasiyerid="+label6.Text.ToString();
            MessageBox.Show("sql" + sql3);


            NpgsqlDataAdapter adapter3 = new NpgsqlDataAdapter(sql3, baglanti);
            adapter3.Fill(dataSet3);

            label5.Text = dataSet3.Tables[0].Rows[0].ItemArray[0].ToString();
            label10.Text = dataSet3.Tables[0].Rows[0].ItemArray[1].ToString();

            baglanti.Close();

            baglanti.Open();

            DataSet dataSet4 = new DataSet();
            string sql4 = "SELECT odemead FROM odemesekli";


            NpgsqlDataAdapter adapter4 = new NpgsqlDataAdapter(sql4, baglanti);
            adapter4.Fill(dataSet4);

            for (int i = 0; i < dataSet4.Tables[0].Rows.Count; i++)
            {
                comboBox1.Items.Add(dataSet4.Tables[0].Rows[i].ItemArray[0].ToString());
            }

            baglanti.Close();

            baglanti.Open();

            DataSet dataSet5 = new DataSet();
            string sql5 = "SELECT DISTINCT fatura FROM satis where durum is false order by fatura";


            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql5, baglanti);
            adapter.Fill(dataSet5);

            for (int i = 0; i < dataSet5.Tables[0].Rows.Count; i++)
            {
                comboBox2.Items.Add(dataSet5.Tables[0].Rows[i].ItemArray[0].ToString());
            }

            baglanti.Close();

            


            /* baglanti.Open();

             DataSet ds = new DataSet();
             string sql0 = "SELECT urunad FROM urun";


             NpgsqlDataAdapter adap = new NpgsqlDataAdapter(sql0, baglanti);
             adap.Fill(ds);

             for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
             {
                 comboBox2.Items.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
             }

             baglanti.Close();


             baglanti.Open();

             DataSet dataSet4 = new DataSet();
             string sql4 = "SELECT odemead FROM odemesekli";


             NpgsqlDataAdapter adapter4 = new NpgsqlDataAdapter(sql4, baglanti);
             adapter4.Fill(dataSet4);

             for (int i = 0; i < dataSet4.Tables[0].Rows.Count; i++)
             {
                 comboBox3.Items.Add(dataSet4.Tables[0].Rows[i].ItemArray[0].ToString());
             }

             baglanti.Close();*/


        }

        private void button1_Click(object sender, EventArgs e)
        {
            KasiyerGiris kasiyerGiris = new KasiyerGiris();
            kasiyerGiris.Show();
            this.Hide();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            baglanti.Open();
            DataSet dataSetS = new DataSet();
            string sql = "Select fatura.faturaid,urun.urunad,fatura.fiyat,fatura.urunmiktar  from urun inner join fatura on urun.urunid = fatura.urun where fatura.faturano = " + comboBox2.SelectedItem.ToString();

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, baglanti);
            adapter.Fill(dataSetS);
           

            dataGridView1.DataSource = dataSetS.Tables[0];
            baglanti.Close();


            baglanti.Open();
            DataSet dataSetT = new DataSet();
            string sqlT = "Select faturatoplam  from satis where durum is false and fatura= " + comboBox2.SelectedItem.ToString();

            NpgsqlDataAdapter adapterT = new NpgsqlDataAdapter(sqlT, baglanti);
            adapterT.Fill(dataSetT);

            label9.Text = (dataSetT.Tables[0].Rows[0].ItemArray[0].ToString());

            baglanti.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //fatutra durum true
            // toplam fiyat kasa miktara ekle

            //satış durumu true

            baglanti.Open();
            NpgsqlCommand satisGuncelle = new NpgsqlCommand("update satis set durum=@p1 , kasa=@p2 ,kasiyer=@p3 , odemesekli=@p4 where fatura= " + comboBox2.SelectedItem.ToString(), baglanti);

            satisGuncelle.Parameters.AddWithValue("@p1", true);
            satisGuncelle.Parameters.AddWithValue("@p2", int.Parse(label10.Text.ToString()));
            satisGuncelle.Parameters.AddWithValue("@p3", int.Parse(label6.Text.ToString()));
            satisGuncelle.Parameters.AddWithValue("@p4", comboBox1.SelectedIndex);
            satisGuncelle.ExecuteNonQuery();
            baglanti.Close();
            

            baglanti.Open();
            NpgsqlCommand faturaGuncelle = new NpgsqlCommand("update fatura set satisdurum=@p1 where faturano= " + comboBox2.SelectedItem.ToString(), baglanti);

            faturaGuncelle.Parameters.AddWithValue("@p1", true);
            faturaGuncelle.ExecuteNonQuery();
            baglanti.Close();

            baglanti.Open();
            DataSet dataSetS = new DataSet();
            string sql = "Select kasamiktar  from kasa  where kasaid = " + label10.Text.ToString();

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, baglanti);
            adapter.Fill(dataSetS);
            
            int bakiye = int.Parse(dataSetS.Tables[0].Rows[0].ItemArray[0].ToString());
            baglanti.Close();


            baglanti.Open();
            NpgsqlCommand kasaGuncelle = new NpgsqlCommand("update kasa set kasamiktar=@p1 where kasaid= " + label10.Text.ToString(), baglanti);
            bakiye += int.Parse(label9.Text.ToString());
            kasaGuncelle.Parameters.AddWithValue("@p1", bakiye);

            kasaGuncelle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Personel güncelleme işlemi başarılı bir şekilde gerçekleşti");



        }
    }
}
