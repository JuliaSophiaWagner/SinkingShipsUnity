using Microsoft.AspNetCore.Mvc;
using ServerLogic;
using ServerLogic.GameParts;
using ServerLogic.Login;
using SinkingShipsServer.Database.Models;
using SinkingShipsServer.Database.Repositories;
using SinkingShipsServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SinkingShipsServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShipSinkingController : Controller
    {
        private ISinkingShipsService service;
        private ISinkingShipsRepository rep;

        public ShipSinkingController(ISinkingShipsRepository rep, ISinkingShipsService service)
        {
            this.rep = rep;
            this.service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlayerModel>> GetString()
        {
            return Ok(this.service.GetAllPlayers());
        }

        //[HttpPost]
        //[Route("RequestGame")]
        //public ActionResult<bool> RequestGame()
        //{
        //    string token = GetToken();

        //    var gameStart = this.service.RequestGame(token);

        //    return gameStart;
        //}

        [HttpGet]
        [Route("StartBotGame")]
        public ActionResult<GameInformation> StartBotGame()
        {
            string token = GetToken();
            return this.service.StartBotGame(token);
        }

        [HttpPost]
        [Route("GameRequestSpecificUser")]
        public ActionResult<bool> GameRequestSpecificUser([FromBody] Player id)
        {
            string token = GetToken();
            this.service.SaveGameRequest(token, id);
            return Ok(200);
        }

        [HttpPost]
        [Route("StartGame")]
        public ActionResult<GameInformation> StartGame([FromBody] Player player)
        {
            string token = GetToken(); //token == der der angenommen hat

            GameInformation gameInfo = this.service.StartGame(player, token);
            return gameInfo;
        }

        [HttpPost]
        [Route("SetGameField")]
        public ActionResult SetGameField([FromBody]GameField gameField)
        {
            string token = GetToken();

            this.service.SetGameField(gameField, token);
            return Ok(200);
        }

        [HttpPost]
        [Route("PauseGame")]
        public ActionResult PauseGame([FromBody] PlayerCredentials credentials)
        {
            string token = GetToken(); //TODO Token

            this.service.PauseGame(credentials, token);
            return Ok(200);
        }


        [HttpPost]
        [Route("GetGameInformation")]
        public ActionResult<GameInformation> SendGameInformation([FromBody] string gameID)
        {
            string token = GetToken(); //TODO Token

            return this.service.GetGameInfo(gameID, token);
        }

        [HttpPost]
        [Route("GetLastShot")]
        public ActionResult<GameData> SendLastShot([FromBody] GameInformation info)
        {
            string token = GetToken();
            return this.service.GetGameData(token, info);
        }

        [HttpGet]
        [Route("GetPlayerID")]
        public ActionResult<string> SendPlayerID()
        {
            string token = GetToken();
            return this.service.GetAllPlayers().Where(x => x.Token == token).FirstOrDefault().ID;
        }

        [HttpGet]
        [Route("GetAllRunningGames")]
        public ActionResult<List<GameInformation>> SendAllRunningGames()
        {
            string token = GetToken();
            return this.service.GetAllRunningGames(token);
        }

        [HttpGet]
        [Route("GetHistory")]
        public ActionResult<List<History>> SendHistory()
        {
            string token = GetToken();
            return this.service.GetHistory(token);
        }

        [HttpGet]
        [Route("GetAllGameRequests")]
        public ActionResult<List<Player>> SendAllGameRequestsFromClient()
        {
            string token = GetToken();
            return this.service.GetAllGameRequests(token);
        }

        [HttpGet]
        [Route("GetAllOnlinePlayers")]
        public ActionResult<List<Player>> SendAllLoginPlayers()
        {
            List<Player> players = new List<ServerLogic.Player>();
            string token = GetToken();
            foreach (var item in this.service.GetAllLoggedInPlayers())
            {
                if (item.Token == token)
                {
                    continue;
                }

                players.Add(new Player(item.Name, item.Token, item.ID));
            }

            return players;
        }

        [HttpPost]
        [Route("GetRunningGameData")]
        public ActionResult<GameField> SendRunningGameData([FromBody]string gameID)
        {
            string token = GetToken();
            return this.service.GetRunningGameData(token, gameID);
        }

        private string GetToken()
        {
            string token = Request.Headers["Token"];

            if (token == null || token == string.Empty)
            {
                return string.Empty;
            }

            return token.Trim(',').Trim(' ');
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult<string> LoginPlayer([FromBody] PlayerCredentials credentials)
        {
            string token = Guid.NewGuid().ToString();

            if (this.service.LoginPlayer(credentials, token) /*&& this.rep.CheckIfPlayerExist(credentials.Name, credentials.Password)*/)
            {
                return token;
            }


            return BadRequest(400);
        }

        [HttpPost]
        [Route("GameShot")]
        public ActionResult SetGameShot([FromBody] PlayerShot shot)
        {
            string token = GetToken();

            //if (this.service.GetPlayerbyId(token))
            //{
                if (this.service.SetGameShot(shot, token))
                {
                    return Ok(200); //When shot was executed correctly 
                }
            //}

            return BadRequest(400);
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult<string> RegisterPlayer([FromBody] PlayerCredentials credentials)
        {
            ClientData player = this.service.RegisterPlayer(credentials);
            if (player != null)
            {
                //this.rep.AddPlayer(new PlayerModel()
                //{
                //    PlayerId = player.ID.ToString(),
                //    Name = player.Name,
                //    Passwort = player.Password
                //});

                return player.ID;
            }

            return string.Empty;
        }

        [HttpGet]
        [Route("Ranking")]
        public ActionResult<List<PlayerStats>> GetRanking()
        {
            string token = GetToken();

            return this.service.GetRanking(token);
        }

        [HttpGet]
        [Route("RankingWon")]
        public ActionResult<List<PlayerStats>> GetRankingWon()
        {
            string token = GetToken();

            return this.service.GetRankingWon(token);
        }
    }
}
