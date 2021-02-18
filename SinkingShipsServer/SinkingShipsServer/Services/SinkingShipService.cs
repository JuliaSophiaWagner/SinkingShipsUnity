using Microsoft.AspNetCore.Mvc;
using ServerLogic;
using ServerLogic.GameParts;
using ServerLogic.Login;
using SinkingShipsServer.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinkingShipsServer.Services
{
    public class SinkingShipService : ISinkingShipsService
    {
        private SinkingShipManager manager;

        public SinkingShipService()
        {
            this.manager = new SinkingShipManager();
        }

        public List<ServerLogic.ClientData> GetAllPlayers()
        {
            return this.manager.AllRegisteredPlayers;
        }

        public List<ServerLogic.ClientData> GetAllLoggedInPlayers()
        {
            return this.manager.LoggedPlayers;
        }

        public bool LoginPlayer(PlayerCredentials player, string token)
        {
            return this.manager.LoginPlayer(player, token);
        }

        public ServerLogic.ClientData RegisterPlayer(PlayerCredentials player)
        {
            if (this.manager.AllRegisteredPlayers.Any(client => client.Name == player.Name) || player.Name == string.Empty || player.Name == null || player.Password == string.Empty || player.Password == null)
            {
                // already registered or name/password not valid
                return null;
            }

            var playermodel = this.manager.RegisterPlayer(player);
            return playermodel;
        }

        public bool SetGameField(GameField gameField, string token)
        {
            string playerID = this.GetClientIDByToken(token);
            this.manager.SetGameField(gameField, playerID);
            return playerID == null || playerID == string.Empty;
        }

        public GameData GetGameData(string token, GameInformation info)
        {
            return this.manager.GetGameData(this.GetClientIDByToken(token), info);
        }

        public bool SetGameShot(PlayerShot shot, string token)
        {
            bool valid = this.manager.SetGameShot(shot, this.GetClientIDByToken(token));
            return valid;
        }

        public GameInformation StartGame(Player player, string tokenOtherPlayer)
        {
            return this.manager.StartGame(player, this.GetClientIDByToken(tokenOtherPlayer));
        }

        public GameInformation StartBotGame(string token)
        {
            return this.manager.StartBotGame(this.GetClientIDByToken(token));
        }


        public List<GameInformation> GetAllRunningGames(string token)
        {
            return this.manager.GetAllRunningGames(this.GetClientIDByToken(token));
        }

        private string GetClientIDByToken(string token)
        {
            ServerLogic.ClientData player = this.manager.LoggedPlayers.Find(x => x.Token == token);
            if (player== null)
            {
                return string.Empty;
            }

            return player.ID;
        }

        public List<Player> GetAllGameRequests(string token)
        {
            return this.manager.GetAllGameRequests(this.GetClientIDByToken(token));
        }

        public bool SaveGameRequest(string token, Player clientToAsk)
        {
            var player = this.GetClientIDByToken(token);
            this.manager.SaveGameRequest(this.GetClientIDByToken(token), clientToAsk);

            return player == null;
        }

        public GameField GetRunningGameData(string token, string gameID)
        {
            try
            {
                Game game = this.manager.RunningGames.Where(x => x.GameInformation.GameID == gameID).FirstOrDefault();
                string playerID = this.GetClientIDByToken(token);
                GameField field = game.Players.Where(x => x.User.ID == playerID).FirstOrDefault().Field;
                field.EnemyField = game.Players.Where(x => x.User.ID != playerID).FirstOrDefault().Field.Fields;
                field.GameID = gameID;
                return field;
            }
            catch
            {
                //not successfull
                return null;
            }
        }

        public GameInformation GetGameInfo(string gameID, string token)
        {
            var game = this.manager.RunningGames.Where(x => x.GameInformation.GameID == gameID).FirstOrDefault();
            
            if (game == null)
            {
                return null;
            }

            return game.GameInformation;
        }

        public List<PlayerStats> GetRanking(string token)
        {
            if (this.GetClientIDByToken(token) == null)
            {
                return null;
            }

            return this.manager.GetRanking(token);
        }

        public List<PlayerStats> GetRankingWon(string token)
        {
            if (this.GetClientIDByToken(token) == null)
            {
                return null;
            }

            return this.manager.GetRankingWon(token);
        }

        public List<History> GetHistory(string token)
        {
            return this.manager.History;
        }

        public List<History> GetHistory()
        {
            return this.manager.History;
        }

        public void Updatedata(List<ClientData> players, List<History> histories)
        {
            var registeredplayer = this.manager.AllRegisteredPlayers;
            this.manager.AllRegisteredPlayers = players;

            foreach (var item in registeredplayer)
            {
                this.manager.AllRegisteredPlayers.Where(x => x.ID == item.ID).First().GameRequests = item.GameRequests;
            }

            this.manager.History = histories;
        }

        public List<ClientData> GetRegisteredPlayer()
        {
            return this.manager.AllRegisteredPlayers;
        }
    }
}