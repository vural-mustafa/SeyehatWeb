﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SeyehatWeb.Yönetim
{
    public partial class BlogDüzenleSecilen : System.Web.UI.Page
    {
        string conf_baglanti = WebConfigurationManager.ConnectionStrings["dbGoTripConnectionString"].ConnectionString;
        DateTime bugun = DateTime.Now;
        protected void Page_Load(object sender, EventArgs e)
        {
            Button2.Enabled = false;
            lbltarih.Text = bugun.ToString();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            SqlConnection baglanti = new SqlConnection(conf_baglanti);
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from tblBlog where id=@id", baglanti);
            komut.Parameters.AddWithValue("@id", Request.QueryString["id"]);
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku);
            TxtBaslik.Text = tablo.Rows[0]["Baslik"].ToString();
            Txtozet.Text = tablo.Rows[0]["Ozet"].ToString();
            DropDownList1.SelectedItem.Text = tablo.Rows[0]["KategoriId"].ToString();
            lblResim.Text = tablo.Rows[0]["Resim"].ToString();
            txtdetay.Text = tablo.Rows[0]["Detay"].ToString();
            lbltarih.Text = tablo.Rows[0]["Tarih"].ToString();

            baglanti.Close();
            Button2.Enabled = true;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                if (FileUpload1.PostedFile.ContentType == "image/jpeg" || FileUpload1.PostedFile.ContentType == "image/jpg" || FileUpload1.PostedFile.ContentType == "image/png")
                {
                    string ResimAd = FileUpload1.FileName.ToString();
                    FileUpload1.SaveAs(Server.MapPath("~/image/BlogDuzenle/" + ResimAd));
                    lblResim.Text = ResimAd.ToString();
                }
                else
                {
                    lblResim.Text = "Lütfen jpeg,jpg veya png uzantılı resim seçin";
                }


            }
            else
                lblResim.Text = "Lütfen bir resim seçiniz...";
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(conf_baglanti);
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update tblBlog set Baslik=@Baslik,Ozet=@Ozet,KategoriId=@KategoriId,Resim=@Resim,Detay=@Detay,Tarih=@Tarih where id=@id", baglanti);
            komut.Parameters.AddWithValue("id", Request.QueryString["id"]);
            komut.Parameters.AddWithValue("@Baslik", TxtBaslik.Text.ToString());
            komut.Parameters.AddWithValue("@Ozet", Txtozet.Text.ToString());
            komut.Parameters.AddWithValue("@KategoriId", DropDownList1.SelectedValue);
            komut.Parameters.AddWithValue("@Resim", lblResim.Text.ToString());
            komut.Parameters.AddWithValue("@Detay", txtdetay.Text.ToString());
            komut.Parameters.AddWithValue("@Tarih", bugun);
            komut.ExecuteNonQuery();
            baglanti.Close();
            Response.Redirect("BlogDüzenleSil.aspx");
        }
       
        
    }
}