using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindExternalVelocityTrigger : MonoBehaviour
{
    private WindVelocityController _windVelocityController;
    private GameObject _player;
    private Material _material;

    private Rigidbody2D _playerRb2d;

    private bool _easeInCoroutineRunning;
    private bool _easeOutCoroutineRunning;

    private int _externalInfluence = Shader.PropertyToID("_ExternalInfluence");

    private float _startingXVelocity;
    private float _velocityLastFrame;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerRb2d = _player.GetComponent<Rigidbody2D>();
        _windVelocityController = GetComponentInParent<WindVelocityController>();

        _material = GetComponent<SpriteRenderer>().material;
        _startingXVelocity = _material.GetFloat(_externalInfluence);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _player)
        {
            if (!_easeInCoroutineRunning && Mathf.Abs(_playerRb2d.velocity.x) > Mathf.Abs(_windVelocityController.VelocityThreshold))
            {
                StartCoroutine(EaseIn(_playerRb2d.velocity.x * _windVelocityController.ExternalInfluenceStrength));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _player)
        {
            StartCoroutine(EaseOut());
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == _player)
        {
            if (Mathf.Abs(_velocityLastFrame) > Mathf.Abs(_windVelocityController.VelocityThreshold) &&
                Mathf.Abs(_playerRb2d.velocity.x) < Mathf.Abs(_windVelocityController.VelocityThreshold))
            {
                StartCoroutine(EaseOut());
            }
            else if (Mathf.Abs(_velocityLastFrame) < Mathf.Abs(_windVelocityController.VelocityThreshold) &&
                Mathf.Abs(_playerRb2d.velocity.x) > Mathf.Abs(_windVelocityController.VelocityThreshold))
            {
                StartCoroutine(EaseIn(_playerRb2d.velocity.x * _windVelocityController.ExternalInfluenceStrength));
            }
            else if(!_easeInCoroutineRunning && !_easeOutCoroutineRunning)
            {
                _windVelocityController.InfluenceGrass(_material, _playerRb2d.velocity.x * _windVelocityController.ExternalInfluenceStrength);
            }


            _velocityLastFrame = _playerRb2d.velocity.x;
        }
    }

    private IEnumerator EaseIn(float Xvelocity)
    {
        _easeInCoroutineRunning = true;

        float elapsedTime = 0f;
        while (elapsedTime < _windVelocityController.EaseInTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmount = Mathf.Lerp(_startingXVelocity, Xvelocity, (elapsedTime / _windVelocityController.EaseInTime));
            _windVelocityController.InfluenceGrass(_material, lerpedAmount);

            yield return null;
        }

        _easeInCoroutineRunning = false;
    }

    private IEnumerator EaseOut()
    {
        _easeOutCoroutineRunning = true;
        float currentXInfluence = _material.GetFloat(_externalInfluence);

        float elapsedTime = 0f;
        while (elapsedTime < _windVelocityController.EaseOutTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmount = Mathf.Lerp(currentXInfluence, _startingXVelocity, (elapsedTime / _windVelocityController.EaseOutTime));
            _windVelocityController.InfluenceGrass(_material, lerpedAmount);

            yield return null;
        }

        _easeOutCoroutineRunning = false;
    }
}
