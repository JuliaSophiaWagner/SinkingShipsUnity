using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogic.GameParts
{
    public class GameData
    {
        public GameData()
        {

        }

        public GameData(List<PlayerShot> shots, bool gameOver, string message, int points)
        {
            this.LastShot = shots;
            this.GameOver = gameOver;
            this.GameOverMessage = string.Empty;
            this.Points = points;
            this.Message = message;
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
