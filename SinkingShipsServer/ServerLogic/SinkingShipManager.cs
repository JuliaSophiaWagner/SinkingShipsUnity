using ServerLogic.GameParts;
using ServerLogic.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ServerLogic
{
    public class SinkingShipManager
    {
        public SinkingShipManager()
        {
            this.AllRegisteredPlayers = new List<ClientData>();
            this.LoggedPlayers = new List<ClientData>();
            this.RunningGames = new List<Game>();
            this.History = new List<History>();
        }

        public List<ClientData> AllRegisteredPlayers { get; set; }
        public List<ClientData> LoggedPlayers { get; private set; }
        public List<Game> RunningGames { get; private set; }

        public List<History> History { get; set; }
       
        public bool SetGameShot(PlayerShot shot, string id)
        {
            Game game = this.RunningGames.Where(x => x.GameInformation.GameID == shot.GameID).FirstOrDefault();

            if (game == null)
            {
                return false;
            }

            bool valid = game.ExecutePlayerShot(shot, id);

            if (valid) //if a ship is hit, adds one point to the score
            {
                if (shot.IsShip)
                {
                    this.AllRegisteredPlayers.Where(x => x.ID == id).FirstOrDefault().Points++;
                }
            }

            if (game.IsBot)
            {
                game.BotMove();
            }

            this.History.Where(x => x.GameID == game.GameInformation.GameID).First().FirstPlayerPoints = 10 - game.Players.First().ShipsLeft;
            this.History.Where(x => x.GameID == game.GameInformation.GameID).First().SecondPlayerPoints = 10 - game.Players.Last().ShipsLeft;

            if (game.GameOver)
            {
                var winner = this.AllRegisteredPlayers.Where(y => y.ID == game.Players.Where(x => x.HasWon).First().User.ID).First();
                if (winner != null)
                {
                    winner.Won++;
                }

                Thread.Sleep(5000);
                this.RunningGames.Remove(game);
            }

            return valid;
        }

        public void SetGameField(GameField field, string id)
        {
            Game game = this.RunningGames.Where(x => x.GameInformation.GameID == field.GameID).FirstOrDefault();
            game.AddField(field, id);
        } 

        public GameData GetGameData(string id, GameInformation game)
        {
            Game gameTemp = this.RunningGames.Where(x => x.GameInformation.GameID == game.GameID).FirstOrDefault();

            if (gameTemp == null)
            {
                return null;
            }

            return gameTemp.GetGameData(id);
        }

        public ClientData RegisterPlayer(PlayerCredentials credentials)
        {            
            ClientData player = new ClientData(Guid.NewGuid().ToString(), credentials.Name, credentials.Password);
            this.AllRegisteredPlayers.Add(player);

            return player;            
        }

        public List<GameInformation> GetAllRunningGames(string clientID)
        {
            List<GameInformation> gameData = new List<GameInformation>();

            foreach (var game in this.RunningGames)
            {
                if (game.Players.Any(x => x.User.ID == clientID))
                {
                    gameData.Add(game.GameInformation);
                }
            }

            //TODO save game in database
            return gameData;
        }

        public GameInformation StartBotGame(string id)
        {
            Game game = new Game(this.GetPlayer(id), new BotPlayer(Guid.NewGuid().ToString()), true);
            this.AllRegisteredPlayers.Add(game.Players.Last().User);
            this.History.Add(new History(game.Players.First().User.Name + " vs. " + game.Players.Last().User.Name, 0, 0, game.GameInformation.GameID));
            game.Start();
            this.RunningGames.Add(game);
            return game.GameInformation;
        }

        public void SaveGameRequest(string clientID, Player clientToAsk)
        {
            this.GetPlayer(clientToAsk.ID).GameRequests.Add(new Player(this.GetPlayer(clientID).Name, this.GetPlayer(clientID).Token, this.GetPlayer(clientID).ID));
        }


        private ClientData GetPlayer(string id)
        {
            return this.AllRegisteredPlayers.Where(x => x.ID == id).FirstOrDefault();
        }

        public GameInformation StartGame(Player player, string iDplayerTwo)
        {
            if (player == null || this.GetPlayer(iDplayerTwo) == null)
            {
                return null;
            }

            Game game = new Game(this.GetPlayer(player.ID), this.GetPlayer(iDplayerTwo));
            this.RunningGames.Add(game);
            var playerTemp = this.AllRegisteredPlayers.Where(x => x.ID == iDplayerTwo).FirstOrDefault();

            if (playerTemp != null)
            {
                playerTemp.GameRequests.Remove(playerTemp.GameRequests.Where(x => x.ID == player.ID).FirstOrDefault());
            }

            this.History.Add(new History(game.Players.First().User.Name + " vs. " + game.Players.Last().User.Name, 0, 0, game.GameInformation.GameID));
            game.Start();
            return game.GameInformation;
        }

        public List<Player> GetAllGameRequests(string id)
        {
            if (id == string.Empty)
            {
                return null;
            }

            var temp = this.AllRegisteredPlayers.Where(x => x.ID == id).FirstOrDefault().GameRequests;

            return this.AllRegisteredPlayers.Where(x => x.ID == id).FirstOrDefault().GameRequests;
        }

        public List<PlayerStats> GetRanking(string token)
        {
            List<PlayerStats> temp = new List<PlayerStats>();

            foreach (var item in this.AllRegisteredPlayers)
            {
                PlayerStats playerStat = new PlayerStats();

                playerStat.Name = item.Name;
                playerStat.Points = item.Points;

                temp.Add(playerStat);
            }

           return temp.OrderByDescending(x => x.Points).ToList();
        }

        public List<PlayerStats> GetRankingWon(string token)
        {
            List<PlayerStats> temp = new List<PlayerStats>();

            foreach (var item in this.AllRegisteredPlayers)
            {
                PlayerStats playerStat = new PlayerStats();

                playerStat.Name = item.Name;
                playerStat.Points = item.Won;

                temp.Add(playerStat);
            }

            return temp.OrderByDescending(x => x.Points).ToList();
        }

        public bool LoginPlayer(PlayerCredentials credentials, string token)
        {
            ClientData player = this.AllRegisteredPlayers.Where(x => x.Name == credentials.Name && x.Password == credentials.Password).FirstOrDefault();
            
            if (player == null)
            {
                return false; 
            }

            this.AllRegisteredPlayers.Where(x => x.Name == credentials.Name && x.Password == credentials.Password).FirstOrDefault().Token = token;

            if (this.LoggedPlayers.Where(x => x.Name == credentials.Name && x.Password == credentials.Password).FirstOrDefault() != null)
            {
                this.LoggedPlayers.Where(x => x.Name == credentials.Name && x.Password == credentials.Password).FirstOrDefault().Token = token;
                return true;
            }

            player.Token = token;
            this.LoggedPlayers.Add(player);

            return true;
        }
    }
}
