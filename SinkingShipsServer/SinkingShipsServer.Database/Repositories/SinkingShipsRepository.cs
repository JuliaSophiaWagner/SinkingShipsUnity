using SinkingShipsServer.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinkingShipsServer.Database.Repositories
{
    public class SinkingShipsRepository : ISinkingShipsRepository
    {
        private SinkingShipsContext dbContext;

        public SinkingShipsRepository(SinkingShipsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddPlayer(PlayerModel player)
        {
            this.dbContext.SinkingShipsPlayers.Add(player);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<PlayerModel> GetAllPlayers()
        {
           return this.dbContext.SinkingShipsPlayers;
        }

        public bool GetPlayer(string idClient)
        {
            return this.dbContext.SinkingShipsPlayers.Any(x => x.PlayerId == idClient);
        }

        public bool CheckIfPlayerExist(string name, string password)
        {
            return this.dbContext.SinkingShipsPlayers.Any(x => (x.Name == name && x.Passwort == password));
       
        }

        public void RemoveGameInstance(int id)
        {
          GameInstances temp =  this.dbContext.GameInstances.Where(x => x.Id == id).FirstOrDefault();
          this.dbContext.GameInstances.Remove(temp);
        }
    } 
}
