using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Highscores : MonoBehaviour
{
        const string privateCode = "Wr1X9cKrG0eYvAz0Js754w5hlAMHfAWU-EiMLwT68udw";
        const string publicCode = "5d2e0ba476827f17585ead40";
    const string webUrl = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;
    static Highscores instance;
    DisplayHighscores highscoresDisplay;

    private void Awake()
    {
        instance = this;
        highscoresDisplay = GetComponent<DisplayHighscores>();
    }

    public static void AddNewHighScore(string username, int score)
    {
        instance.StartCoroutine(instance.UploadNewHighScore(username, score));
    }

    IEnumerator UploadNewHighScore(string username, int score)
    {
        string url = webUrl + privateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score;
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        yield return webRequest.SendWebRequest();


        if (string.IsNullOrEmpty(webRequest.error))
        {
            print("Upload successful");
            if(username != string.Empty)
            DownloadHighscores(username);
        }
        else
        {
            print("Error uploading: " + webRequest.error);
        }
    }

    public void DownloadHighscores(string username)
    {
        StartCoroutine(DownloadHighscoresFromDatabase(username));
    }

    IEnumerator DownloadHighscoresFromDatabase(string username)
    {
        Debug.Log("username: " + username);
        string url = webUrl + publicCode + "/pipe-get/" + UnityWebRequest.EscapeURL(username);
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        yield return webRequest.SendWebRequest();


        if (string.IsNullOrEmpty(webRequest.error))
        {
            FormatHighscores(webRequest.downloadHandler.text);
            highscoresDisplay.OnHighscoresDownloaded(highscoresList);
        }
        else
        {
            print("Error downloading: " + webRequest.error);
        }
    }

    void FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);

            highscoresList[i] = new Highscore(username, score);

            print("highscores list: " + highscoresList[i].username + ": " + highscoresList[i].score);
        }
    }
}

public struct Highscore
{
    public string username;
    public int score;

    public Highscore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}