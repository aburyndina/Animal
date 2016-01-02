using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Animal.Models.Repository;

namespace Animal.Models
{
    public static class Refference
    {
        public static IEnumerable<AnimalType> GetAnimalType(IDictionaryRepository repository)
        {
            return ((IRepository<AnimalType>) repository).GetAll();
        }

        public static IEnumerable<FellColor> GetFellColor(IDictionaryRepository repository)
        {
            return ((IRepository<FellColor>)repository).GetAll();
        }

        public static IEnumerable<Location> GetLocation(IDictionaryRepository repository)
        {
            return ((IRepository<Location>)repository).GetAll();
        }

        public static IEnumerable<Region> GetRegion(IDictionaryRepository repository)
        {
            return ((IRepository<Region>)repository).GetAll();
        }
    }
}