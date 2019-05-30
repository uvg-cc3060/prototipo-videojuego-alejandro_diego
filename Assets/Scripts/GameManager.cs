using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Static instance of the Game Manager,
    // can be access from anywhere
    public static GameManager instance = null;

    public int score = 0;     // Player score
    public int highScore = 0;     // High score

    public int currentLevel = 1; // Level, starting in level 1
    public int highestLevel = 2; // Highest level available in the game

    // Called when the object is initialized
    void Awake()
    {
        // if it doesn't exist
        if (instance == null)
        {
            // Set the instance to the current object (this)
            instance = this;
        }
        else if (instance != this) // There can only be a single instance of the game manager
        { 
            Destroy(gameObject); // Destroy the current object, so there is just one manager
        }

        DontDestroyOnLoad(gameObject); // Don't destroy this object when loading scenes
    }

    // Increase score
    public void IncreaseScore(int amount)
    {
        score += amount;         // Increase the score by the given amount
        print("New Score: " + score.ToString());         // Show the new score in the console

        if (score > highScore)
        {
            highScore = score;
            print("New high score: " + highScore);
        }
    }

    // Restart game. Refresh previous score and send back to level 1
    public void Reset()
    {
        score = 0;         // Reset the score
        currentLevel = 1;        // Set the current level to 1
        // Load corresponding scene (level 1 or other scene)
        SceneManager.LoadScene("level" + currentLevel);
    }

    // Go to the next level
    public void IncreaseLevel()
    {
        if (currentLevel < highestLevel)
        {
            currentLevel++;
        }
        else
        {
            currentLevel = 1;
        }
        SceneManager.LoadScene("level" + currentLevel);
    }

    public void changeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}