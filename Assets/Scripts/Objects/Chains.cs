using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chains : MonoBehaviour
{
    private HingeJoint2D _hj2D;
    private JointMotor2D _hjMotor2D;
    private float _timeCounter;
    private float _timeVariance = 1;
    [SerializeField] private float _motorSpeed = 1.35f;

    private void Start()
    {
        _hj2D = GetComponent<HingeJoint2D>();
        _hjMotor2D = _hj2D.motor;
    }

    private void Update()
    {
        if (_timeCounter >= _timeVariance)
        {
            _timeCounter = 0;
            _motorSpeed *= -1f;
            _timeVariance = Random.Range(0.9f,1.15f);
            _hjMotor2D.motorSpeed = _motorSpeed;
            _hj2D.motor = _hjMotor2D;
        }

        _timeCounter += Time.deltaTime;
    }
}
