using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{ 

    public void WaitAndPlay(float seconds)
    {
        Invoke("Play", seconds);
    }
    public void Play()
    {
        SceneManager.LoadScene("Basic Maze");
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
