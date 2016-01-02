using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal.Models.Repository
{
    public interface IRepositoryEx<T>: IRepository<T>
    {
        void Add(T entity);
        void Edit(T entity);
        void Delete(int id);
        IEnumerable<T> FindAnimals(int? typeId, int? fellColorId, List<Region> regionList);
    }
}
