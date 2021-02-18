using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class PlayerShot
    {
        public PlayerShot(string id, string gameID, int xCoordinate, int yCoordinate, bool isShip)
        {
            this.ID = id;
            this.GameID = gameID;
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
            this.IsShip = isShip;
        }

        public PlayerShot()
        {

        }

        public PlayerShot(int xCoordinate, int yCoordinate, bool isShip)
        {
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
            this.IsShip = isShip;
        }

        public string ID
        {
            get;
            set;
        }
        public string GameID
        {
            get;
            set;
        }

        public int XCoordinate
        {
            get;
            set;
        }

        public int YCoordinate
        {
            get;
            set;
        }

        public bool IsShip
        {
            get;
            set;
        }
    }
}
