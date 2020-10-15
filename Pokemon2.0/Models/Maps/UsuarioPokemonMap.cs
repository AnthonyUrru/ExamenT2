using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon2._0.Models.Maps
{
    public class UsuarioPokemonMap : IEntityTypeConfiguration<UsuarioPokemon>
    {
        public void Configure(EntityTypeBuilder<UsuarioPokemon> builder)
        {
            builder.ToTable("UsuarioPokemon");
            builder.HasKey(o => o.Id);
        }
    }
}
