using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SinkingShipsServer.Database
{
    public class DbContextFactory : IDesignTimeDbContextFactory<SinkingShipsContext>
    {
        public SinkingShipsContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<SinkingShipsContext>();
            var connectionString = configuration.GetConnectionString("SinkingShipsDb");
            builder.UseSqlServer(connectionString);

            return new SinkingShipsContext(builder.Options);
        }
    }
}
