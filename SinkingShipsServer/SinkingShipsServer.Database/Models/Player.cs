using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SinkingShipsServer.Database.Models
{
    public class Player
    {
        [Key]
        public int PrimaryKey
        {
            get;
            set;
        }

        public string Name { get; set; }

        public string ID { get; set; }

        public string Token { get; set; }
    }
}
