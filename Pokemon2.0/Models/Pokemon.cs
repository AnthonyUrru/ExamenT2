using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon2._0.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PokeTypeId { get; set; }
        public string ImagePath { get; set; }
        public int UserId{ get; set; }
        public PokeType PokeTypes { get; set; }
    }
}
