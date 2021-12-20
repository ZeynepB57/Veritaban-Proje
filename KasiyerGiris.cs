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
    
    public partial class KasiyerGiris : Form

    {
       
        public KasiyerGiris()
        {
            
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=ZBmarket; User Id=postgres; Password=134105");
        private void KasiyerGiris_Load(object sender, EventArgs e)
        {
            
            baglanti.Open();

            DataSet dataSet = new DataSet();
            string sql = "SELECT kasaad FROM kasa  order by kasaad";

            
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, baglanti);
            adapter.Fill(dataSet);

            for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                comboBox1.Items.Add(dataSet.Tables[0].Rows[i].ItemArray[0].ToString());
            }

            baglanti.Close();
        }
        
        private void btnSat_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            
            string sql = "SELECT * FROM kasa where kasaad='"+comboBox1.SelectedItem.ToString()+"'";

            NpgsqlCommand kontrol = new NpgsqlCommand(sql, baglanti);
            NpgsqlDataReader reader = kontrol.ExecuteReader();

            if (reader.Read())
            {
                int kasaid = int.Parse(reader["kasaid"].ToString());
                if (bool.Parse(reader["kasadurum"].ToString()))
                {
                    MessageBox.Show("Bu Kasa Şuanda Kullanımdadır. Başka kasa seçiniz!");
                }
                else
                {
                    baglanti.Close();
                    baglanti.Open();

                    NpgsqlCommand kasaAktif = new NpgsqlCommand("update kasa set kasadurum=@p1,kasiyerid=@p2 where kasaad='" + comboBox1.SelectedItem.ToString() + "'", baglanti);

                    kasaAktif.Parameters.AddWithValue("@p1", true);
                    kasaAktif.Parameters.AddWithValue("@p2", DataContainer.kullaniciid);

                    kasaAktif.ExecuteNonQuery();
                    
                    MessageBox.Show("Kasa Aktif!!");

                    baglanti.Close();
                    baglanti.Open();

                    NpgsqlCommand kasaTakip = new NpgsqlCommand("insert into kasiyertakip(kasiyerid,kasa,bastarih) values(@p1,@p2,@p3)", baglanti);

                    kasaTakip.Parameters.AddWithValue("@p1", DataContainer.kullaniciid);
                    kasaTakip.Parameters.AddWithValue("@p2", kasaid);
                    kasaTakip.Parameters.AddWithValue("@p3", DateTime.Parse(DateTime.Now.ToLongDateString()));
                   

                    kasaTakip.ExecuteNonQuery();

                    MessageBox.Show("Kasa Aktif!!");
                    baglanti.Close();

                    KasiyerSatis kasiyerSatis = new KasiyerSatis();
                    kasiyerSatis.Show();
                    this.Hide();
                }
            }
            baglanti.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

             baglanti.Open();

                    NpgsqlCommand kasaAktif = new NpgsqlCommand("update kasa set kasadurum=@p1 where kasaad='" + comboBox1.SelectedItem.ToString() + "' and kasiyerid="+DataContainer.kullaniciid.ToString() , baglanti);

                    kasaAktif.Parameters.AddWithValue("@p1", false);
                    kasaAktif.ExecuteNonQuery();

                    baglanti.Close();
            baglanti.Open();

            NpgsqlCommand kasaTakip = new NpgsqlCommand("update kasiyertakip set bittarih=@p1 where bittarih is NULL  and kasiyerid=" + DataContainer.kullaniciid.ToString(), baglanti);

            kasaTakip.Parameters.AddWithValue("@p1", DateTime.Parse(DateTime.Now.ToLongTimeString()));
            kasaTakip.ExecuteNonQuery();

            MessageBox.Show("Kasa Kapatıldı!!");

            baglanti.Close();

        }
    }
}
