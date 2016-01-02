using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Animal.Models.Repository
{
    public class SqlDataContext : IDataContext
    {
        public IRepository<AnimalType> AnimalType { get; private set; }
        public IRepository<FellColor> FellColor { get; private set; }
        public IRepository<Location> Location { get; private set; }
        public IRepository<Region> Region { get; private set; }

        public IRepositoryEx<Animal> Animals { get; set; }

        public SqlDataContext()
        {
            SqlAnimalRepository repository = new SqlAnimalRepository();
        
            AnimalType = repository;
            FellColor = repository;
            Location = repository;
            Region = repository;
            Animals = repository;
        }
    }
}