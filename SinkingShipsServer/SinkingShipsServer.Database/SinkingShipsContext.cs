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

        public DbSet<PlayerModel> SinkingShipsPlayers { get; set; }

        public DbSet<GameInstances> GameInstances { get; set; }
    }
}
