using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Ship
    {
        public Ship()
        {

        }
        public Ship(int size, List<GridElement> fields, bool rotated, int xPos, int yPos)
        {
            this.Size = size;
            this.CountShot = size;
            this.Fields = fields;
            this.Rotated = rotated;
            this.PosX = xPos;
            this.PosY = yPos;
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
    }
}
