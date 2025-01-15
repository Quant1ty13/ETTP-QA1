using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayerHandler playerhandler;
    [SerializeField] private GameObject cutsceneUI;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        cutsceneUI.SetActive(true);
        playerhandler.enabled = false;
    }
}
