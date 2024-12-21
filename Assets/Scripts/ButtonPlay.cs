using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPlay : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject Player;

    [Header("Main Menu Variables")]
    [SerializeField] GameObject M_Menu;
    [SerializeField] GameObject M_Settings;
    [SerializeField] GameObject M_LevelSelect;

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the scene
    }

    public void ExitTutorial()
    {
        Player.SetActive(true); // sets player game object to true
        tutorial.SetActive(false);
    }


    // Buttons for Main Menu
    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Settings_Main()
    {
        M_Menu.SetActive(false);
        M_Settings.SetActive(true);
    }

    public void DisableSettings_Main()
    {
        M_Menu.SetActive(true);
        M_Settings.SetActive(false);
    }

    public void LevelSelect()
    {
        M_Menu.SetActive(false);
        M_LevelSelect.SetActive(true);
    }

    public void DisableLevelSelect()
    {
        M_Menu.SetActive(true);
        M_LevelSelect.SetActive(false);
    }
}
