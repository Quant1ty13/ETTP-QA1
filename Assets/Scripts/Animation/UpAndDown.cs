using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    [SerializeField] private int animationSpeed;
    [SerializeField] private float _height;

    private Vector2 originalPos;
    private bool _moveUp = true;

    private void Awake()
    {
        originalPos = this.transform.position;
    }

    private void Update()
    {
        float truespeed = animationSpeed * Time.deltaTime;
        Vector3 object_pos = this.transform.position;


        if (object_pos.y >= originalPos.y + _height)
        {
            _moveUp = false;
        }

        if (object_pos.y <= originalPos.y - _height)
        {
            _moveUp = true;
        }

        if (_moveUp)
        {
            object_pos.y += _height * truespeed;
        }
        else
        {
            object_pos.y -= _height * truespeed;
        }

        transform.position = object_pos;
    }
}
