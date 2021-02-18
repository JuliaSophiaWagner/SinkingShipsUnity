using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerLogic.GameParts
{
    public class BotPlayer
    {
        private List<Ship> shotShips;
        private bool firstTimeShot;
        private bool triedLeft;
        private bool triedRight;
        private bool triedVertical;

        public BotPlayer(string playerID)
        {
            this.firstTimeShot = false;
            this.shotShips = new List<Ship>();
            this.ShotsMade = new List<PlayerShot>();
            this.PlayerID = playerID;
        }

        public List<PlayerShot> ShotsMade
        {
            get;
            set;
        }

        public string PlayerID
        {
            get;
            private set;
        }

        public string GameID
        {
            get;
            set;
        }

        public bool GameRunning
        {
            get;
            set;
        }

        public bool ShipSunk
        {
            get;
            set;
        }

        public void StartRunningGameAgain()
        {

        }

        public string GetRandomName()
        {
            List<string> names = new List<string> {
                                "Captain Jack Sparrow",
                                "David Jones",
                                "Constantin Barbossa",
                                "Einäugiger Ole",
                                "Einbeiniger Joe",
                                "Elia Schwarzzahn",
                                "Emma Schielauge",
                                "Holzbein-Timo",
                                "Kapitän Flint",
                                "Käpt’n_Blaumarie",
                                "Kilian Haifisch",
                                "Lennard Laszlo der Fürchterliche",
                                "Lilly Brezelzopf",
                                "Merle Säbelrost",
                                "Messerjockel",
                                "Mia Spargelbein",
                                "Mila Stielauge",
                                "Peter der Blasse",
                                "Rico Augenklappe",
                                "Rotbart",
                                "Rotbart-Anna",
                                "Sandro Krümel",
                                "Simon Hefekloß",
                                "Smutje Haifisch",
                                "Stoppelbart",
                                "Stiefelriemen",
                                "3 Finger Jack",
                                "Augenklappen-Aaron",
                                "Black Jack",
                                "Captain Dauerwelle",
                                "Captain Drecksspatz",
                                "Captain Glasauge"};
            return names.ElementAt(new Random().Next(names.Count)) + "(bot)";
        }

        public PlayerShot ExecuteShot()
        {
            Thread.Sleep(3000);
            this.CalculateShot();
            return this.ShotsMade.Last();
         }

        private void ResetConditions()
        {
            this.triedLeft = false;
            this.triedRight = false;
            this.triedVertical = false;
            this.firstTimeShot = false;
        }

        private void CalculateShot()
        {
            if (this.ShotsMade.Count < 1)
            {
                this.ShotsMade.Add(this.GetRandomShot());
                return;
            }

            if (this.ShotsMade.Last().IsShip && !ShipSunk && !this.firstTimeShot)
            {
                Ship shipTemp = new Ship();
                shipTemp.Fields.Add(new GridElement(this.ShotsMade.Last().XCoordinate, this.ShotsMade.Last().YCoordinate, true));
                this.shotShips.Add(shipTemp);
                this.ShotsMade.Add(TryNewShotNearShip());
                this.firstTimeShot = true;
                return;
            }
            else if (!ShipSunk && this.firstTimeShot)
            {
                if (this.ShotsMade.Last().IsShip)
                {
                    this.shotShips.Last().Fields.Add(new GridElement(this.ShotsMade.Last().XCoordinate, this.ShotsMade.Last().YCoordinate, true));
                }

                this.ShotsMade.Add(TryNewShotNearShip());
                return;
            }
            else if (this.ShotsMade.Last().IsShip && ShipSunk)
            {
                this.shotShips.Last().Fields.Add(new GridElement(this.ShotsMade.Last().XCoordinate, this.ShotsMade.Last().YCoordinate, true));
                this.shotShips.Last().Size = this.shotShips.Last().Fields.Count;
                this.shotShips.Last().Fields = new ShipManager().CreateTempFields(this.shotShips.Last());
            }

            ShipSunk = false;
            ResetConditions();
            this.ShotsMade.Add(this.GetRandomShot());
        }

        private PlayerShot GetRandomShot()
        {
            int x = -1;
            int y = -1;
            PlayerShot shotTemp = new PlayerShot(this.PlayerID, this.GameID, x, y, false);
            Random random = new Random();

            while (CheckIfMoveExist(x, y))
            {
                x = random.Next(0, 10);
                y = random.Next(0, 10);
            }

            shotTemp.XCoordinate = x;
            shotTemp.YCoordinate = y;
            return shotTemp;
        }

        private bool CheckIfVerticalShip()
        {
            if (CheckIfFirstShot())
            {
                return false;
            }

            if (this.shotShips.Last().Fields.First().XCoordinate == this.shotShips.Last().Fields.Last().XCoordinate)
            {
                return false;
            }

            this.shotShips.Last().Rotated = true;
            return true;
        }

        private bool CheckIfFirstShot()
        {
            if (this.shotShips.Last().Fields.Count < 2)
            {
                return true;
            }

            if (this.shotShips.Last().Fields.Count == 2 && !this.ShotsMade.Last().IsShip)
            {
                return true;
            }

            return false;
        }

        private PlayerShot TryNewShotNearShip()
        {
            int x, y;
            if (this.ShotsMade.Last().IsShip)
            {
                x = this.ShotsMade.Last().XCoordinate;
                y = this.ShotsMade.Last().YCoordinate;
            }
            else
            {
                x = this.shotShips.Last().Fields.First().XCoordinate;
                y = this.shotShips.Last().Fields.First().YCoordinate;
            }

            PlayerShot shotTemp = new PlayerShot(this.PlayerID, this.GameID, x, y, false);

            if (CheckIfVerticalShip() || CheckIfFirstShot())
            {
                x = x - 1;
                if (!CheckIfMoveExist(x, y) && this.ShotsMade.Last().IsShip)
                {
                    triedLeft = true;
                    shotTemp.XCoordinate = x;
                    return shotTemp;
                }

                if (triedLeft)
                {
                    x = this.shotShips.Last().Fields.First().XCoordinate + 1;
                    triedRight = true;
                }

                if (CheckIfMoveExist(x, y) && triedLeft && triedRight)
                {
                    this.triedVertical = true;
                }
            }

            if (this.triedVertical || !CheckIfVerticalShip())
            {
                if (this.ShotsMade.Last().IsShip)
                {
                    x = this.ShotsMade.Last().XCoordinate;
                    y = this.ShotsMade.Last().YCoordinate - 1;
                }
                else
                {
                    x = this.shotShips.Last().Fields.First().XCoordinate;
                    y = this.shotShips.Last().Fields.First().YCoordinate - 1;
                }

                if (!CheckIfMoveExist(x, y))
                {
                    shotTemp.YCoordinate = y;
                    return shotTemp;
                }

                if (CheckIfMoveExist(x, y) || !this.ShotsMade.Last().IsShip)
                {
                    y = this.shotShips.Last().Fields.First().YCoordinate + 1;
                }

                //if (CheckIfMoveExist(x, y))
                //{
                //    y = this.shotShips.Last().Fields.First().YCoordinate + 1;
                //}
            }

            shotTemp.XCoordinate = x;
            shotTemp.YCoordinate = y;

            if (CheckIfMoveExist(x, y))
            {
                shotTemp = GetRandomShot();
                this.ResetConditions();
            }

            return shotTemp;
        }

        private bool CheckIfMoveExist(int x, int y)
        {
            return (x < 0 || y < 0 || x > 9 || y > 9) || this.ShotsMade.Any(s => s.XCoordinate == x && s.YCoordinate == y) || this.shotShips.Any(s => s.Fields.Any(f => f.XCoordinate == x && f.YCoordinate == y));
        }
    }
}
