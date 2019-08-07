using MalaKnjizara.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MalaKnjizara.Repositories
{
    public class NakladnikRepository : INakladnikRepository
    {
        private readonly IApplicationDbContext _context_Nakladnik;

        public NakladnikRepository(IApplicationDbContext context)
        {
            _context_Nakladnik = context;
        }

        public List<Nakladnik> VratiNakladnike()
        {
            return _context_Nakladnik.Nakladnik
                .ToList();
        }

        public Nakladnik VratiJednogNakladnika(int id)
        {
            return _context_Nakladnik.Nakladnik.Find(id);
        }

        public void SpremiNakladnika(Nakladnik nakladnik)
        {
            _context_Nakladnik.Nakladnik.Add(nakladnik);
        }

        public void IzbrisiNakladnika(Nakladnik nakladnik)
        {
            _context_Nakladnik.Nakladnik.Remove(nakladnik);
        }
    }
}