using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : PlayerStat
{
    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject PauseMenu;
    public AudioSource music;
    public PlayerController pausemenu_input;
    private bool PauseMenuOn;

    private void Awake()
    {
        pausemenu_input = new PlayerController();
        pausemenu_input.Action.Pause.performed += enablepause => EnablePauseMenu();
    }
    private void Start()
    {
        // Instantiating audio values

        music = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
        music.volume = PlayerPrefs.GetFloat("MusicVolume");
    }

    private void OnEnable()
    {
        pausemenu_input.Enable();
    }

    private void OnDisable()
    {
        pausemenu_input.Disable();
    }
    private void Update()
    {
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
            GameOver.SetActive(true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Reset"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the scene
        }
    }
    // PUT EVERYTHING BELOW HERE INTO A DIFFERENT PAUSE MENU SCRIPT
    private void EnablePauseMenu()
    {
        Debug.Log("is this work?");
        if (PauseMenuOn == true)
        {
            ExitPause();
        }
        else if (PauseMenuOn == false)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0f;
            PauseMenuOn = true;
        }
    }

    public void ExitPause()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        PauseMenuOn = false;
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
