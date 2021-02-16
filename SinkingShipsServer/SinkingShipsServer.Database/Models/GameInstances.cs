
using System;
using System.Collections.Generic;
using System.Text;

namespace SinkingShipsServer.Database.Models
{
    public class GameInstances
    {

        public int Id { get; set; }
        public List<GridElement> Fields
        {
            get;
            set;
        }


        public List<Ship> Ships
        {
            get;
            set;
        }
    }
}
