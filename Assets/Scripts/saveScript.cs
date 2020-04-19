using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class saveScript 
{
    public float volume;
    public int highScore;

    public saveScript()
    {
        //Characteristics without prewsave
        volume = 50;
        highScore = 0;
    }

    public void Update (gameScript gameManager, int Reason, mainmenuScript MMManager)
    {
        if (Reason == 1)
        {
            highScore = gameManager.highScore;
        }
        else if (Reason == 2)
        {
            if (gameManager != null)
            {
                volume = gameManager.Volume;
            }
            else
            {
                volume = MMManager.Volume;
            }
        }
        if (Reason == 3)
        {
            volume = gameManager.Volume;
            highScore = gameManager.highScore;
        }
    }
}
