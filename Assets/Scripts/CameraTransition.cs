using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public GameObject transitionCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transitionCam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transitionCam.SetActive(false);
    }
}
