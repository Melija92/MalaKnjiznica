using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaKnjizara.Models
{
    public interface IApplicationDbContext
    {
        DbSet<Knjiga> Knjiga { get; set; }
        DbSet<Nakladnik> Nakladnik { get; set; }
    }
}
