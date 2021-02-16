using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class GameField
    {
        public GameField()
        {
            this.Fields = new List<GridElement>();
            this.Ships = new List<Ship>();
            this.EnemyField = new List<GridElement>();
            this.GameID = string.Empty;
        }

        public List<GridElement> Fields
        {
            get;
            set;
        }

        public string GameID
        {
            get;
            set;
        }

        public List<GridElement> EnemyField
        {
            get;
            set;
        }

        public List<Ship> Ships
        {
            get;
            set;
        }
    }
}
