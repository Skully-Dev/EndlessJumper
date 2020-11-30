using UnityEngine;
using System.IO;//for files on the operating system, Input Output
using System.Collections.Generic;//for lists
using System.Runtime.Serialization.Formatters.Binary; //serialization to binary

//Static as it will never change. ALSO allows access to its Methods and Variables from any script in same namespace without needing an instance. 
public static class SaveSystem
{
    public static List<HighScore> highScores = new List<HighScore>();

    public static void Save(List<HighScore> _highScores)
    {
        highScores = _highScores;
        BinaryFormatter formatter = new BinaryFormatter();//creating a binary formatter, converts our core C# objects to binary text

        //saved character by character, so like a stream //opens the file to be written to
        using (FileStream file = File.Create(Path())) // i disposables. types that get destroied when left.
        {
            formatter.Serialize(file, highScores); //converting C# object to binary characters, saving the file
            file.Close(); //close off the stream.
        }
    }

    public static void Load()
    {
        if (File.Exists(Path()))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            //Opening the file //The location //How you want to open it?
            using (FileStream file = File.Open(Path(), FileMode.Open)) //.Open to load
            {
                highScores = (List<HighScore>)formatter.Deserialize(file); //using that open file, since we serialized(Binary), we deserialize (which makes an Object) then Cast the deserialized object as a List<HighScore>
                file.Close();
            }
        }
    }

    /// <summary>
    /// True if a save already exists, otherwise false
    /// </summary>
    public static bool SaveExists()
    {
        string path = Path();
        if (File.Exists(path)) //if the file exists
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// A string of the path we want, where to save.
    /// </summary>
    /// <returns>path as string</returns>
    private static string Path()
    {
        //persistantDataPath saves to a specific location on a computer. Location varies between computers.
        //Outside of game files, so is kept when game uninstalled
        //extention DOESNT matter, can be .gd, .sav, or any other thing like .skully
        return Application.persistentDataPath + "/highScores.sav";
    }
}
