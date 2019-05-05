using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MalaKnjizara.Models
{
    [Table("dbo.Autorstvo")]
    public class Autorstvo
    {
        [Key]
        public int AutorstvoID { get; set; }
        [Column(Order = 1)]
        public int KnjigaID { get; set; }
        public virtual Knjiga Knjiga { get; set; }
        [Column(Order = 2)]
        public int AutorID { get; set; }
        public virtual Autor  Autor { get; set; }
        [Required(ErrorMessage = "Udio autorstva obvezan je za unos!")]
        public int UdioAutorstva { get; set; }
    }
}