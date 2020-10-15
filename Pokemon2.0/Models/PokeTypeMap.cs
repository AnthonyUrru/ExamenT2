using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pokemon2._0.Models
{
    public class PokeTypeMap : IEntityTypeConfiguration<PokeType>
    {
        public void Configure(EntityTypeBuilder<PokeType> builder)
        {
            builder.ToTable("PokeType");
            builder.HasKey(o => o.Id);
        }
    }
}
