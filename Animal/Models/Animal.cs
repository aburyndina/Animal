using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace Animal.Models
{
    public class Animal
    {
        public int? Id { get; internal set; }
        public string Name { get; internal set; }
        internal int TypeId { get; set; }
        internal int ColorId { get; set; }
        internal int LocationId { get; set; }

        public AnimalType AnimalType { get; internal set; }
        public FellColor FellColor { get; internal set; }
        public Location Location { get; internal set; }

        public Animal(string name, AnimalType animalType, FellColor fellColor, Location location)
        {
            this.Name = name;
            this.TypeId = animalType.Id;
            this.ColorId = fellColor.Id;
            this.LocationId = location.Id;
            this.AnimalType = animalType;
            this.FellColor = fellColor;
            this.Location = location;
        }

        internal  Animal()
        {
        }
    }
}