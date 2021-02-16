using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class GameData
    {
        public GameData()
        {

        }

        public List<PlayerShot> LastShot
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public bool GameOver
        {
            get;
            set;
        }

        public string GameOverMessage
        {
            get;
            set;
        }

        public int Points
        {
            get;
            set;
        }
    }
}
