using MalaKnjizara.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MalaKnjizara.Repositories
{
    public class KnjigaRepository : IKnjigaRepository
    {
        private readonly IApplicationDbContext _context_Knjiga;

        public KnjigaRepository(IApplicationDbContext context)
        {
            _context_Knjiga = context;
        }

        public List<Knjiga> VratiKnjige()
        {
            return _context_Knjiga.Knjiga
                .ToList();
        }

        public Knjiga VratiJednuKnjigu(int id)
        {
            return _context_Knjiga.Knjiga.Find(id);
        }

        public void SpremiKnjigu(Knjiga Knjiga)
        {
            _context_Knjiga.Knjiga.Add(Knjiga);
        }

        public void IzbrisiKnjigu(Knjiga Knjiga)
        {
            _context_Knjiga.Knjiga.Remove(Knjiga);
        }
    }
}