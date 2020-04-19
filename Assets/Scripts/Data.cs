using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public float volume;
    public int highScore;
    Data()
    {
        volume = 50;
        highScore = 0;
    }
    public Data (int reason, gameScript gameManager, mainmenuScript MMManager) //0 - high score 1 - volume
    {

    }
}
