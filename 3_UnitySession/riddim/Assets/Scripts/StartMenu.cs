using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameSalaryman");
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene("CycleBeatTutorial");
    }
}
