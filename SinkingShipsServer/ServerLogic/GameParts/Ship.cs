using SinkingShipsGameInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogic.GameParts
{
    public class Ship
    {
        public Ship(int size)
        {
            this.Fields = new List<GridElement>();
            this.Size = size;
            this.CountShot = this.Size;
        }
        public Ship()
        {

        }
        public int Size { get; }

        public int CountShot { get; set; }

        public bool Rotated { get; set; }
        public int PosX { get; set; }

        public int PosY { get; set; }
        public List<GridElement> Fields
        {
            get;
            set;
        }

        public bool ShipHasSunk
        {
            get
            {
                return this.CountShot == 0;
            }
        }

        
        public void ShipWasShot()
        {
            this.CountShot--;
        }
    }
}
