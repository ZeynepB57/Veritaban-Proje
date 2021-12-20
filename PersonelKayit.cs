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
    public partial class PersonelKayit : Form
    {
        public PersonelKayit()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=ZBmarket; User Id=postgres; Password=134105");

        private void btnEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand personelEkle = new NpgsqlCommand("insert into personel(perad,persoyad,baslamatarih,gorev,tel,sifre,durum) values(@p1,@p2,@p3,@p4,@p5,@p6,@p7)", baglanti);

            personelEkle.Parameters.AddWithValue("@p1", txtAd.Text);
            personelEkle.Parameters.AddWithValue("@p2", txtSoyad.Text);
            personelEkle.Parameters.AddWithValue("@p3", DateTime.Parse(DateTime.Now.ToShortDateString()));
            personelEkle.Parameters.AddWithValue("@p4", (comboBox1.SelectedIndex) + 1);
            personelEkle.Parameters.AddWithValue("@p5", txtTel.Text);
            personelEkle.Parameters.AddWithValue("@p6", txtPsifre.Text);
            personelEkle.Parameters.AddWithValue("@p7", true);
            personelEkle.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Personel ekleme işlemi başarılı bir şekilde gerçekleşti");
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (txtPerId.Text == "")
            {
                MessageBox.Show("Personl Id Girilmeden Silme Yapılmaz!");
            }
            else
            {
                btnEkle.Enabled = false;
                baglanti.Open();
                NpgsqlCommand personelSil = new NpgsqlCommand("DElete From personel where perid=@p1", baglanti);
                personelSil.Parameters.AddWithValue("@p1", int.Parse(txtPerId.Text));
                personelSil.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Personel silme işlemi başarılı bir şekilde gerçekleşti");
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (txtPerId.Text == "")
            {
                MessageBox.Show("Personl Id Girilmeden Güncelleme Yapılmaz!");
            }
            else
            {
                btnEkle.Enabled = false;

                baglanti.Open();
                NpgsqlCommand personelEkle = new NpgsqlCommand("update personel set perad=@p1 , persoyad=@p2 , gorev=@p3 , tel=@p4 , sifre=@p5 where perid=@p6", baglanti);

                personelEkle.Parameters.AddWithValue("@p1", txtAd.Text);
                personelEkle.Parameters.AddWithValue("@p2", txtSoyad.Text);
                personelEkle.Parameters.AddWithValue("@p3", (comboBox1.SelectedIndex) + 1);
                personelEkle.Parameters.AddWithValue("@p4", txtTel.Text);
                personelEkle.Parameters.AddWithValue("@p5", txtPsifre.Text);
                personelEkle.Parameters.AddWithValue("@p6", int.Parse(txtPerId.Text));
                personelEkle.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Personel güncelleme işlemi başarılı bir şekilde gerçekleşti");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
      
        }

        private void btnListe_Click(object sender, EventArgs e)
        {
            DataSet dataSet = new DataSet();
            string sql = "SELECT *FROM personel";

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, baglanti);
            adapter.Fill(dataSet);

            dataGridViewListe.DataSource = dataSet.Tables[0];
        }

        private void txtPerId_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void txtPsifre_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewListe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtGorev_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTel_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSoyad_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAd_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void PersonelKayit_Load(object sender, EventArgs e)
        {
            baglanti.Open();

            DataSet ds = new DataSet();
            string sql0 = "SELECT gorevad FROM gorev";


            NpgsqlDataAdapter adap = new NpgsqlDataAdapter(sql0, baglanti);
            adap.Fill(ds);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                comboBox1.Items.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
            }

            baglanti.Close();
        }

        private void txtPerId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                DataSet dataSet = new DataSet();
                string sql = "SELECT * FROM personel where perid=" + txtPerId.Text;

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, baglanti);
                adapter.Fill(dataSet);

                txtAd.Text = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
                txtSoyad.Text = dataSet.Tables[0].Rows[0].ItemArray[2].ToString();
                txtTel.Text = dataSet.Tables[0].Rows[0].ItemArray[4].ToString();
                if (bool.Parse(dataSet.Tables[0].Rows[0].ItemArray[6].ToString()))
                    txtDurum.Text = "AKTİF";
                else
                    txtDurum.Text = "PASİF";

                comboBox1.SelectedIndex = (int.Parse(dataSet.Tables[0].Rows[0].ItemArray[5].ToString())-1);
                txtPsifre.Text = dataSet.Tables[0].Rows[0].ItemArray[7].ToString();
            }
        }
    }
}
