using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPBubble : MonoBehaviour
{
    private const float COOLDOWN_TIME = 1.25f;
    private GameObject _player;

    [SerializeField] bool _isOneWay = false;

    private bool _isActive;
    private float _timeCounter;

    [SerializeField] private GameObject _bubblePosition;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _isActive == false && _isOneWay == false)
        {
            // consideration to also enable dashing when the player enters this bubble
            _isActive = true;
            _player.transform.position = new Vector2(_bubblePosition.transform.position.x, _bubblePosition.transform.position.y);

        }
    }

    private void Update()
    {
        if (_isActive)
        {
            _timeCounter += Time.deltaTime;
        }

        if (_timeCounter >= COOLDOWN_TIME)
        {
            _isActive = false;
            _timeCounter = 0;
        }
    }
}
