using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class mainmenuScript : MonoBehaviour
{
    public float Volume;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeValue;
    [SerializeField] private GameObject Heart;
    [SerializeField] private movelikeAnim H; //heart
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject playbuttonClone1;
    [SerializeField] private GameObject playbuttonClone2;

    private void Update()
    {
        QualitySettings.vSyncCount = 1;
        Volume = volumeSlider.value;
        volumeValue.text = Volume.ToString();
        AudioListener.volume = Volume / 20;
    }

    private void Start()
    {
        Time.timeScale = 1;
        saveScript data = saveSystem.loadhighScore();
        volumeSlider.value = data.volume;
        Volume = data.volume;
    }

    public void Play()
    {
        Camera.main.GetComponents<AudioSource>()[0].Play();
        H.Move();
        playButton.SetActive(false);
        playbuttonClone1.SetActive(true);
        StartCoroutine(waitforPLay1());
    }

    private IEnumerator waitforPLay1()
    {
        Camera.main.GetComponents<AudioSource>()[1].Play();
        yield return new WaitUntil(() => H.moved == true);
        Camera.main.GetComponents<AudioSource>()[2].Play();
        playbuttonClone1.SetActive(false);
        playbuttonClone2.SetActive(true);
        Heart.SetActive(false);
        StartCoroutine(waitforPlay2());
    }

    private IEnumerator waitforPlay2()
    {
        saveSystem.savehighScore(null, 2, this);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("SampleScene");
    }
    
    public void howtoPlay()
    {
        saveSystem.savehighScore(null, 2, this);
        SceneManager.LoadScene("howtoPlay");
    }
}
