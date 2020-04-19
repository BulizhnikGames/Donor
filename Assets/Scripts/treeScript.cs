using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeScript : MonoBehaviour
{
    private float minX;
    private float maxX;
    [SerializeField] public gameScript gameManager;
    [SerializeField] public GameObject Player;
    private SpriteRenderer SR;
    private BoxCollider2D BC;
    private float distancetoCollider;

    void Start()
    {
        maxX = transform.position.x + 2.5f;
        minX = transform.position.x - 2.5f;
        SR = this.GetComponent<SpriteRenderer>();
        BC = this.GetComponent<BoxCollider2D>();
        distancetoCollider = 4 * BC.offset.y;
    }

    void Update()
    {
        if (Player != null)
        {
            float x = Player.transform.position.x;
            if (((Player.transform.position.y + Player.GetComponent<BoxCollider2D>().offset.y) <= transform.position.y + distancetoCollider) && (x > minX && x < maxX))
            {
                SR.sortingLayerName = "backTree";
            }
            else if (((Player.transform.position.y + Player.GetComponent<BoxCollider2D>().offset.y) > transform.position.y + distancetoCollider) && (x > minX && x < maxX))
            {
                SR.sortingLayerName = "Tree";
            }
        }
    }
}
