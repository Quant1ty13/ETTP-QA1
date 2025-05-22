using UnityEngine;


[CreateAssetMenu(menuName = "BaseAmbience")]
public class BaseAmbience : ScriptableObject
{
    public AudioClip[] ambienceSFX;
    public float maxAmbienceWait;
    public float lowestAmbienceWait;
}
