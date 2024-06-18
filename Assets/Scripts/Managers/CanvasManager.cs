using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Audio;
using System.Collections.Specialized;
using System;

[RequireComponent(typeof(AudioSource))]

public class CanvasManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [SerializeField] private AudioClip pauseClip;
    [SerializeField] private AudioClip buttonClip;
    private AudioSource audioSource;
    private Coroutine changeAudio = null;

    [Header("Button")]
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;
    public Button backButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button returnToMenu;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    [Header("Text")]
    public TMP_Text healthText;
    public TMP_Text coinText;
    public TMP_Text masterVolSliderText;
    public TMP_Text musicVolSliderText;
    public TMP_Text SFXVolSliderText;

    [Header("Images")]
    public GameObject axeImage;
    public GameObject knifeImage;

    [Header("Slider")]
    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider SFXVolSlider;

    IEnumerator WaitChangeSceneCoroutine(float time, string scene)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.LoadScene(scene);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (quitButton)
            quitButton.onClick.AddListener(Quit);

        if (resumeButton)
            resumeButton.onClick.AddListener(delegate {
                audioSource.PlayOneShot(pauseClip);
                GameManager.Instance.UnPause();
                SetMenus(null, pauseMenu);
            });

        if (returnToMenu)
            returnToMenu.onClick.AddListener(delegate {
                audioSource.PlayOneShot(buttonClip);
                GameManager.Instance.UnPause();
                changeAudio = StartCoroutine(WaitChangeSceneCoroutine(0.5f, "Title"));
            });

        if (playButton)
            playButton.onClick.AddListener(delegate {
                audioSource.PlayOneShot(buttonClip);
                changeAudio = StartCoroutine(WaitChangeSceneCoroutine(0.5f, "Level1"));
            });

        if (settingsButton)
            settingsButton.onClick.AddListener(delegate {
                audioSource.PlayOneShot(buttonClip);
                SetMenus(settingsMenu, mainMenu);
            });

        if (backButton)
            backButton.onClick.AddListener(delegate {
                audioSource.PlayOneShot(buttonClip);
                SetMenus(mainMenu, settingsMenu);
            });

        if (masterVolSlider)
        {
            SetupSliderInfo(masterVolSlider, masterVolSliderText, "MasterVol");
        }
        if (musicVolSlider)
        {
            SetupSliderInfo(musicVolSlider, musicVolSliderText, "MusicVol");
        }
        if (SFXVolSlider)
        {
            SetupSliderInfo(SFXVolSlider, SFXVolSliderText, "SFXVol");
        }

        if (healthText)
        {
            GameManager.Instance.onHealthValueChange += OnHealthValueChanged;
            healthText.text = $"Health: {GameManager.Instance.health}";
        }

        if (coinText)
        {
            GameManager.Instance.onCoinValueChange += OnCoinValueChanged;
            coinText.text = $"Coins: {GameManager.Instance.coins}";
        }

        if (axeImage)
        {
            GameManager.Instance.PlayerInstance.onAxeValueChange += OnAxeValueChanged;
        }
        if (knifeImage)
        {
            GameManager.Instance.PlayerInstance.onKnifeValueChange += OnKnifeValueChanged;
        }
    }

    void SetupSliderInfo(Slider mySlider, TMP_Text sliderText, string parameterName)
    {
        mySlider.onValueChanged.AddListener((value) => onSliderValueChanged(value, sliderText, parameterName));

        if (sliderText)
            sliderText.text = Convert.ToInt16(mySlider.value * 100).ToString() + "%";

        float value = (mySlider.value == 0.0f) ? -80.0f : 20.0f * Mathf.Log10(mySlider.value);
        audioMixer.SetFloat(parameterName, value);
    }

    void OnHealthValueChanged(int value)
    {
        if (healthText)
            healthText.text = $"Health: {value}";
    }

    void OnCoinValueChanged(int value)
    {
        if (coinText)
            coinText.text = $"Coins: {value}";
    }

    void OnAxeValueChanged(bool value)
    {
        if (axeImage)
            axeImage.SetActive(true);
    }

    void OnKnifeValueChanged(bool value)
    {
        if (knifeImage)
            knifeImage.SetActive(true);
    }

    void onSliderValueChanged(float value, TMP_Text volSliderText, string mixerParameterName)
    {
        if (volSliderText)
            volSliderText.text = (value == -80.0f) ? "0%" : (Convert.ToInt32(value * 100)).ToString() + "%";

        value = (value == 0.0f) ? -80.0f : 20.0f * Mathf.Log10(value);

        audioMixer.SetFloat(mixerParameterName, value);
    }

    void SetMenus(GameObject menuToActivate, GameObject menuToDeactivate)
    {
        if (menuToActivate)
            menuToActivate.SetActive(true);

        if (menuToDeactivate)
            menuToDeactivate.SetActive(false);
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            audioSource.PlayOneShot(pauseClip);
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            if (pauseMenu.activeSelf == true)
                GameManager.Instance.Pause();
            else
                GameManager.Instance.UnPause();
        }
    }
}
