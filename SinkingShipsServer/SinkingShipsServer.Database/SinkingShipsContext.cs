using Microsoft.EntityFrameworkCore;
using SinkingShipsServer.Database.Models;
using System;

namespace SinkingShipsServer.Database
{
    public class SinkingShipsContext : DbContext
    {
        public SinkingShipsContext(DbContextOptions<SinkingShipsContext> options) : base(options)
        {
        }

        public DbSet<ClientData> AllRegisteredPlayers { get; set; }
    }
}
