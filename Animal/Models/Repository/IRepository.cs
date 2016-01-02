using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal.Models.Repository
{
    public interface IRepository<T>
    {
        T GetbyId(int id);
        IEnumerable<T> GetAll();
    }
}
