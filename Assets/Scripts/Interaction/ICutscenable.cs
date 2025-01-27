using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public interface ICutscenable
{
    public void PlayCutscene();
    public void EndCutscene();
    public void SkipCutscene();
}
