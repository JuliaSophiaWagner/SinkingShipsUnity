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
using GameParts = ServerLogic.GameParts;

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
        [Route("StartBotGame")]
        public ActionResult<GameInformation> StartBotGame()
        {
            string token = GetToken();
            var game = this.service.StartBotGame(token);

            if (token == null || token == string.Empty || game == null)
            {
                return BadRequest(400);
            }

            return game;
        }

        [HttpPost]
        [Route("GameRequestSpecificUser")]
        public ActionResult<bool> GameRequestSpecificUser([FromBody] ServerLogic.Player id)
        {
            string token = GetToken();
            
            if (token == null || token == string.Empty || this.service.SaveGameRequest(token, id))
            {
                return BadRequest(400);
            }

            this.UpdateData();
            return Ok(200);
        }

        [HttpPost]
        [Route("StartGame")]
        public ActionResult<GameInformation> StartGame([FromBody] ServerLogic.Player player)
        {
            string token = GetToken(); 
            GameInformation gameInfo = this.service.StartGame(player, token);

            if (token == null || token == string.Empty || gameInfo == null)
            {
                return BadRequest(400);
            }

            return Ok(gameInfo);
        }

        [HttpPost]
        [Route("SetGameField")]
        public ActionResult SetGameField([FromBody]GameField gameField)
        {
            string token = GetToken();

            if (token == null || token == string.Empty || this.service.SetGameField(gameField, token))
            {
                return BadRequest(400);
            }

            return Ok(200);
        }

        [HttpPost]
        [Route("GetGameInformation")]
        public ActionResult<GameInformation> SendGameInformation([FromBody] string gameID)
        {
            string token = GetToken();
            var gameInfo = this.service.GetGameInfo(gameID, token);
            
            if (token == null || token == string.Empty || gameInfo == null)
            {
                return BadRequest(400);
            }

            return Ok(gameInfo);
        }

        [HttpPost]
        [Route("GetLastShot")]
        public ActionResult<GameData> SendLastShot([FromBody] GameInformation info)
        {
            string token = GetToken();

            if (token == null || token == string.Empty)
            {
                return BadRequest(400);
            }

            return this.service.GetGameData(token, info);
        }

        [HttpGet]
        [Route("GetPlayerID")]
        public ActionResult<string> SendPlayerID()
        {
            string token = GetToken();
            var players = this.service.GetAllPlayers().Where(x => x.Token == token).FirstOrDefault();

            if (token == null || token == string.Empty || players == null)
            {
                return BadRequest(400);
            }

            return players.ID;
        }

        [HttpGet]
        [Route("GetAllRunningGames")]
        public ActionResult<List<GameInformation>> SendAllRunningGames()
        {
            string token = GetToken();
            var games = this.service.GetAllRunningGames(token);

            if (token == null || token == string.Empty || games == null)
            {
                return BadRequest(400);
            }

            return games;
        }

        [HttpGet]
        [Route("GetHistory")]
        public ActionResult<List<GameParts.History>> SendHistory()
        {
            string token = GetToken();

            if (token == null || token == string.Empty)
            {
                return BadRequest(400);
            }

            return this.service.GetHistory(token);
        }

        [HttpGet]
        [Route("GetAllGameRequests")]
        public ActionResult<List<ServerLogic.Player>> SendAllGameRequestsFromClient()
        {
            string token = GetToken();

            if (token == null || token == string.Empty)
            {
                return BadRequest(400);
            }

            return this.service.GetAllGameRequests(token);
        }

        [HttpGet]
        [Route("GetAllOnlinePlayers")]
        public ActionResult<List<ServerLogic.Player>> SendAllLoginPlayers()
        {
            List<ServerLogic.Player> players = new List<ServerLogic.Player>();
            string token = GetToken();

            if (token == null || token == string.Empty)
            {
                return BadRequest(400);
            }

            foreach (var item in this.service.GetAllLoggedInPlayers())
            {
                if (item.Token == token)
                {
                    continue;
                }

                players.Add(new ServerLogic.Player(item.Name, item.Token, item.ID));
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
            this.UpdateData();
            string token = Guid.NewGuid().ToString();

            if (this.service.LoginPlayer(credentials, token) && this.rep.CheckIfPlayerExist(credentials.Name, credentials.Password))
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

            if (token == null || token == string.Empty)
            {
                return BadRequest(400);
            }

            if (this.service.SetGameShot(shot, token))
            {
                return Ok(200); //When shot was executed correctly 
            }


            return BadRequest(400);
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult<string> RegisterPlayer([FromBody] PlayerCredentials credentials)
        {
            this.UpdateData();
            ServerLogic.ClientData player = this.service.RegisterPlayer(credentials);
            if (player != null)
            {
                this.rep.RegisterPlayer(player);
                return player.ID;
            }

            return string.Empty;
        }

        [HttpGet]
        [Route("Ranking")]
        public ActionResult<List<PlayerStats>> GetRanking()
        {
            string token = GetToken();

            if (token == null || token == string.Empty)
            {
                return BadRequest(400);
            }
            
            return this.service.GetRanking(token);
        }

        [HttpGet]
        [Route("RankingWon")]
        public ActionResult<List<PlayerStats>> GetRankingWon()
        {
            string token = GetToken();

            if (token == null || token == string.Empty)
            {
                return BadRequest(400);
            }

            return Ok(this.service.GetRankingWon(token));
        }

        private void UpdateData()
        {
           this.service.Updatedata(this.rep.GetAllRegisteredPlayers());
        }
    }
}
