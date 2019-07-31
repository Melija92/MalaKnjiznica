using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaKnjizara.Repositories
{
    public interface IUnitOfWork
    {
        IKnjigaRepository Knjige { get; }
        void Complete();
    }
}
