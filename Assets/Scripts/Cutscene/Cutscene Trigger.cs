using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayerHandler playerhandler;
    [SerializeField] private GameObject cutsceneUI;
    [SerializeField] private PlayableDirector cutsceneClip;

    private void Awake()
    {
        cutsceneClip.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        cutsceneUI.SetActive(true);
        playerhandler.enabled = false;
    }
}
