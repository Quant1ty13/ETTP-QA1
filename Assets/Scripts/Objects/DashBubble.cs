using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBubble : MonoBehaviour
{
    private const float REACTIVATION_TIMER = 2.5f;
    private PlayerHandler _playerHandler;
    private bool _activated;
    private float _timeCounter;

    // Temp
    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _playerHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _activated == false)
        {
            _sr.color = Color.gray;
            _activated = true;
            _playerHandler.ExternalEnableDashCooldown = true;
            Debug.Log("hello");
        }
    }

    private void Update()
    {
        if (_activated)
        {
            _timeCounter += Time.deltaTime;
        }

        if (_timeCounter >= REACTIVATION_TIMER)
        {
            _sr.color = Color.red;
            _activated = false;
            _timeCounter = 0;
        }
    }
}
