using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Settings;
    private bool PauseMenuOn;

    public AudioSource music;
    public float musicvolume;
    public Slider musicSlider;

    public AudioSource soundfxManager;
    public Slider soundfxSlider;
    public float soundfxvolume;

    public GameObject firstSelectMain;
    public GameObject firstSelectSettings;
    public GameObject SettingsButton;
    public void SetMusicVolume(float volume) { musicvolume = volume; }
    public void SetSFXVolume(float volume) { soundfxvolume = volume; }

    private void Start()
    {
        music.volume = 1;
        musicSlider.value = 1;
        soundfxManager.volume = 1;
        soundfxSlider.value = 1;
        music.volume = PlayerPrefs.GetFloat("MusicVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundfxManager.volume = PlayerPrefs.GetFloat("SoundFXVolume");
        soundfxSlider.value = PlayerPrefs.GetFloat("SoundFXVolume");
    }

    public void EnablePauseMenu()
    {
        if (PauseMenuOn == true)
        {
            ExitPause();
        }
        else if (PauseMenuOn == false)
        {
            Menu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstSelectMain);
            Time.timeScale = 0f;
            PauseMenuOn = true;
        }
    }

    public void ExitPause()
    {
        Time.timeScale = 1f;
        Menu.SetActive(false);
        PauseMenuOn = false;
    }

    public void EnableSettings()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectSettings);
        Settings.SetActive(true);
        Menu.SetActive(false);
    }

    public void DisableSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicvolume);
        PlayerPrefs.SetFloat("SoundFXVolume", soundfxvolume);
        music.volume = PlayerPrefs.GetFloat("MusicVolume");
        soundfxManager.volume = PlayerPrefs.GetFloat("SoundFXVolume");

        EventSystem.current.SetSelectedGameObject(SettingsButton);
        Menu.SetActive(true);
        Settings.SetActive(false);
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
