using Benday.Presidents.Api.DataAccess;
using Benday.Presidents.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Benday.Presidents.Common
{
    public interface IPresidentsDbContext
    {
        DbSet<Person> Persons { get; set; }
        DbSet<PersonFact> PersonFacts { get; set; }
        DbSet<Relationship> Relationships { get; set; }
        DbSet<Feature> Features { get; set; }
        DbSet<LogEntry> LogEntries { get; set; }
        EntityEntry Entry(object entity);
        int SaveChanges();
    }
}
