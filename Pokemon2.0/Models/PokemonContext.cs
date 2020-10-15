using Microsoft.EntityFrameworkCore;
using Pokemon2._0.Models;
using Pokemon2._0.Models.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon2._0.Models
{
    public class PokemonContext : DbContext
    {
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<PokeType> PokeTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UsuarioPokemon> UsuarioPokemons { get; set; }
        public PokemonContext(DbContextOptions<PokemonContext> options)
           : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Esto se hace por cada tabla de base de datos
            modelBuilder.ApplyConfiguration(new PokemonMap());
            modelBuilder.ApplyConfiguration(new PokeTypeMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new UsuarioPokemonMap());

        }

    }
}
