using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Benday.Presidents.Api.DataAccess
{
    public class PresidentsDesignTimeDbContextFactory :
            IDesignTimeDbContextFactory<PresidentsDbContext>
    {
        public PresidentsDbContext Create()
        {
            var environmentName =
                Environment.GetEnvironmentVariable(
                    "ASPNETCORE_ENVIRONMENT");

            var basePath = AppContext.BaseDirectory;

            return Create(basePath, environmentName);
        }

        public PresidentsDbContext CreateDbContext(string[] args)
        {
            return Create(
                Directory.GetCurrentDirectory(),
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        }

        private PresidentsDbContext Create(string basePath, string environmentName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetConnectionString("default");

            if (String.IsNullOrWhiteSpace(connstr) == true)
            {
                throw new InvalidOperationException(
                    "Could not find a connection string named 'default'.");
            }
            else
            {
                return Create(connstr);
            }
        }

        private PresidentsDbContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(
             $"{nameof(connectionString)} is null or empty.",
             nameof(connectionString));

            var optionsBuilder =
                 new DbContextOptionsBuilder<PresidentsDbContext>();

            Console.WriteLine(
                "PresidentsDesignTimeDbContextFactory.Create(string): Connection string: {0}",
                connectionString);

            optionsBuilder.UseSqlServer(connectionString);

            return new PresidentsDbContext(optionsBuilder.Options);
        }
    }
}
