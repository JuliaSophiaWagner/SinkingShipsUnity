using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogic.GameParts
{
    public class History
    {
        public History()
        {

        }

        public History(string name, int firstPlayerPoints, int secondPlayerPoints, string gameID)
        {
            this.Name = name;
            this.FirstPlayerPoints = firstPlayerPoints;
            this.SecondPlayerPoints = secondPlayerPoints;
            this.GameID = gameID;
        }

        public string Name { get; set; }

        public string GameID { get; set; }

        public int FirstPlayerPoints { get; set; }

        public int SecondPlayerPoints { get; set; }
    }
}
