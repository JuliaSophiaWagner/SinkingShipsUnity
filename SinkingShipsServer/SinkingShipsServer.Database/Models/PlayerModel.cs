using System;
using System.Collections.Generic;
using System.Text;

namespace SinkingShipsServer.Database.Models
{
    public class PlayerModel
    {
        public int Id { get; set; }
        public string PlayerId { get; set; }
        public string Name { get; set; }
        public string Passwort { get; set; } 
    }
}
