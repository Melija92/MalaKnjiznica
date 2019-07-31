using MalaKnjizara.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaKnjizara.Repositories
{
    public interface IKnjigaRepository
    {
        List<Knjiga> VratiKnjige();

        Knjiga VratiJednuKnjigu(int id);

        void SpremiKnjigu(Knjiga Knjiga);

        void IzbrisiKnjigu(Knjiga Knjiga);
    }
}
