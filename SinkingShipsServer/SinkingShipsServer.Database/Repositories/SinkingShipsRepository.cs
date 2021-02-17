using ServerLogic.Login;
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

        public bool CheckIfPlayerExist(string name, string password)
        {
            if (this.dbContext.AllRegisteredPlayers.FirstOrDefault() == null) 
            { 
                return false; 
            } 

            return true;
        }

        public List<ServerLogic.ClientData> GetAllRegisteredPlayers()
        {
            List<ServerLogic.ClientData> temp = new List<ServerLogic.ClientData>();

            foreach(var player in this.dbContext.AllRegisteredPlayers)
            {
                temp.Add(new ServerLogic.ClientData(player.ID, player.Name, player.Password));
            }

            return temp;
        }

        public void RegisterPlayer(ServerLogic.ClientData player)
        {
            this.dbContext.AllRegisteredPlayers.Add(new ClientData
            {
                Name = player.Name,
                ID = player.ID,
                Password = player.Password
            });

            this.dbContext.SaveChanges();
        }
    } 
}
