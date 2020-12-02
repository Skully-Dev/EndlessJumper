using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField]
    private HighScores highScores;
    [SerializeField]
    private World world;

    [SerializeField]
    private GameObject pannelBackdropUI;
    [SerializeField]
    private GameObject newHighScoreUI;
    [SerializeField]
    private GameObject deathScreenUI;

    public static bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
    }

    public void PlayerDied()
    {
        isDead = true;
        Time.timeScale = 0;

        if (highScores.CheckScore(world.score))
        {
            ToggleNewHighScoreUI(true);
        }
        else
        {
            pannelBackdropUI.SetActive(true);
            deathScreenUI.SetActive(true);
        }
    }

    private void ToggleNewHighScoreUI(bool isVisable)
    {
        pannelBackdropUI.SetActive(isVisable);
        newHighScoreUI.SetActive(isVisable);
    }
}
