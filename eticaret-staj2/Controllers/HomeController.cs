using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using eticaret_staj2.Models;
using System.Diagnostics;

namespace eticaret_staj2.Controllers
{
    public class HomeController : Controller
    {

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        List<Urun> gelenurun = new List<Urun>();
            /*                     Giriş Yapma Kısmı                        */
       [HttpGet]
        public ActionResult SignIn()
        {
            return View();
            
        }
        
        [HttpPost]
        public ActionResult Verify(Kullanicilar kul)
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
        public ActionResult Register(Kullanicilar kul)
        {
            connectionString();
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Kullanicilar(Ad,SoyAd,Eposta,Sifre) " +
                "values('" + kul.Ad + "','" + kul.SoyAd + "','" + kul.Eposta + "','" + kul.Sifre + "')");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
            return View("SignIn");


            

        }

        /*                   ana menü ve ARAMA KISMI                           */


        [HttpGet]
        public ActionResult Search()
        {
            Debug.WriteLine("brrrrrrr");
            return View();
        }
        [HttpPost]
        public ActionResult SearchAndFind(Urun urn)
        {
            if(gelenurun.Count > 0)
            {
                gelenurun.Clear();
            }
            try
            {
                connectionString();
                con.Open();
                SqlCommand ara = new SqlCommand("select * from Urun where tanim like '%" + urn.tanim + "%'");
                ara.Connection = con;
                dr = ara.ExecuteReader();
                while (dr.Read())
                {
                    gelenurun.Add(new Urun() { tanim = dr["tanim"].ToString(), resimURL = dr["resimURL"].ToString() });
                }
                
                con.Close();
                return View("Search");
            }
            catch (Exception)
            {
                return View("Hata");
                throw;
            }
            

        }


        /*                ConnectionString                */

        void connectionString()
        {
           
            con.ConnectionString = "Data Source = MONSTERPC; database = ProjeDB; Integrated Security = False; User Id = MyUser; Password = MyUser1234;";
        }
    }
}