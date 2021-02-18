using Microsoft.AspNetCore.Mvc;
using ServerLogic;
using ServerLogic.GameParts;
using ServerLogic.Login;
using System.Collections;
using System.Collections.Generic;

namespace SinkingShipsServer.Services
{
    public interface ISinkingShipsService
    {
        void Updatedata(List<ClientData> players, List<History> histories);
        bool LoginPlayer(PlayerCredentials player, string token);
        ClientData RegisterPlayer(PlayerCredentials player);
        bool SetGameField(GameField gameField, string token);
        GameInformation StartGame(Player player, string token);
        List<ClientData> GetAllPlayers();
        List<ClientData> GetAllLoggedInPlayers();
        bool SetGameShot(PlayerShot shot, string token);
        List<GameInformation> GetAllRunningGames(string token);
        List<Player> GetAllGameRequests(string token);
        bool SaveGameRequest(string clientID, Player clientToAsk);
        GameField GetRunningGameData(string token, string gameID);
        GameInformation StartBotGame(string token);
        GameData GetGameData(string token, GameInformation info);
        GameInformation GetGameInfo(string gameID, string token);
        List<PlayerStats> GetRanking(string token);
        List<History> GetHistory(string token);
        List<PlayerStats> GetRankingWon(string token);
        List<ClientData> GetRegisteredPlayer();
        List<History> GetHistory();
    }
}