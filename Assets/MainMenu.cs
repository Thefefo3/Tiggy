using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
