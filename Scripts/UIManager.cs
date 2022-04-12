using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Button fireButton;
    [SerializeField]
    TMP_InputField inputField;
    [SerializeField]
    TMP_Text playerUsername;
    [SerializeField]
    Transform enemies;
    List<string> randomNames = new List<string>();
    public List<TMP_Text> scoreTexts = new List<TMP_Text>();
    [SerializeField]
    GameObject leaderBoard;
    [SerializeField]
    Player player;
    // Start is called before the first frame update
    void Awake()
    {
        randomNames = AINamesGenerator.Utils.GetRandomNames(enemies.childCount);
        for (int i = 0; i < enemies.childCount; i++)
        {
            enemies.GetChild(i).GetComponentInChildren<TMP_Text>().text = randomNames[i];
            enemies.GetChild(i).name = randomNames[i];
        }
       
    }
    private void Start()
    {
        
        for (int i = 0; i < scoreTexts.Count; i++)
        {
            scoreTexts[i].text = Leaderboard.Instance.scores[i].full.ToString();
        }
    }

    public void SetPlayersUsername()
    {
        if (inputField.text.Trim() == "")
            playerUsername.text = "Noob";
        else
            playerUsername.text = inputField.text;

        player.gameObject.name = playerUsername.text;
        Leaderboard.Instance.players.Add(player.transform);
        Leaderboard.Instance.scores.Add(new LeaderboardData(player.name, player.experience));

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateScore()
    {
        for (int i = 0; i < scoreTexts.Count; i++)
        {
            scoreTexts[i].text = Leaderboard.Instance.scores[i].Name + " " + Leaderboard.Instance.scores[i].score * 10;
        }
    }

    public void WaitOnFire()
    {
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        fireButton.interactable = false;
        yield return new WaitForSeconds(0.4f);
        fireButton.interactable = true;

    }
}
