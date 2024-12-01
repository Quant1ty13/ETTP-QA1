using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGUI : MonoBehaviour
{
    [SerializeField] Player playerinfo;


    private void OnGUI()
    {
        GUI.Box(new Rect(20, 25, 150, 25), "Health: " + playerinfo.playerHealth);
    }
}
