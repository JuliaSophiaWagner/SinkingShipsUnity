using Microsoft.AspNetCore.Mvc;
using ServerLogic;
using ServerLogic.GameParts;
using ServerLogic.Login;
using SinkingShipsServer.Database.Models;
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

        public List<ClientData> GetAllPlayers()
        {
            return this.manager.AllRegisteredPlayers;
        }

        public List<ClientData> GetAllLoggedInPlayers()
        {
            return this.manager.LoggedPlayers;
        }

        public bool GetPlayerbyId(string idClient)
        {
            throw new NotImplementedException();
        }

        public bool LoginPlayer(PlayerCredentials player, string token)
        {
            return this.manager.LoginPlayer(player, token);
        }

        public ClientData RegisterPlayer(PlayerCredentials player)
        {
            if (this.manager.AllRegisteredPlayers.Any(client => client.Name == player.Name) || player.Name == string.Empty || player.Name == null || player.Password == string.Empty || player.Password == null)
            {
                // already registered or name/password not valid
                return null;
            }

            var playermodel = this.manager.RegisterPlayer(player);
            return playermodel;
        }

        public void PauseGame(PlayerCredentials credentials, string token)
        {
            this.manager.PauseGame(credentials, token); 
        }

        public void SetGameField(GameField gameField, string token)
        {
            this.manager.SetGameField(gameField, this.GetClientIDByToken(token));
        }

        public GameData GetGameData(string token, GameInformation info)
        {
            // find player anhand des token
            return this.manager.GetGameData(this.GetClientIDByToken(token), info);
        }

        public bool SetGameShot(PlayerShot shot, string token)
        {
            bool valid = this.manager.SetGameShot(shot, this.GetClientIDByToken(token));
            // save manager players and field to db
            // rep.SaveGame()
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

        public bool RequestGameSpecificUser(Player player, string token)
        {
            throw new NotImplementedException();
        }

        public List<GameInformation> GetAllRunningGames(string token)
        {
            // TODO: token checken !!!         

            return this.manager.GetAllRunningGames(this.GetClientIDByToken(token));
        }

        private string GetClientIDByToken(string token)
        {
            ClientData player = this.manager.LoggedPlayers.Find(x => x.Token == token);
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

        public void SaveGameRequest(string token, Player clientToAsk)
        {
            this.manager.SaveGameRequest(this.GetClientIDByToken(token), clientToAsk);
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
            return this.manager.RunningGames.Where(x => x.GameInformation.GameID == gameID).FirstOrDefault().GameInformation;
        }

        public List<PlayerStats> GetRanking(string token)
        {
            //todo token checken!!

            return this.manager.GetRanking(token);
        }

        public List<PlayerStats> GetRankingWon(string token)
        {
            //todo token checken!!

            return this.manager.GetRankingWon(token);
        }

        public List<History> GetHistory(string token)
        {
            return this.manager.AllRegisteredPlayers.Where(x => x.ID == this.GetClientIDByToken(token)).First().History;
        }
    }
}