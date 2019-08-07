using MalaKnjizara.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaKnjizara.Repositories
{
    public interface INakladnikRepository
    {
        List<Nakladnik> VratiNakladnike();

        Nakladnik VratiJednogNakladnika(int id);

        void SpremiNakladnika(Nakladnik Nakladnik);

        void IzbrisiNakladnika(Nakladnik Nakladnik);
    }
}
