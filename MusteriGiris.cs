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
    public partial class MusteriGiris : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=ZBmarket; User Id=postgres; Password=134105");
        public MusteriGiris()
        {
            InitializeComponent();
        }

        private void MusteriGiris_Load(object sender, EventArgs e)
        {
            label7.Text = DataContainer.kullaniciid.ToString();

            baglanti.Open();

            DataSet dataSetM = new DataSet();
            string sql = "SELECT mad FROM musteri where musteriid="+label7.Text;

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, baglanti);
            adapter.Fill(dataSetM);

            label10.Text = (dataSetM.Tables[0].Rows[0].ItemArray[0].ToString());

            DataSet dataSet = new DataSet();
            sql = "SELECT urunid,urunad FROM urun";

            adapter = new NpgsqlDataAdapter(sql, baglanti);
            adapter.Fill(dataSet);

            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                comboBox1.Items.Add((dataSet.Tables[0].Rows[i].ItemArray[0].ToString()) + " * " + (dataSet.Tables[0].Rows[i].ItemArray[1].ToString()));
            }

            baglanti.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e) //sepete ekle
        {
            label7.Text = DataContainer.kullaniciid.ToString();

            baglanti.Open();
            
            string urun = comboBox1.SelectedItem.ToString();
            int position=urun.IndexOf(" * ");
            urun = urun.Substring(0, position);

            DataSet dataSet = new DataSet();
            string sql = "SELECT * FROM urun where urunid=" + urun;

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, baglanti);
            adapter.Fill(dataSet);

            if (int.Parse(txtMiktar.Text) <= int.Parse(dataSet.Tables[0].Rows[0].ItemArray[8].ToString()))
            {
                MessageBox.Show("Satış Yapılabilir.");

                if (lblFatura.Text == "Fatura No")
                {
                    DataSet dataSetF = new DataSet();
                    sql = "SELECT MAX(faturano) FROM fatura";

                    NpgsqlDataAdapter adapterF = new NpgsqlDataAdapter(sql, baglanti);
                    adapterF.Fill(dataSetF);

                    //dataGridView1.DataSource = dataSetF.Tables[0];
                    int faturano = int.Parse(dataSetF.Tables[0].Rows[0].ItemArray[0].ToString())+1;
                    lblFatura.Text = faturano.ToString();



                    NpgsqlCommand faturaEkle = new NpgsqlCommand("insert into fatura(faturano,fiyat,musteriid,tarih,urun,urunmiktar) values(@p1,@p2,@p3,@p4,@p5,@p6)", baglanti);

                    faturaEkle.Parameters.AddWithValue("@p1", int.Parse(lblFatura.Text));
                    int fiyat = int.Parse(dataSet.Tables[0].Rows[0].ItemArray[3].ToString()) * int.Parse(txtMiktar.Text);
                    faturaEkle.Parameters.AddWithValue("@p2", fiyat);
                    faturaEkle.Parameters.AddWithValue("@p3", DataContainer.kullaniciid); // musteriid
                    faturaEkle.Parameters.AddWithValue("@p4", DateTime.Parse(DateTime.Now.ToShortDateString()));
                    faturaEkle.Parameters.AddWithValue("@p5", int.Parse(urun));
                    faturaEkle.Parameters.AddWithValue("@p6", int.Parse(txtMiktar.Text));
                    faturaEkle.ExecuteNonQuery();
                   
                }
                else
                {

                    NpgsqlCommand faturaEkle = new NpgsqlCommand("insert into fatura(faturano,fiyat,musteriid,tarih,urun,urunmiktar) values(@p1,@p2,@p3,@p4,@p5,@p6)", baglanti);

                    faturaEkle.Parameters.AddWithValue("@p1", int.Parse(lblFatura.Text));
                    int fiyat = int.Parse(dataSet.Tables[0].Rows[0].ItemArray[3].ToString()) * int.Parse(txtMiktar.Text);
                    faturaEkle.Parameters.AddWithValue("@p2", fiyat);
                    faturaEkle.Parameters.AddWithValue("@p3", DataContainer.kullaniciid); // musteriid
                    faturaEkle.Parameters.AddWithValue("@p4", DateTime.Parse(DateTime.Now.ToShortDateString()));
                    faturaEkle.Parameters.AddWithValue("@p5", int.Parse(urun));
                    faturaEkle.Parameters.AddWithValue("@p6", int.Parse(txtMiktar.Text));
                    faturaEkle.ExecuteNonQuery();
                   
                }
                
                DataSet dataSetS = new DataSet();
                sql = "Select fatura.faturaid,urun.urunad,fatura.fiyat,fatura.urunmiktar  from urun inner join fatura on urun.urunid = fatura.urun where fatura.faturano = "+lblFatura.Text;

                adapter = new NpgsqlDataAdapter(sql, baglanti);
                adapter.Fill(dataSetS);
                int toplam = 0;
                for (int i = 0; i < dataSetS.Tables[0].Rows.Count; i++)
                {
                    toplam+=int.Parse(dataSetS.Tables[0].Rows[i].ItemArray[2].ToString());
                    
                }
                label13.Text = toplam.ToString();

                dataGridView1.DataSource = dataSetS.Tables[0];
                baglanti.Close();
            }
            else
            {
                MessageBox.Show("Satış Yapılamaz.");

            }



            

            baglanti.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand UrunSil = new NpgsqlCommand("DElete From fatura where faturaid="+txtUrunNo.Text+ "and faturano="+lblFatura.Text, baglanti);
            UrunSil.ExecuteNonQuery();

            DataSet dataSetS = new DataSet();
            string sql = "Select fatura.faturaid,urun.urunad,fatura.fiyat,fatura.urunmiktar  from urun inner join fatura on urun.urunid = fatura.urun where fatura.faturano = " + lblFatura.Text;

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, baglanti);
            adapter.Fill(dataSetS);
            int toplam = 0;
            for (int i = 0; i < dataSetS.Tables[0].Rows.Count; i++)
            {
                toplam += int.Parse(dataSetS.Tables[0].Rows[i].ItemArray[2].ToString());

            }
            label13.Text = toplam.ToString();
            dataGridView1.DataSource = dataSetS.Tables[0];

            baglanti.Close();
            MessageBox.Show("Ürününüz Sepetten Çıkarılmıştır");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            switch (MessageBox.Show("Satışı Tamamlamak İstiyor Musunuz?", "ZeynepMarket", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    MessageBox.Show("Satışınız Tamamlanmıştır Kasaya Gidebilirsiniz.  Fatura No=" + lblFatura.Text);
                         baglanti.Open();

                            NpgsqlCommand satisEkle = new NpgsqlCommand("insert into satis(fatura,musteri,sattarih,faturatoplam) values(@p1,@p2,@p3,@p4) ", baglanti);

                            satisEkle.Parameters.AddWithValue("@p1", int.Parse(lblFatura.Text));
                            satisEkle.Parameters.AddWithValue("@p2", DataContainer.kullaniciid); // musteriid
                            satisEkle.Parameters.AddWithValue("@p3", DateTime.Parse(DateTime.Now.ToShortDateString()));
                            satisEkle.Parameters.AddWithValue("@p4", int.Parse(label13.Text));
                            satisEkle.ExecuteNonQuery();

                    
                    baglanti.Close();

                    Form1 form1 = new Form1();
                      form1.Show();
                      this.Hide();
                    break;

                case DialogResult.No:
                    MessageBox.Show("Alışverişe Devam Edebilirsiniz");
                    break;

            }
   
            baglanti.Close();

        }
    }
}
