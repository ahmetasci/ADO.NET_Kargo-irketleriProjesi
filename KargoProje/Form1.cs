using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;


namespace KargoProje
{
    public partial class Form1 : Form
    {
        //lstMusteriler'e müşteri listesini(companyname) getirecek. 
        //Comboboxa kargo şirketlerini getirecek.
        //    lstMüsterilerden seçili müşteriye comboboxdan seçili kargo şirketiyle giden siparişlerin
        //    OrderId ve OrderDateleri buttona tıklandığında lstSipariler listboxında gösterilecek
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(Tools.ConnectionString);//Tools.cs'deki tanımlı bağlantıyı çektik.
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void KargoDoldur()
        {
            if (baglanti.State == ConnectionState.Closed)
            {//CONNECTED MİMARİ
                baglanti.Open();
                SqlCommand cmd = new SqlCommand("SELECT CompanyName FROM Shippers", baglanti);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.HasRows)//HasRows data olması durumunda true, yoksa false döndürcek
                {//içinde değer varsa temizle
                    comboBox1.Items.Clear();
                }

                while (rdr.Read())
                {
                    comboBox1.Items.Add(rdr["CompanyName"].ToString());
                }
                baglanti.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SiparisDoldur();
        }
        private void SiparisDoldur()
        {
            lstSiparisler.Items.Clear();

            SqlCommand cmd = new SqlCommand("SELECT OrderID, OrderDate FROM Orders o INNER JOIN Customers c ON o.CustomerID = c.CustomerID WHERE c.CompanyName= @comName", baglanti);
            //Siparişlerin ID'lerini ve tarihlerini getirir.
            cmd.Parameters.AddWithValue("@comName", lstMusteriler.SelectedItem.ToString());

            baglanti.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {


                string siparisid = rdr["OrderID"].ToString();  
                string siparistarihi = rdr["OrderDate"].ToString();

                lstSiparisler.Items.Add(siparisid + "******" + siparistarihi);

            }
            baglanti.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            KargoDoldur();
            //ComboBox'ın içini kargo firmalarıyla dolduruyoruz.
            SqlCommand cmd = new SqlCommand("SELECT CustomerID,CompanyName FROM Customers", baglanti);
            baglanti.Open();
            SqlDataReader rdr = cmd.ExecuteReader();//olan bütün kayıtları döndürür.
            while (rdr.Read())
            {
                //string id = rdr["CustomerID"].ToString();
                string sirketismi = rdr["CompanyName"].ToString();

                lstMusteriler.Items.Add(sirketismi);

            }
            baglanti.Close();

        }
    }
}
