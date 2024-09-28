using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Data
{
    public interface IUnitOfWork : IDisposable
    {
       

        IGenericRepository<T> AsyncRepositories<T>()
           where T : class;
        int save();
    }
}
