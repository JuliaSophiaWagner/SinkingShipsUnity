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

        public void AddHistory(List<ServerLogic.GameParts.History> getHistory)
        {
            foreach (var item in getHistory)
            {
                this.dbContext.History.Add(new History
                {
                   GameID = item.GameID,
                   Name = item.Name,
                   FirstPlayerPoints = item.FirstPlayerPoints,
                   SecondPlayerPoints = item.SecondPlayerPoints
                });
            }

            this.dbContext.SaveChanges();
        }

        public bool CheckIfPlayerExist(string name, string password, string token)
        {
            if (this.dbContext.AllRegisteredPlayers.Where(x => x.Name == name && x.Password == password) == null) 
            { 
                return false; 
            }
            var temp = this.dbContext.AllRegisteredPlayers.Where(x => x.Name == name && x.Password == password).First();
            temp.Token = token;
            this.dbContext.AllRegisteredPlayers.Update(temp);
            this.dbContext.SaveChanges();

            return true;
        }

        public List<ServerLogic.GameParts.History> GetAllHistory()
        {
            List<ServerLogic.GameParts.History> temp = new List<ServerLogic.GameParts.History>();

            foreach (var item in this.dbContext.History)
            {
                temp.Add(new ServerLogic.GameParts.History
                {
                    GameID = item.GameID,
                    Name = item.Name,
                    FirstPlayerPoints = item.FirstPlayerPoints,
                    SecondPlayerPoints = item.SecondPlayerPoints
                });
            }

            return temp;
        }

        public List<ServerLogic.ClientData> GetAllRegisteredPlayers()
        {
            List<ServerLogic.ClientData> temp = new List<ServerLogic.ClientData>();
            var test = this.dbContext.AllRegisteredPlayers;
            foreach (ClientData player in this.dbContext.AllRegisteredPlayers)
            {
                var temp2 = new ServerLogic.ClientData(player.ID, player.Name, player.Password);
                temp2.Token = player.Token;
                temp2.Won = player.Won;
                temp2.Points = player.Points;
                if (player.GameRequests != null)
                {
                    foreach (var item in player.GameRequests)
                    {
                        temp2.GameRequests.Add(new ServerLogic.Player
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Token = item.Token
                        });
                    }
                }
              
                temp.Add(temp2);
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

        public void SetAllHistory(List<ServerLogic.GameParts.History> histories)
        {
            throw new NotImplementedException();
        }

        public void SetAllRegisteredPlayer(List<ServerLogic.ClientData> lists)
        {
                foreach (var players in lists)
                {

                foreach (var item in players.GameRequests)
                {

                    var temp = this.dbContext.AllRegisteredPlayers.Where(x => x.ID == players.ID).First().GameRequests;
                    if (temp == null)
                    {
                        temp = new List<Player>();
                        continue;
                    }


                    if (temp.Count() < 1)
                    {
                        this.dbContext.AllRegisteredPlayers.Where(x => x.ID == players.ID).First().GameRequests.Add(new Player
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Token = item.Token
                        });
                    }

                    this.dbContext.GameRequests.Update(new Player
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Token = item.Token
                    });
                }
            }
            //List<ClientData> temp = new List<ClientData>();

            //foreach (var player in lists)
            //{
            //    List<Player> requests = new List<Player>();

            //    foreach (var item in player.GameRequests)
            //    {
            //        requests.Add(new Player
            //        {
            //            Name = item.Name,
            //            ID = item.ID,
            //            Token = item.Token
            //        });
            //    }

            //    this.dbContext.AllRegisteredPlayers.Add(new ClientData 
            //    {
            //        ID = player.ID,
            //        Name = player.Name,
            //        Password = player.Password,
            //        GameRequests = requests,
            //        Won = player.Won,
            //        Points = player.Points,
            //        Token = player.Token
            //    });
            //}

            this.dbContext.SaveChanges();
        }

        public void UpdateHistory(List<ServerLogic.GameParts.History> getHistory)
        {
            this.dbContext.History.RemoveRange(this.dbContext.History);
            //foreach (var item in getHistory)
            //{
            //    var temp = this.dbContext.History.Where(x => x.GameID == item.GameID).First();
            //    this.dbContext.History.Update(temp);
            //    //this.dbContext.History.Where(x => x.GameID == item.GameID).a                
            //}
            this.AddHistory(getHistory);
        }
    } 
}
