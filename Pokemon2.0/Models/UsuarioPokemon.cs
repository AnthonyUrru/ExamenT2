using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon2._0.Models
{
    public class UsuarioPokemon
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PokemonId { get; set; }
        public DateTime fecha { get; set; }
    }
}
