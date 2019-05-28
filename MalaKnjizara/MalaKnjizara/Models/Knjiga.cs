using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MalaKnjizara.Models
{
    [Table("dbo.Knjiga")]
    public class Knjiga
    {
        [Key]
        public int KnjigaID { get; set; }
        [Required(ErrorMessage = "Naziv knjige obvezno je za unos!")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Količina knjiga obavezna je za unos!")]
        public int Kolicina { get; set; }
        [Required(ErrorMessage = "Broj stranica obvezan je za unos!")]
        public int BrojStranica { get; set; }
        [Required(ErrorMessage = "Cijena je obvezna za unos!")]
        public decimal Cijena { get; set; }
        public string JezikPisanja { get; set; }
        public int NakladnikID { get; set; }
        public virtual Nakladnik Nakladnik { get; set; }

        public int PolicaID { get; set; }
        public Polica Polica { get; set; }
        public IEnumerable<Autorstvo> Autorstva { get; set; }
    }
}