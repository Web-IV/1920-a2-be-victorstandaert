using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MetingApi.Models;
using System;
using Project.Models;

namespace MetingApi.Data
{
    public class MetingContext : IdentityDbContext
    {
        public MetingContext(DbContextOptions<MetingContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Meting>()
                .HasMany(p => p.Resultaten)
                .WithOne()
                .IsRequired()
                .HasForeignKey("MetingId"); //Shadow property

            builder.Entity<Meting>()
                .HasOne(p => p.User)
                .WithMany(b => b.Metingen);
            
            builder.Entity<Resultaat>().Property(r => r.Vraag).IsRequired().HasMaxLength(20);

            builder.Entity<User>().Property(c => c.LastName).IsRequired().HasMaxLength(50);
            builder.Entity<User>().Property(c => c.FirstName).IsRequired().HasMaxLength(50);
            builder.Entity<User>().Property(c => c.Email).IsRequired().HasMaxLength(100);
            builder.Entity<User>().Ignore(c => c.Metingen);


            //Another way to seed the database
            builder.Entity<Meting>().HasData(
                 new Meting { Id = 1, Created = DateTime.Now},
                 new Meting { Id = 2, Created = Convert.ToDateTime("2020-03-26T08:31:19.0000000") }
  );

            builder.Entity<Resultaat>().HasData(
                    //Shadow property can be used for the foreign key, in combination with anaonymous objects
                    new { Id = 1, Vraag = "Werk", Amount = (double?)75, MetingId = 1 },
                    new { Id = 2, Vraag = "Familie", Amount = (double?)15, MetingId = 1 },
                    new { Id = 3, Vraag = "Gezondheid", Amount = (double?)10, MetingId = 1 }
                 );
        }

        public DbSet<Meting> Metingen { get; set; }
        public DbSet<User> users { get; set; }
    }
}