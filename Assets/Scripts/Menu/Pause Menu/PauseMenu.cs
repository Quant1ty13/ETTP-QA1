using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Settings;
    private bool PauseMenuOn;
    private bool SettingsOn;

    public AudioSource music;
    public float musicvolume;
    public Slider musicSlider;

    public AudioSource soundfxManager;
    public Slider soundfxSlider;
    public float soundfxvolume;

    public Toggle automaticWallClimbing_checkBox;
    public bool enableAutomaticWallClimbing;
    private string PlayerPrefAutoWallClimbing;

    public GameObject firstSelectMain;
    public GameObject firstSelectSettings;
    public GameObject SettingsButton;
    public void SetMusicVolume(float volume) { musicvolume = volume; }
    public void SetSFXVolume(float volume) { soundfxvolume = volume; }
    public void EnableAutomaticWallClimbing(bool eAWC) { enableAutomaticWallClimbing = eAWC; }

    private void Start()
    {
        PlayerPrefAutoWallClimbing = PlayerPrefs.GetString("AutomaticWallClimbing");
        switch (PlayerPrefAutoWallClimbing)
        {
            case "True":
                enableAutomaticWallClimbing = true;
                break;
            case "False":
                enableAutomaticWallClimbing = false;
                break;
        }

        music.volume = PlayerPrefs.GetFloat("MusicVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundfxManager.volume = PlayerPrefs.GetFloat("SoundFXVolume");
        soundfxSlider.value = PlayerPrefs.GetFloat("SoundFXVolume");

        automaticWallClimbing_checkBox.isOn = enableAutomaticWallClimbing;
    }

    public void EnablePauseMenu()
    {
        if (PauseMenuOn == true && SettingsOn == false)
        {
            ExitPause();
        }
        else if (PauseMenuOn == false && SettingsOn == false)
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
        SettingsOn = true;

        EventSystem.current.SetSelectedGameObject(firstSelectSettings);
        Settings.SetActive(true);
        Menu.SetActive(false);
    }

    public void DisableSettings()
    {
        SettingsOn = false;

        PlayerPrefs.SetFloat("MusicVolume", musicvolume);
        PlayerPrefs.SetFloat("SoundFXVolume", soundfxvolume);
        PlayerPrefs.SetString("AutomaticWallClimbing", enableAutomaticWallClimbing.ToString());
        music.volume = PlayerPrefs.GetFloat("MusicVolume");
        soundfxManager.volume = PlayerPrefs.GetFloat("SoundFXVolume");
        Debug.Log(enableAutomaticWallClimbing);

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
