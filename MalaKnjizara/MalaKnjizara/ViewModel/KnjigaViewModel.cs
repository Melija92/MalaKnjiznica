using MalaKnjizara.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MalaKnjizara.ViewModel
{
    public class KnjigaViewModel
    {
        public string Naziv { get; set; }
        public int Kolicina { get; set; }
        public int BrojStranica { get; set; }
        public string JezikPisanja { get; set; }
        public string NazivNakladnika { get; set; }
        public string NazivPolice { get; set; }
    }
}