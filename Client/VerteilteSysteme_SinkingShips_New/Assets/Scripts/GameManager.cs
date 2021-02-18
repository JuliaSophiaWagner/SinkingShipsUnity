using Assets;
using Assets.Scripts;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Material shipHitTexture;
    public GameObject quitGameObject;
    public GameObject enemyGrid;
    public GameObject game;
    public GameObject login;
    public GameObject register;
    public GameObject registerErrorMessage;
    public GameObject loginErrorMessage;
    public GameObject mulitplayerListHolder;
    public GameObject gameRequestListHolder;
    public GameObject gameListHolder;
    public GameObject rankingListHolder;
    public GameObject gameOverMessage;
    public GameObject historyListHolder;
    public GameObject rankingWonListHolder;
    private static HttpClient client;
    private static string token;
    private GameField gameField;
    private static PlayerCredentials playerCredentials;
    private static bool gameRunning;
    private static GameInformation gameInformation;
    private bool gamefieldFinished;
    private static string message;
    private static string points;
    private static float musicVolume;
    private static float animationVolume;
    private static bool musicChanged;

    // Start is called before the first frame update
    void Start()
    {
        client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:44367/ShipSinking/");
        this.gameField = new GameField();
        this.SetToken();
        client.Timeout = TimeSpan.FromMinutes(0.5);

        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            StartCoroutine(GetOnlinePlayers());

            if (!musicChanged)
            {
                musicVolume = 0.205f;
                animationVolume = 0.198f;
            }
        }
        else
        {
            GameObject.Find("Audio Source").GetComponent<AudioSource>().volume = musicVolume;
            if (!gamefieldFinished)
            {
                Task.Run(async () => await GetStartGameInformation());
                var task = Task.Run(async () => await this.GetRunningGameData(gameInformation.GameID));
                this.SetShips(task.Result);
                enemyGrid.GetComponent<GridManagerScript>().InstantiateAnimation(task.Result.EnemyField, animationVolume);
                GameObject.Find("Grid Player 1").GetComponent<GridManagerScript>().InstantiateAnimation(task.Result.Fields, animationVolume);
                gameRunning = true;
            }

            var enemy = gameInformation.FirstPlayer.Name;

            if (enemy == playerCredentials.Name)
            {
                enemy = gameInformation.SecondPlayer.Name;
            }

            GameObject.Find("Text (TMP) player1").GetComponent<TextMeshProUGUI>().SetText("Player: " + playerCredentials.Name);
            GameObject.Find("Text (TMP) player2").GetComponent<TextMeshProUGUI>().SetText("Player: " + enemy);

            StartCoroutine(GetGameInformation());
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && gameRunning)
        {
            this.LoadScene(-1);
            GameObject.Find("Audio Source").GetComponent<AudioSource>().volume = musicVolume;
            gameRunning = false;
            Debug.Log("Quit Game");
        }
    }

    IEnumerator StartGameOverMessage(string message)
    {
        yield return new WaitForSeconds(1);
        this.gameOverMessage.SetActive(true);
        this.gameOverMessage.GetComponent<TextMeshProUGUI>().SetText(message);
    }

    IEnumerator GetOnlinePlayers()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            if (token != string.Empty && !gameRunning)
            {
                var taskPlayers = Task.Run(async () => await this.GetAllPlayers());
                mulitplayerListHolder.GetComponent<ItemDetails>().InstantiatePlayers(taskPlayers.Result);
                var taskGames = Task.Run(async () => await this.GetAllRunningGames());
                gameListHolder.GetComponent<ItemDetails>().InstantiateRunningGameList(taskGames.Result);
                var taskGameRequests = Task.Run(async () => await this.GetAllGameRequests());
                gameRequestListHolder.GetComponent<ItemDetails>().InstantiateGameRequestList(taskGameRequests.Result);
            }
        }
    }
    IEnumerator CloseGameOverMessage()
    {
        yield return new WaitForSeconds(5);
        this.gameOverMessage.SetActive(false);
    }



    IEnumerator GetGameInformation()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            if (gameRunning && token != string.Empty)
            {
                var task = Task.Run(async () => await this.GetPlayerShot());
                GameData data = task.Result;
                if (points != null && message != null)
                {
                    GameObject.Find("Points").GetComponent<TextMeshProUGUI>().SetText(points);
                    GameObject.Find("Message").GetComponent<TextMeshProUGUI>().SetText(message);
                }

                if (data.LastShot != null)
                {
                    foreach (var item in data.LastShot)
                    {
                        if (item.ID == playerCredentials.ID)
                        {
                            enemyGrid.GetComponent<GridManagerScript>().EnemyHit(item, animationVolume);
                            continue;
                        }

                        GameObject.Find("Grid Player 1").GetComponent<GridManagerScript>().EnemyHit(item, animationVolume);
                    }

                    this.GameOver(data);

                    continue;
                }
            }
        }
    }

    public void ChangeAudio()
    {
        musicVolume = GameObject.Find("Slider Volume").GetComponent<Slider>().value;
        animationVolume = GameObject.Find("Slider Animation").GetComponent<Slider>().value;
        musicChanged = true;
    }

    private async Task<List<Player>> GetAllGameRequests()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("GetAllGameRequests");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Player>>(content);
        }
        catch
        {
            Debug.Log("Get all game requests not sucessfull.");
            return null;
        }
    }

    public async Task<List<Player>> GetAllPlayers()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("GetAllOnlinePlayers");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Player>>(content);
        }
        catch
        {
            Debug.Log("Get all online players not sucessfull.");
            return null;
        }
    }

    public async Task<List<GameInformation>> GetAllRunningGames()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("GetAllRunningGames");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<GameInformation>>(content);
        }
        catch
        {
            Debug.Log("Get running games not sucessfull.");
            return null;
        }
    }

    public async Task<bool> SendClickedField(int xCoordinate, int yCoordinate)
    {
        try
        {
            string jsonText = JsonConvert.SerializeObject(new PlayerShot(token, gameInformation.GameID, xCoordinate, yCoordinate, gameField.Ships.Any(x => x.Fields.Any(y => y.XCoordinate == xCoordinate && y.YCoordinate == yCoordinate))));
            StringContent data = new StringContent(jsonText, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("GameShot", data);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch
        {
            Debug.Log("Shot not sucessfull.");
            return false;
        }
    }


    public async void GameFieldFinished()
    {
        this.gameField.Ships = GameObject.Find("Ships").GetComponent<ShipManager>().GetShips();

        if (this.gameField.Ships == null)
        {
            string message = "You have to finish your gamefield before setting it!";
            StartCoroutine(StartGameOverMessage(message));
            StartCoroutine(CloseGameOverMessage());
            return;
        }

        this.gameField.Fields = GameObject.Find("Grid Player 1").GetComponent<GridManagerScript>().GetGameField();
        this.gameField.GameID = gameInformation.GameID;
        Parallel.ForEach(gameField.Ships, (ship) =>
        {
            Parallel.ForEach(ship.Fields, (field) =>
            {
                GridElement fieldTemp = this.gameField.Fields.Where(x => x.XCoordinate == field.XCoordinate && x.YCoordinate == field.YCoordinate).FirstOrDefault();

                if (fieldTemp != null)
                {
                    fieldTemp.IsShip = true;
                }
            });
        });

        await SendGameField();
        gamefieldFinished = true;
        Debug.Log("finished");
    }

    public async void GetRanking()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("Ranking");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            rankingListHolder.GetComponent<ItemDetails>().InstantiateRanking(JsonConvert.DeserializeObject<List<PlayerStats>>(content));
            response = await client.GetAsync("RankingWon");
            response.EnsureSuccessStatusCode();
            content = await response.Content.ReadAsStringAsync();
            rankingWonListHolder.GetComponent<ItemDetails>().InstantiateRankingWon(JsonConvert.DeserializeObject<List<PlayerStats>>(content));
        }
        catch
        {
            Debug.Log("Get all online players not sucessfull.");
        }
    }

    public void StartBotGame()
    {
        Task.Run(async () => await SendStartBotGame());
        Thread.Sleep(1000);
        this.LoadScene(1);
    }

    private async Task SendStartBotGame()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("StartBotGame");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            gameInformation = JsonConvert.DeserializeObject<GameInformation>(content);
        }
        catch
        {
            Debug.Log("Start bot game not sucessfull.");
        }
    }

    public async Task<string> SendGameField()
    {
        try
        {
            string jsonText = JsonConvert.SerializeObject(this.gameField);
            StringContent game = new StringContent(jsonText, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("SetGameField", game);
            response.EnsureSuccessStatusCode();
            GameObject.Find("Ships").GetComponent<ShipManager>().DeactivateDragAndDrop();
            GameObject.Find("SetGameField").SetActive(false);
            GameObject.Find("AutoPlaceShips").SetActive(false);
            return "Successfull";
        }
        catch
        {
            Debug.Log("not successfull");
            return "Not successfull";
        }
    }


    public async Task<bool> Login()
    {
        try
        {
            playerCredentials = GameObject.Find("Login").GetComponent<UserManager>().playerLoginCredentials;
            string jsonText = JsonConvert.SerializeObject(playerCredentials);
            StringContent data = new StringContent(jsonText, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("Login", data);
            response.EnsureSuccessStatusCode();
            var tokenTemp = await response.Content.ReadAsStringAsync();

            if (tokenTemp != null)
            {
                if (token != string.Empty)
                {
                    client.DefaultRequestHeaders.Remove("Token");
                }
                token = tokenTemp;
                this.SetToken();
                Debug.Log("Successfull");
            }
            await this.GetPlayerID();
            return true;
        }
        catch
        {
            Debug.Log("Login not successfull");
            return false;
        }
    }

    public async void Logout()
    {
        try
        {
            StringContent data = new StringContent(token, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync("Logout", data);
            response.EnsureSuccessStatusCode();
            Debug.Log("Logout successful");
            token = string.Empty;
        }
        catch
        {
            Debug.Log("Logout not successful");
        }
    }

    private void SetToken()
    {
        client.DefaultRequestHeaders.Add("Token", token);
    }

    public async void SendLoginData()
    {
        bool result = await this.Login();
        if (result)
        {
            this.login.SetActive(false);
            this.game.SetActive(true);
            return;
        }

        this.loginErrorMessage.SetActive(true);
    }

    public async Task<GameData> GetPlayerShot()
    {
        try
        {
            string jsonText = JsonConvert.SerializeObject(gameInformation);
            StringContent dataToSend = new StringContent(jsonText, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("GetLastShot", dataToSend);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            GameData data = JsonConvert.DeserializeObject<GameData>(content);
            message = data.Message;
            points = "Ships left: " + data.Points.ToString();
            return data;
        }
        catch
        {
            Debug.Log("Server Access not successfull");
            return null;
        }
    }

    private void GameOver(GameData data)
    {
        if (data.GameOver)
        {
            StartCoroutine(StartGameOverMessage(data.Message + "\n Press q to return to the menu"));
        }
    }

    public async void SendRegisterData()
    {
        bool result = await this.Register();
        if (result)
        {
            this.register.SetActive(false);
            this.login.SetActive(true);
            return;
        }

        this.registerErrorMessage.SetActive(true);
    }

    private void LoadScene(int sceneCount)
    {
        DontDestroyOnLoad(GameObject.Find("GameManager"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + sceneCount);
    }

    private async Task GetPlayerID()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("GetPlayerID");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            playerCredentials.ID = content;
        }
        catch
        {
            Debug.Log("Get Player ID not successfull");
        }
    }

    private async Task<bool> Register()
    {
        try
        {
            playerCredentials = GameObject.Find("Register").GetComponent<UserManager>().playerRegisterCredentials;
            string jsonText = JsonConvert.SerializeObject(playerCredentials);
            StringContent data = new StringContent(jsonText, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("Register", data);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            playerCredentials.ID = content;
            return true;
        }
        catch
        {
            Debug.Log("Register not successfull");
            return false;
        }
    }

    public async void GetHistoryData()
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync("GetHistory");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            historyListHolder.GetComponent<ItemDetails>().InitializeHistoryList(JsonConvert.DeserializeObject<List<History>>(content));
        }
        catch
        {
            Debug.Log("Get history not successfull");
        }
    }

    private async Task SendGameRequestToSpecificUser(Player player)
    {
        try
        {
            string jsonText = JsonConvert.SerializeObject(player);
            StringContent data = new StringContent(jsonText, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("GameRequestSpecificUser", data);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
        }
        catch
        {
        }
    }

    private async Task GetStartGameInformation()
    {
        try
        {
            string jsonText = JsonConvert.SerializeObject(gameInformation.GameID);
            StringContent data = new StringContent(jsonText, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("GetGameInformation", data);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            gameInformation = JsonConvert.DeserializeObject<GameInformation>(content);
        }
        catch
        {
        }
    }

    public void StartGame(GameObject name)
    {
        string text = name.GetComponent<TextMeshProUGUI>().text;
        Player player = name.transform.parent.parent.GetComponent<ItemDetails>().GameRequests.Where(x => x.Name == text).FirstOrDefault();
        Task.Run(async () => await this.SendStartGame(player));
    }

    private async Task SendStartGame(Player player)
    {
        try
        {
            string jsonText = JsonConvert.SerializeObject(player);
            StringContent data = new StringContent(jsonText, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("StartGame", data);
            string content = await response.Content.ReadAsStringAsync();
            gameInformation = JsonConvert.DeserializeObject<GameInformation>(content);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }

    private async Task<GameField> GetRunningGameData(string gameID)
    {
        try
        {
            string jsonText = JsonConvert.SerializeObject(gameID);
            StringContent data = new StringContent(jsonText, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("GetRunningGameData", data);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GameField>(content);
        }
        catch
        {
            Debug.Log("Get game data not successfull");
            return null;
        }
    }

    private async Task GetCurrentGameStatus(string gameID)
    {
        try
        {
            StringContent data = new StringContent(gameID, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("GetCurrentGameStatus", data);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
        }
    }

    public void SendGetCurrentGameData(GameObject id)
    {
        gameInformation = new GameInformation();
        gameInformation.GameID = id.GetComponent<TextMeshProUGUI>().text;
        this.LoadScene(1);
    }

    private void SetShips(GameField data)
    {
        if (GameObject.Find("Ships").GetComponent<ShipManager>().InstanitateFieldWithData(data.Ships))
        {
            this.gamefieldFinished = true;
            GameObject.Find("SetGameField").SetActive(false);
            GameObject.Find("AutoPlaceShips").SetActive(false);
        }
    }

    public void SendGameRequest(GameObject playerObject)
    {
        Player player = playerObject.transform.parent.parent.GetComponent<ItemDetails>().Players.Where(x => x.Name == playerObject.GetComponent<TextMeshProUGUI>().text).FirstOrDefault();
        Task.Run(async () => await this.SendGameRequestToSpecificUser(player));
    }
}
