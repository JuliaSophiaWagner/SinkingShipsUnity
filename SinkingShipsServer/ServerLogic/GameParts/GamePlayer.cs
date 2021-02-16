using SinkingShipsGameInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerLogic.GameParts
{
    public class GamePlayer
    {
        public GamePlayer(ClientData user)
        {
            this.User = user;
            this.Field = new GameField();
            this.Shots = new List<PlayerShot>();
        }

        public ClientData User
        {
            get;
            private set;
        }

        public GameField Field
        {
            get;
            set;
        }

        //public List<IShip> Ships
        //{
        //    get;
        //    set;
        //}

        public List<PlayerShot> Shots
        {
            get;
            set;
        }

        public bool HasWon
        {
            get;
            set;
        }


        //public List<IGridElement> GameField
        //{
        //    get;
        //    set;
        //}

        public int ShipsLeft
        {
            get
            {
                return this.Field.Ships.Where(x => !x.ShipHasSunk).Count();
            }
        }

        public bool PlayerTurn
        {
            get;
            set;
        }

        public bool FieldBuildFinished
        {
            get
            {
                return this.Field.Fields.Count == 100;
            }
        }
    }
}
