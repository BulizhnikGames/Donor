using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movelikeAnim : MonoBehaviour
{
    private float x;
    private float y;
    public bool moved = false;
    private float multiply = 1000;
    private RectTransform RT;
    private bool move = false;

    private void Start()
    {
        y = Mathf.Round((float)Screen.height / (Screen.height / 200));
        x = ((float)Screen.width / (Screen.width / 700.0f)) - y * 3.6f;
        RT = this.GetComponent<RectTransform>();
        RT.anchoredPosition = new Vector3(Screen.width / (Screen.width / 700.0f), -(Screen.height / (Screen.height / 20)), 0);
        Debug.Log("go to x: " + x);
        Debug.Log("go to y: " + y);
        Debug.Log("start x: " + RT.anchoredPosition.x);
        Debug.Log("start y: " + RT.anchoredPosition.y);
    }

    void Update()
    {
        if (move == true && RT.anchoredPosition.x > x && RT.anchoredPosition.y < y)
        {
            Debug.Log("go");
            RT.anchoredPosition += (new Vector2(-1 * multiply * 3.6f, 1 * multiply) * Time.deltaTime * 0.5f);
        }
        else if (move == true && RT.anchoredPosition.x <= x || RT.anchoredPosition.y >= y)
        {
            RT.anchoredPosition = new Vector2(-92, 200);
            Debug.Log(move);
            moved = true;
        }
    }

    public void Move()
    {
        move = true;
    }
}
