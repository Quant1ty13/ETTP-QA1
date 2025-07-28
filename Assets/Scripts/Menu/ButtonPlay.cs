using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonPlay : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject Player;
    [SerializeField] PlayerHandler PlayerHandler;
    [SerializeField] GameObject CrownCollectMenu;
    [SerializeField] private GameObject CrownFirstOption;
    public AudioSource music;
    public AudioSource soundfxManager;

    private void Start()
    {
        if (music != null && soundfxManager != null)
        {
            music.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
            soundfxManager.volume = PlayerPrefs.GetFloat("SoundFXVolume", 1);
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the scene
    }

    public void ExitTutorial()
    {
        Time.timeScale = 1;
        Player.SetActive(true); // sets player game object to true
        tutorial.SetActive(false);
    }

    public void CrownCollected()
    {
        PlayerHandler.AccessPlayerInputs = false;
        PlayerHandler.pausemenu_script.GameConcluded = true;
        CrownCollectMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(CrownFirstOption);
        Time.timeScale = 0;
    }

    public void NextLevel(int level)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(level);
    }

    public void ResetToCheckpoint()
    {
        Time.timeScale = 1;
        PlayerHandler.rb2d.gravityScale = PlayerHandler.originalGravityScale;
        PlayerHandler.pausemenu_script.GameConcluded = false;
        PlayerHandler.AccessPlayerInputs = true;
        CrownCollectMenu.SetActive(false);
        Player.transform.position = PlayerHandler.lastCheckpointLocation;
    }
}
