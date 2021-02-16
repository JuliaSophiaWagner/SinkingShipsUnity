using System;
using System.Collections.Generic;
using System.Text;

namespace SinkingShipsServer.Database.Models
{
    public class GridElement 
    {
        public int Id { get; set; }

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
