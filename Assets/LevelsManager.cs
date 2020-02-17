using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        //SceneManager.LoadScene(1);
    }
}
