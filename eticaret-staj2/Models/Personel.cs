using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eticaret_staj2.Models
{
    public class Personel
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public int id { get; set; }
        public string Eposta { get; set; }
        public string Sifre { get; set; }
        public byte Yetki { get; set; }
    }
}