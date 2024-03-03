using Benday.Presidents.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Benday.Presidents.Api.DataAccess
{
    public class PresidentsDbContext : DbContext, IPresidentsDbContext
    {

        public PresidentsDbContext(DbContextOptions options) :
            base(options)
        {

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonFact> PersonFacts { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }

        public override int SaveChanges()
        {
            CleanupOrphanedPersonFacts();
            CleanupOrphanedRelationships();

            return base.SaveChanges();
        }

        private void CleanupOrphanedPersonFacts()
        {
            var deleteThese = new List<PersonFact>();

            foreach (var deleteThis in PersonFacts.Local.Where(pf => pf.Person == null))
            {
                deleteThese.Add(deleteThis);
            }

            foreach (var deleteThis in deleteThese)
            {
                PersonFacts.Remove(deleteThis);
            }
        }

        private void CleanupOrphanedRelationships()
        {
            var deleteThese = new List<Relationship>();

            foreach (var deleteThis in Relationships.Local
                .Where(r => r.FromPerson == null))
            {
                deleteThese.Add(deleteThis);
            }

            foreach (var deleteThis in deleteThese)
            {
                Relationships.Remove(deleteThis);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");
            });

            modelBuilder.Entity<PersonFact>().ToTable("PersonFact");

            modelBuilder.Entity<Feature>().ToTable("Feature");

            modelBuilder.Entity<LogEntry>().ToTable("LogEntry");

            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.ToTable("Relationship");

                entity.Property(e => e.RelationshipType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.FromPerson)
                    .WithMany(p => p.Relationships)
                    .HasForeignKey(d => d.FromPersonId)
                    .OnDelete(DeleteBehavior.Restrict);


                // THE PROBLEM IS HERE
                // SOMEHOW NEED TO MAP THE "TO" RELATIONSHIP
                entity.HasOne(d => d.ToPerson);
            });
        }
    }
}
