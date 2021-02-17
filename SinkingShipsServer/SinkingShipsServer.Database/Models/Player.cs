using System;
using System.Collections.Generic;
using System.Text;

namespace SinkingShipsServer.Database.Models
{
    public class Player
    {
        public int Id
        {
            get;
            set;
        }

        public string Name { get; set; }

        public string ID { get; set; }

        public string Token { get; set; }
    }
}
