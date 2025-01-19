using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CutsceneManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public string sceneName;

    public virtual void CutsceneSkip()
    {
        SceneManager.LoadScene(sceneName);
    }
}
