using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HighScore
{
    public static int Count;
    public int enteryNumber; //this number is always different meaning one score is always better than the other.
    public string name;
    public float score;

    public HighScore(string _name, float _score)
    {
        enteryNumber = Count++; //incrament the score count
        name = _name;
        score = _score;
    }
}

public class HighScores : MonoBehaviour
{    
    private int numberOfScoresOnBoard = 10;
    public int nameCharLimit = 3;

    List<HighScore> highScores = new List<HighScore>();

    [SerializeField]
    private TextMeshProUGUI[] scores;

    public World world;
    public Button nameButton;
    public string inputName;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveSystem.SaveExists())
        {
            LoadScores();
        }
        else
        {
            GenerateFakeScores();
            SaveScores();
        }

        RefreshHallOfSlime();
    }

    public void EnterName(string _name)
    {
        inputName = _name.ToUpper();

        if (inputName.Length < nameCharLimit)
        {
            nameButton.interactable = false;
        }
        else
        {
            nameButton.interactable = true;
        }
    }

    public void AddNewHighScore()
    {
        //a new score board score

        //get user details
        AddHighScore(inputName, world.score);
        SortScores();
        highScores.RemoveAt(numberOfScoresOnBoard); //removes the score in 11th place.
        SaveScores();
        RefreshHallOfSlime();
        //Display scores
    }

    /// <summary>
    /// Debug - prints all the highscores with their info
    /// </summary>
    private void PrintHighScores()
    {
        for (int i = 0; i < highScores.Count; i++)
        {
            Debug.Log((i + 1) + " name: " + highScores[i].name + " score: " + highScores[i].score + " entry number: " + highScores[i].enteryNumber);
        }
    }

    public void AddHighScore(string name, float score)
    {
        highScores.Add(new HighScore(name, score));
    }

    public bool CheckScore(float score)
    {
        if (score > highScores[numberOfScoresOnBoard - 1].score)
        {
            return true;
        }
        return false;
    }

    public void GenerateFakeScores()
    {
        for (int i = 0; i < numberOfScoresOnBoard; i++)
        {
            string randomName = "";//empty string to be added to
            for (int letter = 0; letter < nameCharLimit; letter++) //names will be 3 letters long like the good old days.
            {
                randomName += (char)UnityEngine.Random.Range(65, 91); //random capital letter value.
            }
            highScores.Add(new HighScore(randomName, 50000 - i * 5000)); //scores from 100K to 50K
        }
    }

    private void SortScores()
    {
        for (int i = 0; i < highScores.Count - 1; i++) //dont need to compair the last entery to anything.
        {
            for (int ii = i + 1; ii < highScores.Count; ii++) //all prior enteries are correct
            {
                if (highScores[ii].score > highScores[i].score)
                {
                    SwapScores(i, ii);
                }
                else if (highScores[ii].score == highScores[i].score) //if same score
                {
                    if (highScores[ii].enteryNumber < highScores[i].enteryNumber) //the first to get that score is better.
                    {
                        SwapScores(i, ii);
                    }
                }
            }
        }
    }

    private void SwapScores(int i, int ii)
    {
        HighScore temp = highScores[i];
        highScores[i] = highScores[ii];
        highScores[ii] = temp;
    }

    public void SaveScores()
    {
        SaveSystem.Save(highScores);
    }

    public void LoadScores()
    {
        SaveSystem.Load();
        highScores = SaveSystem.highScores;

        int Count = 0;
        for (int i = 0; i < highScores.Count; i++)
        {
            if (highScores[i].enteryNumber > Count)
            {
                Count = highScores[i].enteryNumber;
            }
        }
        HighScore.Count = Count;
    }

    /// <summary>
    /// mostly used in testing and debug
    /// </summary>
    public void DeleteScores()
    {
        SaveSystem.Delete();
    }

    public void RefreshHallOfSlime()
    {
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i].text = ((i + 1) + ". " + highScores[i].name + " " + highScores[i].score.ToString("0"));
        }
    }
}
