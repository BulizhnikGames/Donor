using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class gameScript : MonoBehaviour
{
    private bool playerDied = false;
    private float Timer = 0.0f;
    private bool firstShow = false;
    private bool secondShow = false;
    private bool thirdShow = false;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI pointsText;
    public List<bool> activeTexts;
    public int highScore = 0;
    public int Points = 0;
    public float Volume;
    public GameObject[] pausemenuElements;
    public GameObject[] Canvases;
    public GameObject[] mainTexts;
    [SerializeField] private Transform lifesonCanvas;
    [SerializeField] private Sprite Tomb;
    private int spawnManiacs = 5;
    private int spawnCivilian = 15;
    public List<GameObject> alreadyTargeted;
    public List<civilianMovement> CFM; //cinivials for maniac
    [SerializeField] private Transform allTrees;
    [SerializeField] private Transform spawnposMain;
    [SerializeField] public playerMovement Player;
    private List<int> used;
    public List<GameObject> Maniacs;
    public List<GameObject> Civilians;
    private Vector2[] spawnPos;
    [SerializeField] private GameObject maniacPrefab;
    private GameObject maniacInstantiate;
    [SerializeField] private GameObject civilianPrefab;
    private GameObject civilianInstantiate;

    void Awake()
    {
        
        Time.timeScale = 1;
        saveScript Data = saveSystem.loadhighScore();
        pausemenuElements[2].GetComponent<Slider>().value = Data.volume;
        Volume = Data.volume;
        AudioListener.volume = Volume / 20;
        highScore = Data.highScore;
        activeTexts = new List<bool>();
        alreadyTargeted = new List<GameObject>();
        CFM = new List<civilianMovement>();
        used = new List<int>();
        for (int r = 0; r < 4; r++)
        {
            for (int e =0; e < 3; e++)
            {
                allTrees.GetChild(r).GetChild(e).GetComponent<treeScript>().Player = Player.gameObject;
                allTrees.GetChild(r).GetChild(e).GetComponent<treeScript>().gameManager = this;
            }
        }
        spawnPos = new Vector2[20];
        for (int t = 0; t < 20; t++)
        {
           spawnPos[t] = spawnposMain.GetChild(t).transform.position;
        }
        Civilians = new List<GameObject>();
        Maniacs = new List<GameObject>();
        for (int i = 0; i < spawnCivilian; i++)
        {
            int ran = 0;
            civilianInstantiate = Instantiate(civilianPrefab);
            civilianInstantiate.transform.position = Vector2.zero;
            Vector2 pos = Vector2.zero;
            while (used.Contains(ran) || pos == Vector2.zero)
            {
                ran = Random.Range(0, 20);
                pos = spawnPos[ran];
            }
            civilianInstantiate.GetComponent<civilianMovement>().Player = Player;
            civilianInstantiate.GetComponent<civilianMovement>().gameManager = this;
            civilianInstantiate.GetComponent<civilianMovement>().Tomb = Tomb;
            civilianInstantiate.transform.position = pos;
            used.Add(ran);
            Civilians.Add(civilianInstantiate);
            CFM.Add(civilianInstantiate.GetComponent<civilianMovement>());
        }
        for (int i = 0; i < spawnManiacs; i++)
        {
            int ran = 0;
            maniacInstantiate = Instantiate(maniacPrefab);
            maniacInstantiate.transform.position = Vector2.zero;
            Vector2 pos = Vector2.zero;
            while (used.Contains(ran) || pos == Vector2.zero)
            {
                ran = Random.Range(0, 20);
                pos = spawnPos[ran];
            }
            maniacInstantiate.GetComponent<murderScript>().gameManager = this;
            maniacInstantiate.GetComponent<murderScript>().Player = Player;
            maniacInstantiate.transform.position = pos;
            used.Add(ran);
            Maniacs.Add(maniacInstantiate);
        }

    }

    void Update()
    {
        QualitySettings.vSyncCount = 1;
        if (Canvases[2].activeSelf == true)
        {
            if (Points > highScore)
            {
                highScore = Points;
                saveSystem.savehighScore(this, 1, null);
            }
            highscoreText.text = highScore.ToString();
            scoreText.text = Points.ToString();
        }
        activeTexts = new List<bool>();
        for (int t = 0; t < mainTexts.Length; t++)
        {
            activeTexts.Add(mainTexts[t].activeSelf);
        }
        if (Timer < 100.0f)
        {
            Timer += Time.deltaTime;
            pointsText.text = Points.ToString();
        }
        else
        {
            StartCoroutine(waitforendofAnimation(mainTexts[5], true, false));
        }
        if (Canvases[1].activeSelf == true)
        {
            pausemenuElements[3].GetComponent<TextMeshProUGUI>().text = pausemenuElements[2].GetComponent<Slider>().value.ToString();
            if (highScore > Points)
            {
                pausemenuElements[5].GetComponent<TextMeshProUGUI>().text = highScore.ToString();
            }
            else
            {
                highScore = Points;
                pausemenuElements[5].GetComponent<TextMeshProUGUI>().text = highScore.ToString();
            }
        }
        if (Player.Lifes == 3)
        {
            lifesonCanvas.transform.GetChild(0).gameObject.SetActive(true);
            lifesonCanvas.transform.GetChild(1).gameObject.SetActive(true);
            lifesonCanvas.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (Player.Lifes == 2)
        {
            lifesonCanvas.transform.GetChild(0).gameObject.SetActive(true);
            lifesonCanvas.transform.GetChild(1).gameObject.SetActive(true);
            lifesonCanvas.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (Player.Lifes == 1)
        {
            lifesonCanvas.transform.GetChild(0).gameObject.SetActive(true);
            lifesonCanvas.transform.GetChild(1).gameObject.SetActive(false);
            lifesonCanvas.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (playerDied == false && Player.Lifes <= 0)
        {
            lifesonCanvas.transform.GetChild(0).gameObject.SetActive(false);
            Player.GetComponents<AudioSource>()[3].Play();
            playerDied = true;
            Destroy(Player.GetComponent<SpriteRenderer>());
            StartCoroutine(waitforendofAnimation(mainTexts[2], true, false));
        }
        if (Timer >= 50.0f && firstShow == false)
        {
            StartCoroutine(waitforendofAnimation(mainTexts[7], false, true));
            firstShow = true;
        }
        else if (Timer >= 75.0f && secondShow == false)
        {
            StartCoroutine(waitforendofAnimation(mainTexts[8], false, true));
            secondShow = true;
        }
        else if (Timer >= 90.0f && thirdShow == false)
        {
            StartCoroutine(waitforendofAnimation(mainTexts[9], false, true));
            thirdShow = true;
        }
        if (Input.GetMouseButtonUp(0) && Canvases[1].activeSelf == false && Canvases[2].activeSelf == false)
        {
            Player.GetComponents<AudioSource>()[1].Play();
            RaycastHit hit;
            Vector2 MPos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(MPos);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hited = hit.collider.transform.parent.gameObject;
                murderScript Maniac = hited.GetComponent<murderScript>();
                civilianMovement Civilian = hited.GetComponent<civilianMovement>();
                if (Maniac != null)
                {
                    if (Maniac.Targeting != null)
                    {
                        alreadyTargeted.Remove(Maniac.Targeting);
                    }
                    Maniacs.Remove(Maniac.gameObject);
                    if (Maniac.isInformating == true)
                    {
                        mainTexts[1].SetActive(false);
                    }
                    Destroy(Maniac.gameObject);
                    if (Player.Lifes < 3)
                    {
                        Player.Lifes++;
                    }
                    Player.GetComponents<AudioSource>()[4].Play();
                    StartCoroutine(waitforendofAnimation(mainTexts[3], false, false));
                    Points += 15;
                }
                else if (Civilian != null)
                {
                    if (Civilian.Alive == true)
                    {
                        Civilians.Remove(Civilian.gameObject);
                        CFM.Remove(Civilian);
                        if (alreadyTargeted.Contains(Civilian.gameObject))
                        {
                            alreadyTargeted.Remove(Civilian.gameObject);
                        }
                        Destroy(Civilian.gameObject);
                        Player.Lifes--;
                        Points -= 15;
                        Player.GetComponents<AudioSource>()[5].Play();
                        StartCoroutine(waitforendofAnimation(mainTexts[6], false, false));
                    }
                }
            }
        }
        if (Civilians.Count == 0)
        {
            StartCoroutine(waitforendofAnimation(mainTexts[2], true, false));
        }
        if (Maniacs.Count == 0)
        {
            StartCoroutine(waitforendofAnimation(mainTexts[4], true, false));
        }
    }
    private IEnumerator waitforendofAnimation(GameObject target, bool end, bool rightNow)
    {
        if (!activeTexts.Contains(true))
        {
            if (target == mainTexts[4])
            {
                Points += 50;
            }
            else if (target == mainTexts[2])
            {
                Points -= 50;
            }
            target.SetActive(true);
            if (end == true)
            {
                for (int c = 0; c < CFM.Count; c++)
                {
                    Points += 10;
                }
                for (int m = 0; m < Maniacs.Count; m++)
                {
                    Points -= 50;
                }
                if (Points < 0)
                {
                    Points = 0;
                }
                //target.GetComponent<TextMeshProUGUI>().text = target.GetComponent<TextMeshProUGUI>().text + "(" + Points + " points)";
            }
            yield return new WaitForSeconds(1.5f);
            target.SetActive(false);
            if (end == true)
            {
                Time.timeScale = 0;
                if (highScore > Points)
                {
                    highscoreText.text = "High score: " + highScore.ToString();
                    saveSystem.savehighScore(this, 1, null);
                }
                else
                {
                    highScore = Points;
                    saveSystem.savehighScore(this, 1, null);
                    highscoreText.text = "High score: " + highScore.ToString();
                }
                scoreText.text = "Score: " + Points.ToString();
                Canvases[2].SetActive(true);
                Canvases[0].SetActive(false);
            }
        }
        else if (activeTexts.Contains(true) && rightNow == false)
        {
            StartCoroutine(waitforOrder(target, end));
        }
    }
    public void Pause()
    {
        Player.GetComponents<AudioSource>()[2].Play();
        Time.timeScale = 0;
        Canvases[0].SetActive(false);
        Canvases[1].SetActive(true);
    }
    public void stopPause()
    {
        //Player.GetComponents<AudioSource>()[2].Play();
        Time.timeScale = 1;
        Canvases[0].SetActive(true);
        Canvases[1].SetActive(false);
        Volume = pausemenuElements[2].GetComponent<Slider>().value;
        AudioListener.volume = Volume / 20;
        saveSystem.savehighScore(this, 2, null);
    }
    private IEnumerator waitforOrder(GameObject saveTarget, bool saveEnd)
    {
        yield return new WaitUntil(() => !activeTexts.Contains(true));
        StartCoroutine(waitMore(saveTarget, saveEnd));
    }
    private IEnumerator waitMore(GameObject saveTarget, bool saveEnd)
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 0.1f));
        if (!activeTexts.Contains(true))
        {
            StartCoroutine(waitforendofAnimation(saveTarget, saveEnd, false));
        }
    }
    public void mainMenu()
    {
        if (Points > highScore)
        {
            saveSystem.savehighScore(this, 3, null);
        }
        else
        {
            saveSystem.savehighScore(this, 2, null);
        }
        SceneManager.LoadScene("mainMenu");
    }
    public void playAgain()
    {
        if (Points > highScore)
        {
            saveSystem.savehighScore(this, 1, null);
        }
        SceneManager.LoadScene("SampleScene");
    }
}
