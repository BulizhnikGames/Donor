using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDCM : MonoBehaviour
{
    //3D collider for maniac

    private void OnTriggerEnter(Collider other)
    {
        string Name = "";
        if (other.transform.parent != null)
        {
            if (other.transform.parent.parent != null)
            {
                Name = other.transform.parent.parent.gameObject.name;
                if (Name == "invisibleTrees")
                {
                    transform.parent.GetComponent<murderScript>().Collided();
                }
            }
        }
    }
}
