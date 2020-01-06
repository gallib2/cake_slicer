using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    const int gamoverScreenIndex = 2;
    const int startScreenIndex = 0;

    public AudioClip buttonClicked;
    public AudioSource audioSource;

    public GameObject panel;
    public InputField textInput;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }


    public void StartGame()
    {
        audioSource.PlayOneShot(buttonClicked);
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (panel && currentScene == startScreenIndex)
        {
            panel.SetActive(true);
        }
        else if (currentScene != startScreenIndex)
        {
            int nextScene = currentScene + 1;

            if (SceneManager.GetActiveScene().buildIndex == gamoverScreenIndex)
            {
                nextScene = 1;
            }
            SceneManager.LoadScene(nextScene);
        }
    }

    public void Play()
    {
        GameManager.playerName = textInput.text == string.Empty ? "caker" : textInput.text;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
