using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerLogic.GameParts
{
    public class BotPlayer
    {
        public BotPlayer(string playerID)
        {
            this.ShotsMade = new List<PlayerShot>();
            this.PlayerID = playerID;
            //this.worker = new Thread(new ThreadStart(MakeShot));
            //this.worker.Start();
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
            this.CalculateShot();
            Thread.Sleep(3000);
            return this.ShotsMade.Last();
        }

        private void CalculateShot()
        {
            if (this.ShotsMade.Count < 1)
            {
                this.ShotsMade.Add(this.GetRandomShot());
                return;
            }

            if (this.ShotsMade.Last().IsShip && !ShipSunk)
            {
                this.ShotsMade.Add(TryNewShotNearShip());
                return;
            }

            ShipSunk = false;
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

        private PlayerShot TryNewShotNearShip()
        {
            int x = this.ShotsMade.Last().XCoordinate - 1;
            int y = this.ShotsMade.Last().YCoordinate;
            PlayerShot shotTemp = new PlayerShot(this.PlayerID, this.GameID, x, y, false);

            if (CheckIfMoveExist(x, y))
            {
                x = this.ShotsMade.Last().XCoordinate + 1;
            }

            if (CheckIfMoveExist(x, y))
            {
                x = this.ShotsMade.Last().XCoordinate;
                y = this.ShotsMade.Last().YCoordinate - 1;
            }

            if (CheckIfMoveExist(x, y))
            {
                y = this.ShotsMade.Last().YCoordinate + 1;
            }

            if (CheckIfMoveExist(x, y))
            {
                return GetRandomShot();
            }

            shotTemp.XCoordinate = x;
            shotTemp.YCoordinate = y;
            return shotTemp;
        }

        private bool CheckIfMoveExist(int x, int y)
        {
            return (x < 0 || y < 0 || x > 9 || y > 9) || this.ShotsMade.Any(s => s.XCoordinate == x && s.YCoordinate == y);
        }
    }
}
