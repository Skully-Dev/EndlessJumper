using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Reference pause menu UI")]
    private GameObject[] pauseUI;
    [SerializeField]
    private GameObject[] hallOfSlimeUI;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Death.isDead) //if escape is pressed
        {
            if (isPaused) //if currently paused
            {
                Resume(); //un-pause game
            }
            else
            {
                Pause(); //otherwise pause game.
            }
        }
    }

    public void HighScoreEnter()
    {
        TogglePauseUI(false);
        ToggleHallOfSlimeUI(true);
    }

    public void HighScoreExit()
    {
        ToggleHallOfSlimeUI(false);
        TogglePauseUI(true);
    }

    /// <summary>
    /// unfreeze time, hides pause UI, set isPaused bool to false. 
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1;
        TogglePauseUI(false);
        isPaused = false;
    }

    /// <summary>
    /// freeze time, show pause UI, set isPaused bool, set isPaused bool to true. 
    /// </summary>
    public void Pause()
    {
        Time.timeScale = 0;
        TogglePauseUI(true); //shows menu UI
        isPaused = true; //sets menu active state bool
    }

    private void TogglePauseUI(bool isVisable)
    {
        foreach (GameObject item in pauseUI)
        {
            item.SetActive(isVisable);
        }
    }

    private void ToggleHallOfSlimeUI(bool isVisable)
    {
        foreach (GameObject item in hallOfSlimeUI)
        {
            item.SetActive(isVisable);
        }
    }

    /// <summary>
    /// reloads the scene
    /// </summary>
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Returns Time Scale to normal and loads the Main Menu
    /// </summary>
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Exit application.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting Game");

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode(); //If using unity, run this code, exit play mode
#endif
        Application.Quit(); //if not unity editor, quits app, i.e. once published, this will quit.
    }
}
