using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour
{
    private Vector2 playerPos;
    private SpriteRenderer SR;
    [SerializeField] public playerMovement Player;
    public Vector2 killPos;

    private void Start()
    {
        SR = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (SR.color.a > 0)
        {
            float newA = SR.color.a * 255 - Time.deltaTime * 0.02f;
            SR.color = new Color32(255, 255, 255, (byte)newA);
        }
        else
        {
            Destroy(this.gameObject);
        }
        playerPos = Player.transform.position;
        transform.position = Player.transform.position;
        transform.rotation = Quaternion.identity;
        //float z = Vector2.Angle(playerPos, killPos);//Angle frome "playerPos" to "killPos"
        int plus = 270;
        float z = Angle(playerPos, killPos) + plus;
        transform.rotation = Quaternion.Euler(0, 0, z);
    }

    float Angle(Vector2 p1, Vector2 p2)
    {
        return Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * 180 / Mathf.PI;
    }
}
