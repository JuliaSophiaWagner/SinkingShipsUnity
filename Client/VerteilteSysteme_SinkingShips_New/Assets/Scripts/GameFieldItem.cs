using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class GameFieldItem
    {
        public GameFieldItem(int x, int y, GameObject gameObject)
        {
            this.PosX = x;
            this.PosY = y;
            this.GameObject = gameObject;
        }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public GameObject GameObject
        {
            get;
            set;
        }
    }
}
