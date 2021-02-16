using SinkingShipsGameInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogic.GameParts
{
    public class GridElement : IGridElement
    {
        public GridElement(int xCoordinate, int yCoordinate, bool isShip)
        {
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
            this.IsShip = isShip;
        }

        public GridElement()
        {

        }

        public int XCoordinate
        {
            get;
            set;
        }

        public int YCoordinate
        {
            get;
            set;
        }

        public bool HasBeenShot
        {
            get;
            set;
        }

        public bool IsShip
        {
            get;
            set;
        }
    }
}
