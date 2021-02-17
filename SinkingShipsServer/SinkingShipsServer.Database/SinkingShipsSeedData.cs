using SinkingShipsServer.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinkingShipsServer.Database
{
    public static class SinkingShipsSeedData
    {
        public static void SeedTestData(this SinkingShipsContext db)
        {   
                var players = new List<PlayerModel> {
                    new PlayerModel
                    {
                        PlayerId = "2",
                        Name = "Enke",
                        Passwort = "1234"
                    },
                    new PlayerModel
                    {
                        PlayerId = "3",
                        Name = "Jules",
                        Passwort = "1234"
                    },
                    new PlayerModel
                    {
                        PlayerId = "4",
                        Name = "Jovin",
                        Passwort = "1234"
                    },
                    new PlayerModel
                    {
                        PlayerId = "5",
                        Name = "Raymahj",
                        Passwort = "1234"
                    }
                };

                db.SinkingShipsPlayers.AddRange(players);
                db.SaveChanges();
            
        }
    }
}
