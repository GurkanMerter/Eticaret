using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using eticaret_staj2.Models;
using System.Diagnostics;
using System.Web.Security;

namespace eticaret_staj2.Controllers
{
   
    public class HomeController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;   
        List<UrunListele> urun = new List<UrunListele>();
        List<Urun> persurunara = new List<Urun>();
        
        /*                     Giriş Yapma Kısmı                        */
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
            
        }
        
        [HttpPost]
        public ActionResult SingIn(Kullanicilar kul)
        {
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from Kullanicilar where Eposta='"+kul.Eposta+"' and Sifre='"+kul.Sifre+"'";

            dr = com.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                return View("Search");
            }
            else
            {
                con.Close();
                return View("Hata");
            }
        }

            /*                 Üye Olma Kısmı                             */
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();

        }
        [HttpPost]
        public ActionResult SignUp(Kullanicilar kul)
        {
            connectionString();
            con.Open();
            com.CommandText="insert into Kullanicilar(Ad,SoyAd,Eposta,Sifre) " +
                "values('" + kul.Ad + "','" + kul.SoyAd + "','" + kul.Eposta + "','" + kul.Sifre + "')";
            com.Connection = con;
            com.ExecuteNonQuery();
            con.Close();
            return View("SignIn");

        }

        /*                   ana menü ve ARAMA KISMI                           */


        [HttpGet]
        public ActionResult Search()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Search(UrunListele urn)
        {
            string urunad;
            urunad = urn.tanim;
            arama(urunad);   
            return View(urun);

        }

        private void arama(string gelenurun)
        {
            if (urun.Count > 0)
            {
                urun.Clear();
            }
            try
            {
                connectionString();
                con.Open();
                com.Connection = con;              
                com.CommandText = "select tanim,resimURL from Urun where tanim like @gelen";
                com.Parameters.AddWithValue("@gelen", "%" + gelenurun + "%");
                dr = com.ExecuteReader();

                while (dr.Read())
                {
                    urun.Add(new UrunListele() { tanim = dr["tanim"].ToString(), resimURL = dr["resimURL"].ToString() });
                }
                con.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
       

        /*               Personel üye olma ve giriş yapma               */
        [HttpGet]
            public ActionResult PersonelSignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PersonelSignIn(Personel pers)
        {
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from Personel where Eposta='" + pers.Eposta + "' and Sifre='" + pers.Sifre + "'";

            dr = com.ExecuteReader();
            if (dr.Read())
            {
                var personeldb = dr["Eposta"];               
                FormsAuthentication.SetAuthCookie(personeldb.ToString(), false);
                con.Close();
                return View("PersonelYetkiliSayfa");
            }
            else
            {
                con.Close();
                return View("Hata");

            }

        }

        [HttpGet]
        public ActionResult PersonelUyeOlma()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PersonelUyeOlma(Personel pers)
        {
            connectionString();
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Personel(Ad,Soyad,Eposta,Sifre,Yetki) " +
                "values('" + pers.Ad + "','" + pers.Soyad + "','" + pers.Eposta + "','" + pers.Sifre + "','" + 0 + "')");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
            return View("PersonelSignIn");
        }


        /*                Ürün Ekleme Kısmı                    */

        [HttpGet]
        [Authorize(Roles ="true")]
        public ActionResult PersonelUrunEkle()
        {
            
            return View();
            
        }

        [HttpPost]
        [Authorize(Roles = "true")]
        public ActionResult PersonelUrunEkle(Urun urun)
        {
            connectionString();
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Urun(tanim,markaID,resimURL) values('" + urun.tanim + "','" + urun.markaID + "','" + urun.resimURL + "')");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
            return View("PersonelUrunEkle");

        }
        
        [HttpGet]
        [Authorize(Roles ="true,false")]
            public ActionResult PersonelYetkiliSayfa()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles ="true,false")]
            public ActionResult PersonelYetkiliSayfa(UrunListele urn)
        {
            string urunad;
            urunad = urn.tanim;
            ikinciarama(urunad);
            return View(persurunara);
        }

        [HttpGet]
        [Authorize(Roles = "true")]
        public ActionResult UrunDuzenle(int? urunid)
        {
            var secilenurun=new Urun();
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText= "select id,tanim,markaID,resimURL from Urun where id='"+urunid+"'";
            dr = com.ExecuteReader();
            
            while (dr.Read())
            {                
                secilenurun = new Urun() { id= Convert.ToInt32(dr["id"]), tanim = dr["tanim"].ToString(), markaID = Convert.ToInt32(dr["markaID"]), resimURL = dr["resimURL"].ToString() };
                
            }
            con.Close();
            return View("UrunDuzenle", secilenurun);
        }

        [HttpPost]
        [Authorize(Roles = "true")]
        public ActionResult UrunDuzenle(FormCollection form)
        {
            var id = form["id"];
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText="update Urun Set tanim='"+ form["tanim"] + "',markaID='" + form["markaID"] + "',resimURL='" + form["resimURL"] + "' where id='" + form["id"]+ "'";
            com.ExecuteNonQuery();

            con.Close();

            return View("PersonelYetkiliSayfa");
        }

        private void ikinciarama(string gelenurun)
        {
            if (persurunara.Count > 0)
            {
                persurunara.Clear();
            }
            try
            {
                connectionString();
                con.Open();
                com.Connection = con;

                com.CommandText = "select id,tanim,markaID,resimURL from Urun where tanim like @gelen";
                com.Parameters.AddWithValue("@gelen", "%" + gelenurun + "%");
                dr = com.ExecuteReader();

                while (dr.Read())
                {
                    persurunara.Add(new Urun() { id = Convert.ToInt32(dr["id"]), tanim = dr["tanim"].ToString(), markaID = Convert.ToInt32(dr["markaID"]), resimURL = dr["resimURL"].ToString() });
                }
                con.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /*                ConnectionString                */

        void connectionString()
        {
           
            con.ConnectionString = "Data Source = MONSTERPC; database = ProjeDB; Integrated Security = False; User Id = MyUser; Password = MyUser123;";
        }
    }
}