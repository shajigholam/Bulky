using System;
using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
	// DbContext is a built-in root class of entity framework core which we will be accessing entity framework from it
	public class ApplicationDbContext : DbContext
	{
		// pass the contection string to DbContext
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

        //create table (dotnet ef migrations add AddCategoryTableToDb)
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Create new recordes (dotnet ef migrations add SeedCategoryTable)
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );
        }
    }
}

