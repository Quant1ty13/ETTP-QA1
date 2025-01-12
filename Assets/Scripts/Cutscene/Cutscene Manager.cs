using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CutsceneManager : MonoBehaviour
{
    private PlayerController playerInput;
    public string sceneName;

    private void Awake()
    {
        playerInput = new PlayerController();

        playerInput.Cutscene.Skip.started += skip_cutscene => ChangeScene(sceneName);
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
