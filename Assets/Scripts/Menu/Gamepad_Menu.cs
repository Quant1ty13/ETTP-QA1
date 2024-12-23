using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Gamepad_Menu : MonoBehaviour
{
    public GameObject MainMenu, SettingsMenu, SelectLevelMenu;
    public GameObject mainFirstOption, settingsFirstOption, levelButton;
    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(mainFirstOption);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
