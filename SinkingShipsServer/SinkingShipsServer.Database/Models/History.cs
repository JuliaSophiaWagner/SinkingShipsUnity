using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SinkingShipsServer.Database.Models
{
    public class History
    {
        [Key]
        public int Id
        {
            get;
            set;
        }

        public string Name { get; set; }

        public string GameID { get; set; }

        public int FirstPlayerPoints { get; set; }

        public int SecondPlayerPoints { get; set; }
    }
}
