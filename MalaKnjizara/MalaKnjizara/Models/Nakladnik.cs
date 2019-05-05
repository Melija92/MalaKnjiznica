using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MalaKnjizara.Models
{
    [Table("dbo.Nakladnik")]
    public class Nakladnik
    {
        [Key]
        public int NakladnikID { get; set; }
        [Required(ErrorMessage = "OIB nakladnika obvezan je za unos!")]
        public string OIB { get; set; }
        [Required(ErrorMessage = "Naziv nakladnika obvezan je za unos!")]
        [RegularExpression(@"\d{11}", ErrorMessage = "OIB nije ispravan")]
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public ICollection<Knjiga> Knjige { get; set; }
    }
}