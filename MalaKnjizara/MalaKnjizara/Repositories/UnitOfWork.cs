using MalaKnjizara.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MalaKnjizara.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IKnjigaRepository Knjige { get; private set; }
        public INakladnikRepository Nakladnici { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Knjige = new KnjigaRepository(context);
            Nakladnici = new NakladnikRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

    }
}