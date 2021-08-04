using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eticaret_staj2.Models
{
    public class Kullanicilar
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string SoyAd { get; set; }
        public string Eposta { get; set; }
        public string Sifre { get; set; }
    }
}