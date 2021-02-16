using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogic
{
    public class GameInformation
    {
        public GameInformation()
        {

        }
        public GameInformation(Player firstPlayer, Player secondPlayer, int points)
        {
            this.FirstPlayer = firstPlayer;
            this.SecondPlayer = secondPlayer;
            this.Points = points;
            this.GameID = Guid.NewGuid().ToString();
        }

        public string GameID { get; set; }

        public Player FirstPlayer { get; set; }
        public Player SecondPlayer { get; set; }

        public int Points { get; set; }
    }
}
