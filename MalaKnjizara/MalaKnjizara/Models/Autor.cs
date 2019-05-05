using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MalaKnjizara.Models
{
    [Table("dbo.Autor")]
    public class Autor
    {
        [Key]
        public int AutorID { get; set; }
        [Required(ErrorMessage = "OIB autora obvezan je za unos!")]
        [RegularExpression(@"\d{11}", ErrorMessage = "OIB nije ispravan")]
        public string OIB { get; set; }
        [Required(ErrorMessage = "Ime autora obvezan je za unos!")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime autora obvezan je za unos!")]
        public string Prezime { get; set; }
        public ICollection<Autorstvo> Iznajmljivanja { get; set; }
    }
}