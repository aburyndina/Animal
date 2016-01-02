using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using Animal.Models;
using Animal.Models.Repository;

namespace Animal.Models.Repository
{
    public interface IDictionaryRepository : IRepository<AnimalType>,
        IRepository<FellColor>,
        IRepository<Location>,
        IRepository<Region>
    {
    }
}


