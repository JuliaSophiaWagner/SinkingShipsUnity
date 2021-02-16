using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class GridElement
    {
        public GridElement(int xCoordinate, int yCoordinate)
        {
            this.XCoordinate = xCoordinate;
            this.YCoordinate = yCoordinate;
        }

        public GridElement()
        {

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

        public bool HasBeenShot
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
