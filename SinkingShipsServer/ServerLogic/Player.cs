using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLogic
{
    public class Player
    {
        public Player(string name, string token, string id)
        {
            this.Name = name;
            this.Token = token;
            this.ID = id;
        }

        public Player()
        {

        }

        public string Name { get; set; }

        public string ID { get; set; }

        public string Token { get; set; }
    }
}
