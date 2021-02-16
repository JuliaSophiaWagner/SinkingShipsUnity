using System.Collections.Generic;

namespace SinkingShipsGameInterfaces
{
    public interface IShip
    {
        int CountShot { get; set; }
        List<(int, int)> Fields { get; set; }
        bool ShipHasSunk { get; }
        int Size { get; }
    }
}