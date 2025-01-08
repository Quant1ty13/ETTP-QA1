using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FreeFallCamZone : MonoBehaviour
{
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera freeFallCam;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            freeFallCam.Priority = 11;
            mainCam.Priority = 9;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        mainCam.Priority = 11;
        freeFallCam.Priority = 9;
    }
}
