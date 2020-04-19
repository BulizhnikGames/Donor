using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    void LateUpdate()
    {
        if (Player != null)
        {
            transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
        }
    }
}
