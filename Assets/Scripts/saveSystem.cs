using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class saveSystem 
{
    public static void savehighScore(gameScript gameManager, int Reason, mainmenuScript MMManager)
    {
        try
        {
            //Not only high score
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/save.thick";
            saveScript save = loadhighScore();
            FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            save.Update(gameManager, Reason, MMManager);
            formatter.Serialize(stream, save);
            stream.Close();
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex);
        }
    }
    public static saveScript loadhighScore()
    {
        //Not only high score
        string path = Application.persistentDataPath + "/save.thick";
        if (File.Exists(path))
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                saveScript highScore = formatter.Deserialize(stream) as saveScript;
                stream.Close();
                return highScore;
            }
            catch(System.Exception ex)
            {
                Debug.Log(ex);
                return new saveScript();
            }
        }
        else
        {
            return new saveScript();
        }
    }
}
