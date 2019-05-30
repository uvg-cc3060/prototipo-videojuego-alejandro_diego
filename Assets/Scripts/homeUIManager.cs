using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class homeUIManager : MonoBehaviour
{

    // Start the game
    public void StartGame(string level)
    {
        SceneManager.LoadScene(level);
    }
}
