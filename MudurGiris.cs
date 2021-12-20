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
    public partial class MudurGiris : Form
    {
        public MudurGiris()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=ZBmarket; User Id=postgres; Password=134105");

        private void btnPerKayit_Click(object sender, EventArgs e)
        {
            PersonelKayit personelKayit = new PersonelKayit();
            personelKayit.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UrunEkle urunekle = new UrunEkle();
            urunekle.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UrunSiparis urunSiparis = new UrunSiparis();
            urunSiparis.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TedarikçiKayit tedarikçiKayit = new TedarikçiKayit();
            tedarikçiKayit.Show();
            this.Hide();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            KasiyerKayit kasiyerKayit = new KasiyerKayit();
            kasiyerKayit.Show();
            this.Hide();
        }
    }
}
