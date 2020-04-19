using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class civilianMovement : MonoBehaviour
{
    private float rebootingTimer = 0.0f;
    private bool canReboot;
    private bool Rebooting = false;
    public bool Alive = true;
    private float dieTimer = 0.0f;
    [SerializeField] public gameScript gameManager;
    [SerializeField] public Sprite Tomb;
    public Vector2 DTP; //Distance to player
    private Vector2 Barrier = new Vector2(19.5f, 19);
    public bool Move;
    private float moveTimer = 0.0f;
    private float stopMoving;
    private bool moveX;
    private bool movePositive;
    private float minX;
    private float maxX;
    private Rigidbody2D RB2D;
    private Vector2 movement;
    [SerializeField] public playerMovement Player;
    private float x = 0;
    private float y = 0;
    public Sprite[] allSkins;
    private Sprite[] currentSkins;
    private Sprite currentSkin;
    private SpriteRenderer SR;
    private BoxCollider2D BC;
    private float distancetoCollider;

    void Start()
    {
        maxX = transform.position.x + 1f;
        minX = transform.position.x - 1f;
        currentSkins = new Sprite[4];
        RB2D = this.GetComponent<Rigidbody2D>();
        SR = this.GetComponent<SpriteRenderer>();
        int startNumber = Random.Range(0, allSkins.Length / 4);
        startNumber *= 4;
        for (int i = 0; i < 4; i++)
        {
            currentSkins[i] = allSkins[startNumber + i];
        }
        currentSkin = currentSkins[0];
        SR.sprite = currentSkins[0];
        BC = this.GetComponent<BoxCollider2D>();
        distancetoCollider = BC.offset.y;
        stopMoving = Random.Range(0.5f, 3.0f);
        if (Random.Range(0, 2) == 1)
        {
            moveX = true;
        }
        else
        {
            moveX = false;
        }
        if (Random.Range(0, 2) == 1)
        {
            Move = true;
        }
        else
        {
            Move = false;
        }
        if (Random.Range(0, 2) == 1)
        {
            movePositive = true;
        }
        else
        {
            movePositive = false;
        }
    }

    void Update()
    {
        if (Alive == false)
        {
            if (BC.IsTouching(Player.GetComponent<BoxCollider2D>()))
            {
                if (Input.GetKey(KeyCode.R))
                {
                    if (rebootingTimer < 2.0f)
                    {
                        rebootingTimer += Time.deltaTime;
                    }
                    else
                    {
                        this.GetComponents<AudioSource>()[0].Play();
                        Rebooted();
                    }
                    Rebooting = true;
                }
                else
                {
                    rebootingTimer = 0.0f;
                    Rebooting = false;
                }
            }
            else
            {
                rebootingTimer = 0.0f;
                Rebooting = false;
            }
        }
        if (Alive == false && Rebooting == false)
        {
            SR.sprite = Tomb;
            if (dieTimer < 12.0f)
            {
                dieTimer += Time.deltaTime;
            }
            else
            {
                gameManager.Civilians.Remove(this.gameObject);
                gameManager.CFM.Remove(this.GetComponent<civilianMovement>());
                Destroy(this.gameObject);
            }
        }
        DTP = new Vector2(Mathf.Abs(transform.position.x - Player.transform.position.x), Mathf.Abs(transform.position.y - Player.transform.position.y));
        if (moveTimer < stopMoving && Alive == true)
        {
            x = 0;
            y = 0;
            moveTimer += Time.deltaTime;
            if (Move == true)
            {
                if (moveX == true)
                {
                    if (movePositive == true)
                    {
                        x += 1;
                        if (currentSkin != currentSkins[2])
                        {
                            currentSkin = currentSkins[2];
                            SR.sprite = currentSkins[2];
                        }
                    }
                    else
                    {
                        x -= 1;
                        if (currentSkin != currentSkins[3])
                        {
                            currentSkin = currentSkins[3];
                            SR.sprite = currentSkins[3];
                        }
                    }
                    if ((transform.position.x + x > Barrier.x) || (transform.position.x + x < -Barrier.x))
                    {
                        moveTimer = 0.0f;
                        stopMoving = Random.Range(0.5f, 3.0f);
                        if (Random.Range(0, 5) < 4)
                        {
                            moveX = !moveX;
                        }
                        if (Random.Range(0, 5) < 4)
                        {
                            movePositive = !movePositive;
                        }
                        x = 0;
                    }
                }
                else
                {
                    if (movePositive == true)
                    {
                        y += 1;
                        if (currentSkin != currentSkins[1])
                        {
                            currentSkin = currentSkins[1];
                            SR.sprite = currentSkins[1];
                        }
                    }
                    else
                    {
                        y -= 1;
                        if (currentSkin != currentSkins[0])
                        {
                            currentSkin = currentSkins[0];
                            SR.sprite = currentSkins[0];
                        }
                    }
                    if ((transform.position.y + y > Barrier.y) || (transform.position.y + y < -Barrier.y))
                    {
                        moveTimer = 0.0f;
                        stopMoving = Random.Range(0.5f, 3.0f);
                        if (Random.Range(0, 5) < 4)
                        {
                            moveX = !moveX;
                        }
                        if (Random.Range(0, 5) < 4)
                        {
                            movePositive = !movePositive;
                        }
                        y = 0;
                    }
                }
            }
        }
        else
        {
            moveTimer = 0.0f;
            stopMoving = Random.Range(0.5f, 3.0f);
            if (Random.Range(0, 5) < 4)
            {
                moveX = !moveX;
            }
            if (Random.Range(0, 5) < 4)
            {
                movePositive = !movePositive;
            }
            if (Random.Range(0, 5) < 4)
            {
                Move = !Move;
            }
        }
        movement = new Vector2(x, y);
    }
    private void FixedUpdate()
    {
        if (Alive == true)
        {
            RB2D.MovePosition((Vector2)transform.position + (movement * Time.deltaTime * Player.speedDuplicate));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Alive == true)
        {
            playerMovement PM = collision.GetComponent<playerMovement>();
            civilianMovement otherCivilian = collision.GetComponent<civilianMovement>();
            string Name = collision.gameObject.name;
            if (Name == "fontanCollider" || PM != null || otherCivilian != null)
            {
                Collided();
            }
        }
    }
    public void Collided()
    {
        moveTimer = 0.0f;
        stopMoving = Random.Range(0.15f, 0.85f);
        movePositive = !movePositive;
    }
    public void Die()
    {
        BC.offset = Vector2.zero;
        BC.size = new Vector2(0.85f, 1.3f);
        Alive = false;
    }
    public void Rebooted()
    {
        rebootingTimer = 0.0f;
        Player.Lifes--;
        Player.WFEOA();
        gameManager.Points += 25;
        Rebooting = false;
        canReboot = false;
        Alive = true;
        SR.sprite = currentSkin;
        dieTimer = 0.0f;
        BC.offset = new Vector2(0, -0.885f);
        BC.size = new Vector2(0.67f, 0.15f);
    }
    public void startRebooting()
    {
        Rebooting = true;
    }
    public void stopRebooting()
    {
        Rebooting = false;
    }
}
