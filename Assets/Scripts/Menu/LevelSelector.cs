using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelector : Level_ID
{
    public int LvlID { get { return LevelID; } set { LevelID = value; } }

    private void Update()
    {
        if (LvlID > 0) { LevelSelected(LvlID); }
        else { return; }
    }

    public void LevelSelected(int level)
    {
        LvlID = 0;
        SceneManager.LoadScene(level);
    }
}
