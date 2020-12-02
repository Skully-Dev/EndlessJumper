using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] menuUI;
    [SerializeField]
    private GameObject[] hallOfSlimeUI;


    private void Awake()
    {
        DebugLogState();
    }

    /// <summary>
    /// Loads the level
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("Level01"); //Loads the game scene
    }

    public void HighScoreEnter()
    {
        ToggleMenuUI(false);
        ToggleHallOfSlimeUI(true);
    }

    public void HighScoreExit()
    {
        ToggleHallOfSlimeUI(false);
        ToggleMenuUI(true);
    }

    private void ToggleHallOfSlimeUI(bool isVisable)
    {
        foreach (GameObject item in hallOfSlimeUI)
        {
            item.SetActive(isVisable);
        }
    }
    private void ToggleMenuUI(bool isVisable)
    {
        foreach (GameObject item in menuUI)
        {
            item.SetActive(isVisable);
        }
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

    private void DebugLogState()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true; //If using unity, print logs
#else
        Debug.unityLogger.logEnabled = false; //otherwise no need, so dont.
#endif
    }
}
