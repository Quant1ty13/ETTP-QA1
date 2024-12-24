using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class Gamepad_Menu : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    public GameObject MainMenu, SettingsMenu, SelectLevelMenu;
    [Header("First Options")]
    public GameObject mainFirstOption, settingsFirstOption, levelButton;
    [Header("Exit Buttons")]
    public GameObject settingsExit, levelExit;

    void Start()
    {
        EventSystem.current.SetSelectedGameObject(mainFirstOption);
    }

    void Update()
    {
        /// PS !!! : your not done yet bro make level selector controller-friendly when you're done with all the levels.
    }


    public void Play()
    {
        SceneManager.LoadScene(4);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Settings_Main()
    {

        // Setup selected button
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstOption);

        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void DisableSettings_Main()
    {
        //Setup selected button
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsExit);

        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void LevelSelect()
    {
        // Setup selected button
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(levelButton);

        MainMenu.SetActive(false);
        SelectLevelMenu.SetActive(true);
    }

    public void DisableLevelSelect()
    {
        //Setup selected button
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(levelExit);

        MainMenu.SetActive(true);
        SelectLevelMenu.SetActive(false);
    }
}
