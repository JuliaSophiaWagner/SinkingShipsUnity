using SinkingShipsGameInterfaces;
using System.Collections.Generic;

namespace SinkingShipsGameInterfaces
{
    public interface IGameField
    {
        List<IGridElement> Fields { get; set; }
        List<IShip> Ships { get; set; }
    }
}