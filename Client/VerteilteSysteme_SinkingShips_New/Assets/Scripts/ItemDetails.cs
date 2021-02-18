using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemDetails : MonoBehaviour
{
    public GameObject prefab;
    public List<Player> Players { get; set; }

    public List<GameInformation> RunningGames { get; set; }

    public List<Player> GameRequests { get; set; }

    public List<PlayerStats> Ranking { get; set; }
    public List<PlayerStats> RankingWon { get; set; }

    public List<History> History { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        this.Players = new List<Player>();
        this.RunningGames = new List<GameInformation>();
        this.GameRequests = new List<Player>();
        this.Ranking = new List<PlayerStats>();
        this.RankingWon = new List<PlayerStats>();
        this.History = new List<History>();
    }

    public void InstantiatePlayers(List<Player> players)
    {
        this.Players = players;

        if (this.Players == null)
        {
            return;
        }

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < this.Players.Count; i++)
        {
            GameObject item = Instantiate(prefab);
            item.transform.SetParent(transform);
            item.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(this.Players.ElementAt(i).Name);
            item.SetActive(true);
        }
    }

    public void InstantiateRanking(List<PlayerStats> ranking)
    {
        this.Ranking = ranking;

        if (this.Players == null)
        {
            return;
        }

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < this.Ranking.Count; i++)
        {
            GameObject item = Instantiate(prefab);
            item.transform.SetParent(transform);
            item.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(this.Ranking.ElementAt(i).Name);
            item.transform.Find("Points").GetComponent<TextMeshProUGUI>().SetText(this.Ranking.ElementAt(i).Points.ToString());
            item.transform.Find("Place").GetComponent<TextMeshProUGUI>().SetText((i + 1).ToString() + ".");
            item.SetActive(true);
        }
    }

    public void InstantiateRankingWon(List<PlayerStats> ranking)
    {
        this.RankingWon = ranking;

        if (this.Players == null)
        {
            return;
        }

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < this.RankingWon.Count; i++)
        {
            GameObject item = Instantiate(prefab);
            item.transform.SetParent(transform);
            item.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(this.RankingWon.ElementAt(i).Name);
            item.transform.Find("Points").GetComponent<TextMeshProUGUI>().SetText(this.RankingWon.ElementAt(i).Points.ToString());
            item.transform.Find("Place").GetComponent<TextMeshProUGUI>().SetText((i + 1).ToString() + ".");
            item.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateRunningGameList(List<GameInformation> games)
    {
        this.RunningGames = games;

        if (this.RunningGames == null)
        {
            return;
        }

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < this.RunningGames.Count; i++)
        {
            GameObject item = Instantiate(prefab);
            item.transform.SetParent(transform);
            item.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(this.RunningGames.ElementAt(i).FirstPlayer.Name + " vs. " + this.RunningGames.ElementAt(i).SecondPlayer.Name);
            item.transform.Find("Points").GetComponent<TextMeshProUGUI>().SetText(this.RunningGames.ElementAt(i).Points.ToString());
            item.transform.Find("ID").GetComponent<TextMeshProUGUI>().SetText(this.RunningGames.ElementAt(i).GameID);
            item.SetActive(true);
        }
    }

    public void InstantiateGameRequestList(List<Player> players)
    {
        this.GameRequests = players;

        if (this.GameRequests == null)
        {
            return;
        }

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < this.GameRequests.Count; i++)
        {
            GameObject item = Instantiate(prefab);
            item.transform.SetParent(transform);
            item.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(this.GameRequests.ElementAt(i).Name);
            item.SetActive(true);
        }
    }

    public void InitializeHistoryList(List<History> histories)
    {
        this.History = histories;

        if (this.History == null)
        {
            return;
        }

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < this.History.Count; i++)
        {
            GameObject item = Instantiate(prefab);
            item.transform.SetParent(transform);
            item.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText(this.History.ElementAt(i).Name);
            item.transform.Find("Points1").GetComponent<TextMeshProUGUI>().SetText(this.History.ElementAt(i).FirstPlayerPoints.ToString());
            item.transform.Find("Points2").GetComponent<TextMeshProUGUI>().SetText(this.History.ElementAt(i).SecondPlayerPoints.ToString());
            item.SetActive(true);
        }
    }
}