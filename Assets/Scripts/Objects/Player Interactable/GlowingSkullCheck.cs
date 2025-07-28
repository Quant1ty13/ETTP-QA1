using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingSkullCheck : MonoBehaviour
{
    public bool isRightCheck = false;
    public GameObject GlowingSkull;
    public GameObject RightCheck;
    public GameObject LeftCheck;
    private bool hasTurned;

    private Vector3 rightRotate = Vector3.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isRightCheck == true) { RotateRight(); }            
            else if (isRightCheck == false) { RotateLeft(); }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isRightCheck == false)
        {
            hasTurned = false;
        }
    }

    public void RotateRight()
    {
        Debug.Log("entered right check");
        if (GlowingSkull.transform.rotation.y != 0)
        {
            GlowingSkull.transform.rotation = Quaternion.Euler(rightRotate);
        }
    }

    public void RotateLeft()
    {
        if (GlowingSkull.transform.rotation.y == 0 && hasTurned == false)
        {
            hasTurned = true;
            Debug.Log("entered left check");
            GlowingSkull.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
