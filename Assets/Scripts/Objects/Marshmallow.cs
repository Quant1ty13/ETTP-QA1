using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marshmallow : MonoBehaviour
{
    // Make Player Prefabs for Marshmallow Count and Level Marshmallow Boolean something something
    [SerializeField] private SoundFX soundfxManager;
    [SerializeField] private AudioClip marshmallowCollected_SFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        soundfxManager.PlaySFX(marshmallowCollected_SFX, false);
        Destroy(gameObject);
    }
}
