using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Animal.Models.Repository
{
    public interface IDataContext
    {
        IRepository<AnimalType> AnimalType { get; }
        IRepository<FellColor> FellColor { get; }
        IRepository<Location> Location { get; }
        IRepository<Region> Region { get; }
        IRepositoryEx<Animal> Animals { get; set; }
    }
}