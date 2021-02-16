namespace SinkingShipsGameInterfaces
{
    public interface IGridElement
    {
        bool HasBeenShot { get; set; }
        bool IsShip { get; set; }
        int XCoordinate { get; set; }
        int YCoordinate { get; set; }
    }
}