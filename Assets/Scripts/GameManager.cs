using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; } // Singleton instance of the GameManager so it can be accessed from other scripts
    public Text cowsRemainingText;
    public GameObject gameOverPanel;

    private int totalCows;
    private int cowsAbducted;
    
    private void Awake() //Awake so the GameManager is created before any other script
    {
        if (Instance == null) // Check if there is no other instance of the GameManager
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Keep this instance alive when loading new scenes

            totalCows = GameObject.FindGameObjectsWithTag("Abductable").Length;
            UpdateCowsUI();
            gameOverPanel.SetActive(false); //Hide the game over panel at the start
            cowsAbducted = 0;
            Debug.Log($"GameManager: {totalCows} cows in level.");

        }

        else
        {
            Destroy(gameObject); 
        }
    }

    
    
    
    public void CowAbducted()

    {
        cowsAbducted++;
        UpdateCowsUI(); //Update the UI to show the remaining cows
        Debug.Log($"Cows abducted: {cowsAbducted}/{totalCows}");

        if(cowsAbducted >= totalCows)
        {
            GameOver();
        }
    }

    void UpdateCowsUI()
    {
        cowsRemainingText.text = $"Cows: {totalCows - cowsAbducted}/{totalCows}";
    }

    private void GameOver()
    {
        Debug.Log("All cows are gone - GAME OVER!");

        Time.timeScale = 0f;
        gameOverPanel.SetActive(true); // Show the game over panel

        var fps = FindFirstObjectByType<MyFPS>();
        if (fps != null)
            fps.enabled = false; // Disable the player controller

        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; 

        
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }


}
