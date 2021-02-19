using ServerLogic.GameParts;
using ServerLogic.Login;
using SinkingShipsGameInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLogic
{
    public class Game
    {
        private List<GamePlayer> players;

        private List<PlayerShot> lastShots;

        private BotPlayer botPlayer;

        public Game(ClientData playerOne, ClientData playerTwo)
        {
            this.lastShots = new List<PlayerShot>();
            this.players = new List<GamePlayer>() { new GamePlayer(playerOne), new GamePlayer(playerTwo) };
            this.GameInformation = new GameInformation(new Player(playerOne.Name, playerOne.Token, playerOne.ID), new Player(playerTwo.Name, playerTwo.Token, playerTwo.ID), 0);
        }

        public Game(ClientData playerOne, BotPlayer playerTwo, bool isBot)
        {
            this.lastShots = new List<PlayerShot>();
            this.botPlayer = playerTwo;
            this.players = new List<GamePlayer>() { new GamePlayer(playerOne), new GamePlayer(new ClientData(playerTwo.PlayerID, playerTwo.GetRandomName(), "")) };
            this.GameInformation = new GameInformation(new Player(playerOne.Name, playerOne.Token, playerOne.ID), new Player(this.players.Last().User.Name, "", playerTwo.PlayerID), 0);
            this.IsBot = isBot;
            this.botPlayer.GameID = this.GameInformation.GameID;

            if (this.IsBot)
            {
                ServerLogic.GameParts.ShipManager manager = new ServerLogic.GameParts.ShipManager();
                manager.AutoPlaceShips();
                this.players.Last().Field.Ships = manager.Ships;
                this.InstantitateBotField();
            }
        }

        public bool IsBot { get; private set; }

        public GameInformation GameInformation { get; private set; }

        public bool GameOver
        {
            get;
            set;
        }

        public List<GamePlayer> Players
        {
            get
            {
                return this.players;
            }
        }

        public bool IsFieldBeeingBuild
        {
            get;
            private set;
        }

        public bool IsGameRunning
        {
            get;
            private set;
        }

        public void Start()
        {
            this.IsFieldBeeingBuild = true;
        }


        private void InstantitateBotField()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    this.players.Last().Field.Fields.Add(new GridElement(i, j, this.players.Last().Field.Ships.Any(x => x.PosX == i && x.PosY == j)));
                }
            }

            Parallel.ForEach(this.players.Last().Field.Ships, (ship) =>
            {
                Parallel.ForEach(ship.Fields, (field) =>
                {
                    GridElement fieldTemp = this.players.Last().Field.Fields.Where(x => x.XCoordinate == field.XCoordinate && x.YCoordinate == field.YCoordinate).FirstOrDefault();

                    if (fieldTemp != null)
                    {
                        fieldTemp.IsShip = true;
                    }
                });
            });
        }

        public void AddField(GameField gameField, string userID)
        {
            if (this.players == null)
            {
                throw new NullReferenceException("Player list is null");
            }

            GamePlayer player = this.players.Where(player => player.User.ID == userID).FirstOrDefault();

            if (player == null)
            {
                throw new NullReferenceException("Player id does not exist");
            }

            player.Field.Fields = gameField.Fields;
            player.Field.Ships = gameField.Ships;

            if (this.players.All(player => player.FieldBuildFinished))
            {
                this.IsFieldBeeingBuild = false;
                this.IsGameRunning = true;
                this.players.First().PlayerTurn = true;
            }
        }

        // true if player is on turn
        public bool ExecutePlayerShot(PlayerShot shot, string userID)
        {
            GamePlayer player = this.players.Where(player => player.User.ID == userID).FirstOrDefault();

            if (player == null)
            {
                return false;
            }

            if (!player.PlayerTurn)
            {
                return false;
            }

            return this.Shot(shot, player);
        }

        public void BotMove()
        {
            bool valid = this.ExecutePlayerShot(this.botPlayer.ExecuteShot(), this.botPlayer.PlayerID);

            if (!valid)
            {
                GamePlayer enemyPlayer = this.players.First();
                GridElement field = enemyPlayer.Field.Fields.Where(field => field.XCoordinate == this.botPlayer.ShotsMade.Last().XCoordinate && field.YCoordinate == this.botPlayer.ShotsMade.Last().YCoordinate).FirstOrDefault();

                while (field.HasBeenShot)
                {
                    this.botPlayer.ShotsMade.Remove(this.botPlayer.ShotsMade.Last());
                    valid = this.ExecutePlayerShot(this.botPlayer.ExecuteShot(), this.botPlayer.PlayerID);
                    field = enemyPlayer.Field.Fields.Where(field => field.XCoordinate == this.botPlayer.ShotsMade.Last().XCoordinate && field.YCoordinate == this.botPlayer.ShotsMade.Last().YCoordinate).FirstOrDefault();
                }

                if (!valid)
                {
                    this.botPlayer.ShotsMade.Remove(this.botPlayer.ShotsMade.Last());
                }
            }

            this.botPlayer.ShotsMade.Last().IsShip = this.lastShots.Last().IsShip;
        }

        private bool Shot(PlayerShot shot, GamePlayer player)
        {
            GamePlayer enemyPlayer = this.players.Where(x => x != player).FirstOrDefault();
            GridElement field = enemyPlayer.Field.Fields.Where(field => field.XCoordinate == shot.XCoordinate && field.YCoordinate == shot.YCoordinate).FirstOrDefault();

            if (field.HasBeenShot)
            {
                return false;
            }

            player.PlayerTurn = false;
            enemyPlayer.PlayerTurn = true;
            field.HasBeenShot = true;
            if (field != null && field.IsShip)
            {
                // hit ship
                Ship ship = enemyPlayer.Field.Ships.Where(ship => ship.Fields.Where(field => field.XCoordinate == shot.XCoordinate && field.YCoordinate == shot.YCoordinate).ToList().Count > 0).FirstOrDefault();

                if (ship == null)
                {
                    throw new NullReferenceException("Ship doesn´t exist.");
                }

                shot.IsShip = true;
                ship.CountShot--;
                field.IsShip = true;
                this.GameOver = this.CheckGameOver(player, enemyPlayer);

                if (this.IsBot && player == this.players.Last())
                {
                    botPlayer.ShipSunk = ship.ShipHasSunk;
                }
            }

            shot.ID = player.User.ID;
            this.lastShots.Add(shot);
            player.Shots.Add(shot);
            return true;
        }

        private bool CheckGameOver(GamePlayer player, GamePlayer enemy)
        {
            player.HasWon = enemy.Field.Ships.All(ship => ship.ShipHasSunk);
            return player.HasWon;
        }

        public GameData GetGameData(string id)
        {
            GamePlayer firstPlayer = this.players.Where(x => x.User.ID != id).FirstOrDefault();
            GamePlayer secondPlayer = this.players.Where(x => x.User.ID == id).FirstOrDefault();

            if (firstPlayer == null || secondPlayer == null)
            {
                throw new NullReferenceException("Player is null");
            }
            string message = "Has to build gamefield: ";

            if (firstPlayer.FieldBuildFinished && secondPlayer.FieldBuildFinished)
            {
                string name = this.players.First().User.Name;
                if (this.players.Last().PlayerTurn)
                {
                    name = this.players.Last().User.Name;
                }
                message = name + " is on turn.";

                if (firstPlayer.HasWon)
                {
                    message = firstPlayer.User.Name + " has WON!";
                }
                else if (secondPlayer.HasWon)
                {
                    message = secondPlayer.User.Name + " has WON!";
                }
            }
            else
            {
                if (!firstPlayer.FieldBuildFinished)
                {
                    message += firstPlayer.User.Name + " " ;
                }

                if (!secondPlayer.FieldBuildFinished && !firstPlayer.FieldBuildFinished)
                {
                    message +=  "and ";
                }

                if (!secondPlayer.FieldBuildFinished)
                {
                    message += secondPlayer.User.Name;
                }
            }

            return new GameData(this.lastShots, this.GameOver, message, firstPlayer.ShipsLeft);
        }
    }
}
