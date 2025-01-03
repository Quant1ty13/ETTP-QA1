using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPlay : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject CrownCollectMenu;
    public AudioSource music;
    public AudioSource soundfxManager;

    private void Start()
    {
        if (music != null && soundfxManager != null)
        {
            music.volume = PlayerPrefs.GetFloat("MusicVolume");
            soundfxManager.volume = PlayerPrefs.GetFloat("SoundFXVolume");
        }
    }

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

    public void CrownCollected()
    {
        GameObject.Find("Player").GetComponent<PlayerHandler>().enabled = false;
        CrownCollectMenu.SetActive(true);
    }

    public void NextLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}
