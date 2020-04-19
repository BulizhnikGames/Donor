using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public int Axe = 0;
    [SerializeField] private gameScript gameManager;
    [SerializeField] private GameObject arrowPrefab;
    private GameObject arrowInstantiate;
    private Vector2 Barrier = new Vector2(19.5f, 19.0f);
    public int Lifes = 3;
    public bool Targeted = false;
    public float rebootingTimer = 0.0f;
    public civilianMovement rebootTarget;
    public bool Rebooting = false;
    private Rigidbody2D RB2D;
    private Sprite Current;
    public Sprite[] Axes;
    private SpriteRenderer SR;
    private float Speed = 9.5f;
    public float x = 0;
    public float y = 0;
    private Vector2 movement;
    public float speedDuplicate;

    private void Start()
    {
        speedDuplicate = Speed;
        RB2D = this.GetComponent<Rigidbody2D>();
        SR = this.GetComponent<SpriteRenderer>();
        Current = Axes[0];
    }

    void Update()
    {
        movement = Vector2.zero;
        if (!Check() && gameManager.Canvases[1].activeSelf == false && gameManager.Canvases[2].activeSelf == false && SR != null)
        {
            if (Input.GetKey(KeyCode.D))
            {
                x = 0;
                x += 1;
                if (Current != Axes[2])
                {
                    Axe = 2;
                    Current = Axes[2];
                    SR.sprite = Axes[2];
                }
                movement = new Vector2(x, 0);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                x = 0;
                x -= 1;
                if (Current != Axes[3])
                {
                    Axe = 3;
                    Current = Axes[3];
                    SR.sprite = Axes[3];
                }
                movement = new Vector2(x, 0);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                y = 0;
                y += 1;
                if (Current != Axes[1])
                {
                    Axe = 1;
                    Current = Axes[1];
                    SR.sprite = Axes[1];
                }
                movement = new Vector2(0, y);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                y = 0;
                y -= 1;
                if (Current != Axes[0])
                {
                    Axe = 0;
                    Current = Axes[0];
                    SR.sprite = Axes[0];
                }
                movement = new Vector2(0, y);
            }
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        Move(movement);
    }

    private void Move(Vector2 were)
    {
        float CX = transform.position.x; //Current x
        float CY = transform.position.y; //Current y
        Vector2 PV = (were * Time.deltaTime * Speed); //Plus vetor
        if (((CX + PV.x) < Barrier.x && (CX + PV.x) > -Barrier.x) && ((CY + PV.y) < Barrier.y && (CY + PV.y) > -Barrier.y))
        RB2D.MovePosition((Vector2)transform.position + PV);
    }

    private bool Check()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void killed(Vector2 killPos)
    {
        arrowInstantiate = Instantiate(arrowPrefab);
        arrowInstantiate.GetComponent<arrowScript>().killPos = killPos;
        arrowInstantiate.GetComponent<arrowScript>().Player = this;
    }
    private IEnumerator waitforendofAnimation(GameObject target)
    {
        if (!gameManager.activeTexts.Contains(true))
        {
            target.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            target.SetActive(false);
        }
        else
        {
            StartCoroutine(waitforOrder(target));
        }
    }
    private IEnumerator waitforOrder(GameObject saveTarget)
    {
        yield return new WaitUntil(() => !gameManager.activeTexts.Contains(true));
        StartCoroutine(waitMore(saveTarget));
    }
    private IEnumerator waitMore(GameObject saveTarget)
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 0.1f));
        if (!gameManager.activeTexts.Contains(true))
        {
            StartCoroutine(waitforendofAnimation(saveTarget));
        }
    }
    public void WFEOA()
    {
        //Wait for end of anomation
        StartCoroutine(waitforendofAnimation(gameManager.mainTexts[0]));
    }
}
