using System;
using System.Collections.Generic;
using System.Text;

namespace SinkingShipsServer.Database.Models
{
    public class Ship
    {
        public int Id { get; set; }
        public int Size { get; set; }

        public int CountShot { get; set; }

        public bool ShipHasSunk
        {
            get; set;
        }

        public List<GridElement> Fields
        {
            get;
            set;
        }
    }
}
