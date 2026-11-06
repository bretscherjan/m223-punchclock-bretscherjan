using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace M223PunchclockDotnet.Model {

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options): base(options){

        }

        public DbSet<Entry> Entries {get; set;}
        public DbSet<Category> Categories {get; set;}
        public DbSet<Tag> Tags {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Entry>().ToTable("Entry");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Tag>().ToTable("Tag");

            // 1:n – viele Entries referenzieren eine Category
            modelBuilder.Entity<Entry>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Entries)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // n:m – Entries und Tags über die Tabelle "EntryTag" (Spalten EntryId, TagId)
            modelBuilder.Entity<Entry>()
                .HasMany(e => e.Tags)
                .WithMany(t => t.Entries)
                .UsingEntity<Dictionary<string, object>>(
                    "EntryTag",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    j => j.HasOne<Entry>().WithMany().HasForeignKey("EntryId"),
                    j => j.HasKey("EntryId", "TagId"));
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseNpgsql("Host=db;Database=postgres;Username=postgres;Password=postgres");
    }
}
