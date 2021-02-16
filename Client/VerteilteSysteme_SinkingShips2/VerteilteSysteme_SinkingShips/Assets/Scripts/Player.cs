using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Player
    {
        public Player()
        {

        }

        public Player(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }

        public string ID { get; set; }

        public string Token { get; set; }
    }
}
