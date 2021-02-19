using ServerLogic.Login;
using SinkingShipsServer.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using GameParts = ServerLogic.GameParts;

namespace SinkingShipsServer.Database.Repositories
{
    public interface ISinkingShipsRepository
    {
        void RegisterPlayer(ServerLogic.ClientData player);
        bool CheckIfPlayerExist(string name, string password, string token);
        List<ServerLogic.ClientData> GetAllRegisteredPlayers();
        void SetAllRegisteredPlayer(List<ServerLogic.ClientData> lists);
        List<GameParts.History> GetAllHistory();
        void SetAllHistory(List<GameParts.History> histories);
        void AddHistory(List<GameParts.History> getHistory);
        void UpdateHistory(List<GameParts.History> getHistory);
        void UpdatePoints(ServerLogic.ClientData data);
    }
}
