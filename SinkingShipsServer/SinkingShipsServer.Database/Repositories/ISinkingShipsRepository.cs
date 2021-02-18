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
        bool CheckIfPlayerExist(string name, string password);
        public List<ServerLogic.ClientData> GetAllRegisteredPlayers();

    }
}
