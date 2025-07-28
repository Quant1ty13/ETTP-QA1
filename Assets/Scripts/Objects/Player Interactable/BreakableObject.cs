using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private ParticleSystem BreakingParticles;
    [SerializeField] private SoundFX soundfxManager;
    [SerializeField] private AudioClip breakSFX;
    [SerializeField] private GameObject _objectsToSpawn;
    [SerializeField] private float MinAmountOfObjectsToSpawn;
    private SpriteRenderer sr;
    private PlayerHandler playerHandler;
    private bool broken = false;
    private float originalAlpha;


    private void Awake()
    {
        playerHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();
        sr = GetComponent<SpriteRenderer>();
        originalAlpha = sr.color.a;
    }

    private void Update()
    {

        if (playerHandler.activateDeathAnim == true)
        {
            if (broken == false)
            {
                return;
            }

            broken = false;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, originalAlpha);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && (playerHandler.IsDashing == true && playerHandler.rb2d.velocity.y >= -1f))
        {
            Break();
        }
    }

    private void Break()
    {
        if (broken)
        {
            return;
        }

        broken = true;
        soundfxManager.PlaySFX(breakSFX, true);
        float count = Random.Range(MinAmountOfObjectsToSpawn, MinAmountOfObjectsToSpawn + 2);
        for (int i = 0; i < count; i++)
        {
            ObjectPoolManager.SpawnObject(_objectsToSpawn, new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z), Quaternion.identity);
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        BreakingParticles.Play();
    }
}
