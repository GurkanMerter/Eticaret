using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eticaret_staj2.Models
{
    public class Urun
    {
        public int Id { get; set; }
        public int markaId { get; set; }
        public string tanim { get; set; }
        public string resimURL { get; set; }
    }
}