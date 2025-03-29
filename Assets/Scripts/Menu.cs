using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject StartScreen, SettingsScreen;
    public GameObject Audio;
    public AudioSource bgsound;
    public Slider VolumeSlider;
    public static float currentvalue = .5f;
void Start()
    {
        if (GameManager.firsttime == false)
        {
            currentvalue = Sunshine.currentvolume;
            VolumeSlider.value = currentvalue;
        }
        StartScreen.SetActive(true);
        SettingsScreen.SetActive(false);
        bgsound = Audio.GetComponent<AudioSource>();
        bgsound.Play();
        if (VolumeSlider != null && bgsound != null)
        {
            bgsound.volume = VolumeSlider.value;
            currentvalue = VolumeSlider.value;
        }
    }
    public void OpenSettings()
    {
        StartScreen.SetActive(false);
        SettingsScreen.SetActive(true);
    }

    public void BackToStart()
    {
        StartScreen.SetActive(true);
        SettingsScreen.SetActive(false);
    }

    public void OnVolumeChanged()
    {
        if (bgsound != null)
        {
            bgsound.volume = VolumeSlider.value;
            currentvalue = VolumeSlider.value;
        }
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
}