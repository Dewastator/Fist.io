using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance;
    [SerializeField]
    public List<Transform> players = new List<Transform>();
    public List<LeaderboardData> scores = new List<LeaderboardData>();
    public GameEvent OnScoreChanged;
    float timer = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].gameObject.layer == 10)
            {
                var lData = new LeaderboardData(players[i].name, players[i].GetComponent<AIController>().experience);
                scores.Add(lData);
            }
            else
            {
                var lData = new LeaderboardData(players[i].name, players[i].GetComponent<Player>().experience );
                scores.Add(lData);
            }
        }
    }
    private void Update()
    {
        //UpdateScore();

        if (Time.time > timer)
        {
            SortOutList();
            timer = Time.time + 2f;
        }
    }


    public void UpdateScore()
    {
        //for (int i = 0; i < players.Count; i++)
        //{
        //    if (players[i].gameObject.layer == 10)
        //    {

        //        if(scores[i].score != players[i].GetComponent<AIController>().experience)
        //        {
        //            scores[i].ChangeScore(players[i].GetComponent<AIController>().experience);
        //        }
        //    }
        //    else
        //    {
        //        if (scores[i].score != players[i].GetComponent<Player>().experience)
        //        {
        //            scores[i].ChangeScore(players[i].GetComponent<Player>().experience);
        //        }
        //    }
        //}
    }


    private void SortOutList()
    {
        scores.Sort(SortFunc);
        OnScoreChanged.Raise();
    }

    private int SortFunc(LeaderboardData a, LeaderboardData b)
    {
        if(a.score < b.score)
        {
            return 1;
        }
        else if(a.score > b.score)
        {
            return -1;
        }
        return 0;
    }

    public void UpdateExp(Transform t)
    {
        scores.Where(s => s.Name == t.name).Select(s => { s.score = t.GetComponent<Expirience>().exp; return s; }).ToList();
    }
}

[Serializable]
public class LeaderboardData
{
    public int score;
    public string Name;
    public string full;

    public LeaderboardData(string n, int s)
    {
        Name = n;
        score = s;
        full = Name + " " + score;
    }

    public void ChangeScore(int s)
    {
        score = s;
        full = Name + " " + score * 10;
    }
}