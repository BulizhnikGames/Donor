using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class murderScript : MonoBehaviour
{
    public bool isInformating = false;
    private int collisionInt = 0;
    private bool moveX;
    private bool movePositive;
    private bool C = false; //Collided
    [SerializeField] public playerMovement Player;
    private float distanceY;
    private float distanceX;
    public Sprite[] allSkins;
    private Sprite[] currentSkins;
    private Sprite currentSkin;
    private SpriteRenderer SR;
    private BoxCollider2D BC;
    private Rigidbody2D RB2D;
    private Vector2 movement;
    private float x = 0;
    private float y = 0;
    private List<float> Distances;
    public GameObject Targeting;
    private List<GameObject> Targets;
    [SerializeField] public gameScript gameManager;

    void Start()
    {
        currentSkins = new Sprite[4];
        BC = this.GetComponent<BoxCollider2D>();
        SR = this.GetComponent<SpriteRenderer>();
        RB2D = this.GetComponent<Rigidbody2D>();
        Distances = new List<float>();
        Targets = new List<GameObject>();
        int startNumber = Random.Range(0, allSkins.Length / 4);
        startNumber *= 4;
        for (int i = 0; i < 4; i++)
        {
            currentSkins[i] = allSkins[startNumber + i];
        }
        currentSkin = currentSkins[0];
        StartCoroutine(changeTarget());
    }

    void Update()
    {
        x = 0;
        y = 0;
        if (Targeting != null && C == false)
        {
            distanceX = Mathf.Abs(transform.position.x - Targeting.transform.position.x);
            distanceY = Mathf.Abs(transform.position.y - Targeting.transform.position.y);
            if (distanceX > 0.1f)
            {
                if (Targeting.transform.position.x > transform.position.x)
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
            }
            else if (distanceX < 0.1f && Targeting.GetComponent<civilianMovement>().Move == false)
            {
                transform.position = new Vector2(Targeting.transform.position.x, transform.position.y);
                if (Targeting.transform.position.y > transform.position.y)
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
            }
            if (distanceX < 0.1f && distanceY < 0.1f)
            {
                movement = Vector2.zero;
                Targeting.GetComponent<civilianMovement>().Die();
                gameManager.alreadyTargeted.Remove(Targeting);
                Player.killed(Targeting.transform.position);
                Player.GetComponents<AudioSource>()[5].Play();
                StartCoroutine(waitforendofAnimation(gameManager.mainTexts[1]));
                Targeting = null;
            }
        }
        else if (C == true)
        {
            if (collisionInt < 25)
            {
                if (moveX)
                {
                    if (movePositive)
                    {
                        x += 1;
                    }
                    else
                    {
                        x -= 1;
                    }
                }
                else
                {
                    if (movePositive)
                    {
                        y += 1;
                    }
                    else
                    {
                        y -= 1;
                    }
                }
                collisionInt++;
            }
            else
            {
                C = false;
                collisionInt = 0;
            }
        }
        movement = new Vector2(x, y);
    }
    private void FixedUpdate()
    {
        RB2D.MovePosition((Vector2)transform.position + (movement * Time.deltaTime * gameManager.Player.speedDuplicate * 1.15f));
    }
    private IEnumerator changeTarget()
    {
        yield return new WaitUntil(() => Targeting == null);
        Targets = new List<GameObject>();
        Distances = new List<float>();
        for (int i = 0; i < gameManager.CFM.Count; i++)
        {
            if (gameManager.CFM[i].DTP.x > 9 && gameManager.CFM[i].DTP.y > 6 && gameManager.CFM[i].Alive == true)
            {
                Targets.Add(gameManager.CFM[i].gameObject);
            }
            else
            {
                if (Targets.Contains(gameManager.CFM[i].gameObject))
                {
                    Targets.Remove(gameManager.CFM[i].gameObject);
                }
            }
        }
        for (int c = 0; c < Targets.Count; c++)
        {
            Distances.Add(Vector2.Distance(transform.position, Targets[c].transform.position));
        }
        int targetNumber = 0;
        float topDistance = 999999999.0f;
        for (int d = 0; d < Distances.Count; d++)
        {
            if (Distances[d] < topDistance)
            {
                topDistance = Distances[d];
                targetNumber = d;
            }
        }
        if (Targets.Count > 0)
        {
            if (!gameManager.alreadyTargeted.Contains(Targets[targetNumber]))
            {
                Targeting = Targets[targetNumber];
                gameManager.alreadyTargeted.Add(Targeting);
            }
        }
        if (Targeting != null)
        {
            yield return new WaitForSeconds(4);
        }
        StartCoroutine(changeTarget());
    }
    public void Collided()
    {
        collisionInt = 0;
        C = true;
        if (x != 0)
        {
            moveX = true;
            if (x > 0)
            {
                movePositive = false;
            }
            else
            {
                movePositive = true;
            }
        }
        else
        {
            moveX = false;
            if (y > 0)
            {
                movePositive = false;
            }
            else
            {
                movePositive = true;
            }
        }
        Targeting = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerMovement PM = collision.GetComponent<playerMovement>();
        murderScript otherManiac = collision.GetComponent<murderScript>();
        string Name = collision.gameObject.name;
        if (Name == "fontanCollider" || PM != null || otherManiac != null)
        {
            Collided();
        }
    }
    private IEnumerator waitforendofAnimation(GameObject target)
    {
        if (!gameManager.activeTexts.Contains(true))
        {
            isInformating = true;
            target.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            target.SetActive(false);
            isInformating = false;
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
}
