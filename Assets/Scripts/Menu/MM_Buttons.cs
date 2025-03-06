using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class MM_Buttons : MonoBehaviour
{
    [Header("Menu Object References")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject ChapterSelect;
    [SerializeField] private GameObject selected_GO;

    //private bool currentSwitch;

    [Header("Start-Up")]
    public AudioSource music;
    public AudioSource soundfxManager;
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(selected_GO);

        if (music != null && soundfxManager != null)
        {
            music.volume = PlayerPrefs.GetFloat("MusicVolume");
            soundfxManager.volume = PlayerPrefs.GetFloat("SoundFXVolume");
        }
    }

/*    private void Update()
    {
        if (EventSystem.current.GameObject() == null && currentSwitch == true)
        {
            Debug.Log("Detecting null game object! Setting current game object back");
            EventSystem.current.SetSelectedGameObject(selected_GO);
            currentSwitch = false;
        }
    }*/

    public void FromMainMenu(GameObject enteringObj)
    {
        enteringObj.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void ToMainMenu(GameObject exitingObj)
    {
        exitingObj.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void ToChapterSelect(GameObject exitingObj)
    {
        exitingObj.SetActive(false);
        ChapterSelect.SetActive(true);
    }

    public void FromChapterSelect(GameObject enteringObj)
    {
        enteringObj.SetActive(true);
        ChapterSelect.SetActive(false);
    }

    public void TempLevelLoader(int levelID)
    {
        SceneManager.LoadScene(levelID);
    }

    public void ChooseFirstSelect(GameObject firstSelect)
    {
        selected_GO = firstSelect;

        //currentSwitch = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected_GO);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
