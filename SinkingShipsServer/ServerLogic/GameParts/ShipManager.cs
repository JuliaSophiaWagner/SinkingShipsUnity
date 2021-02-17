using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerLogic.GameParts
{
    public class ShipManager
    {
        public ShipManager()
        {
            this.Ships = new List<Ship>();
            this.InstantiateShips(new List<(int, int)> { (1, 5), (4, 4), (3, 3), (2, 2) });
        }

        public List<Ship> Ships
        {
            get;
            set;
        }

        /// <summary>
        /// Creates the ships. 
        /// </summary>
        /// <param name="shipSizes">The first value is the count of the ships, and the second the size of it.</param>
        private void InstantiateShips(List<(int,int)> shipSizes)
        {
            foreach (var item in shipSizes)
            {
                for (int i = 0; i < item.Item1; i++)
                {
                    this.Ships.Add(new Ship(item.Item2));
                }
            }

            this.Ships = this.Ships.OrderByDescending(x => x.Size).ToList();
        }

        public void AutoPlaceShips()
        {
            System.Random rand = new System.Random();
            int counter = 0;
            
            for (int i = 0; i < this.Ships.Count; i++)
            {
                int x = rand.Next(0, 10);
                int y = rand.Next(0, 10);
                bool rotatedTemp = rand.Next(2) == 0;
                bool rotated = this.Ships.ElementAt(i).Rotated;

                if (rotated != rotatedTemp)
                {
                    this.Ships.ElementAt(i).Rotated = !rotated;
                }

                this.Ships.ElementAt(i).PosX = x;
                this.Ships.ElementAt(i).PosY = y;

                this.CreateFields(this.Ships.ElementAt(i));               
             
                if (this.CheckPosition(this.Ships.ElementAt(i)))
                {
                    counter = 0;
                    continue;
                }

                this.Ships.ElementAt(i).Fields.Clear();
                this.Ships.ElementAt(i).PosX = 0;
                this.Ships.ElementAt(i).PosY = 0;

                i--;
                counter++;

                if (counter > 1000)
                {
                    this.ClearSetShips();
                    counter = 0;
                    i = -1;
                    continue;
                }
            }
        }

        private void ClearSetShips()
        {
            foreach (var item in this.Ships)
            {
                item.PosX = 0;
                item.PosY = 0;
                item.Fields.Clear();
            }
        }

        private bool CheckPosition(Ship ship)
        {
            foreach (var item in ship.Fields)
            {
                if (this.Ships.Any(z => z != ship && this.CreateTempFields(z).Any(s => s.XCoordinate == item.XCoordinate && s.YCoordinate == item.YCoordinate)))
                {
                    return false;
                }
            }

            if (ship.Fields.Any(z => z.XCoordinate < 0 || z.YCoordinate < 0 || z.XCoordinate > 9 || z.YCoordinate > 9))
            {
                return false;
            }
               
            return true;
        }

        private void CreateFields(Ship ship)
        {
            int offset = (int)Math.Floor((double)(ship.Size / 2));

            if (ship.PosX < 0 || ship.PosY < 0)
            {
                return;
            }

            ship.Fields.Clear();

            if (ship.Rotated)
            {
                int posTemp = ship.PosX - offset;

                if ((ship.Size % 2 - 1) != 0)
                {
                    posTemp++;
                }


                for (int i = 0; i < ship.Size; i++)
                {
                    ship.Fields.Add(new GridElement(posTemp + i, ship.PosY, true));
                }
            }
            else
            {
                int posTemp = ship.PosY - offset;
                for (int i = 0; i < ship.Size; i++)
                {
                    ship.Fields.Add(new GridElement(ship.PosX, posTemp + i, true));
                }
            }
        }

        public List<GridElement> CreateTempFields(Ship ship)
        {
            List<GridElement> fieldTemp = new List<GridElement>();

            if (ship.PosX < 0 || ship.PosY < 0 || ship.Fields.Count < 1)
            {
                return fieldTemp;
            }

            if (ship.Rotated)
            {
                fieldTemp.AddRange(ship.Fields);
                fieldTemp.Add(new GridElement(fieldTemp.Last().XCoordinate + 1, fieldTemp.Last().YCoordinate, false));
                fieldTemp.Add(new GridElement(fieldTemp.First().XCoordinate - 1, fieldTemp.First().YCoordinate, false));

                for (int i = 0; i < ship.Size + 2; i++)
                {
                    fieldTemp.Add(new GridElement(fieldTemp[i].XCoordinate, ship.PosY + 1, false));
                    fieldTemp.Add(new GridElement(fieldTemp[i].XCoordinate, ship.PosY - 1, false));
                }

                return fieldTemp;
            }

            fieldTemp.AddRange(ship.Fields);
            fieldTemp.Add(new GridElement(fieldTemp.Last().XCoordinate, fieldTemp.Last().YCoordinate + 1, false));
            fieldTemp.Add(new GridElement(fieldTemp.First().XCoordinate, fieldTemp.First().YCoordinate - 1, false));

            for (int i = 0; i < ship.Size + 2; i++)
            {
                fieldTemp.Add(new GridElement(ship.PosX + 1, fieldTemp[i].YCoordinate, false));
                fieldTemp.Add(new GridElement(ship.PosX - 1, fieldTemp[i].YCoordinate, false));
            }

            return fieldTemp;
        }
    }
}
