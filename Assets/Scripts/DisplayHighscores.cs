using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHighscores : MonoBehaviour
{
    public Text[] highscoreText;
    Highscores HighscoreManager;

    void Start()
    {
        for (int i = 0; i < highscoreText.Length; i++)
        {
            highscoreText[i].text = "2048";
            //highscoreText[i].text = i + 1 + ". Fetching...";
        }

        HighscoreManager = GetComponent<Highscores>();

        if(GameManager.playerName != string.Empty)
            StartCoroutine(RefreshHighscores());
    }

    public void OnHighscoresDownloaded(Highscore[] highscoreList)
    {
        for (int i = 0; i < highscoreText.Length; i++)
        {
            //highscoreText[i].text = i + 1 + ". ";

            if (highscoreList.Length > i)
            {
                //highscoreText[i].text += highscoreList[i].username + " - " + highscoreList[i].score;
                highscoreText[i].text = highscoreList[i].score.ToString();
            }
        }
    }

    IEnumerator RefreshHighscores()
    {
        while (true)
        {
            HighscoreManager.DownloadHighscores(GameManager.playerName);

            yield return new WaitForSeconds(60);
        }
    }
}