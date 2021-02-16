using SinkingShipsServer.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SinkingShipsServer.Database.Repositories
{
    public interface ISinkingShipsRepository
    {
        IEnumerable<PlayerModel> GetAllPlayers();

        void AddPlayer(PlayerModel player);

        bool GetPlayer(string idClient);
        bool CheckIfPlayerExist(string name, string password);
        void RemoveGameInstance(int id);
    }
}
