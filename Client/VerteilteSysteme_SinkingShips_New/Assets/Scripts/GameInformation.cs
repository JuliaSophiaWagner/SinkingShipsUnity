using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class GameInformation
    {
        public GameInformation()
        {

        }

        public string GameID { get; set; }

        public Player FirstPlayer { get; set; }
        public Player SecondPlayer { get; set; }

        public int Points { get; set; }
    }
}
