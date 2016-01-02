using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Animal.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
    
        public Region Region { get; set; }
       
    }
}