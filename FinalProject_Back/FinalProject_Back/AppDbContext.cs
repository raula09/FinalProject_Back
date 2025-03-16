using FinalProject_Back.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FinalProject_Back
{
    public class AppDbContext : DbContext
    {
       public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
  
            public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
            {
            }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = "1", Name = "Laptops", Image = "https://png.pngtree.com/png-vector/20220705/ourmid/pngtree-laptop-icon-png-image_5683130.png" },
                new Category { Id = "2", Name = "Phones", Image = "https://cdn-icons-png.flaticon.com/512/0/191.png" }
            );
        }


    }

    }



