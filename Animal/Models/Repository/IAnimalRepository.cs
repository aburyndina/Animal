using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Animal.Models.Repository
{
    public interface IAnimalRepository : IRepository<Animal>
    {
        void AddAnimal(Animal animal);
        void EditAnimal(Animal animal);
        void DeleteAnimal(int id);
        IEnumerable<Animal> FindAnimals(AnimalType type, FellColor fellColor, List<Region> regionList);
        Animal GetbyId(int id);
    }
}