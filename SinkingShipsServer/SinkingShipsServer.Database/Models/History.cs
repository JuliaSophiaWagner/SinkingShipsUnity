using System;
using System.Collections.Generic;
using System.Text;

namespace SinkingShipsServer.Database.Models
{
    public class History
    {
        public int Id
        {
            get;
            set;
        }

        public string Name { get; set; }

        public int FirstPlayerPoints { get; set; }

        public int SecondPlayerPoints { get; set; }
    }
}
