using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private MovingPlatformDest _destPoint;
    [SerializeField] private float _speed = 1f;

    private int _targetPointIndex;

    private Transform _previousPoint;
    private Transform _targetPoint;

    private float _timeToPoint;
    private float _elapsedTime;

    private void Start()
    {
        TargetNextPoint();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        float elapsedPercentage = _elapsedTime / _timeToPoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(_previousPoint.position, _targetPoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(_previousPoint.rotation, _targetPoint.rotation, elapsedPercentage);

        if (elapsedPercentage >= 1)
        {
            TargetNextPoint();
        }
    }

    private void TargetNextPoint()
    {
        _previousPoint = _destPoint.GetDestPoint(_targetPointIndex);
        _targetPointIndex = _destPoint.GetNextIndex(_targetPointIndex);
        _targetPoint = _destPoint.GetDestPoint(_targetPointIndex);

        _elapsedTime = 0;

        float distanceToPoint = Vector3.Distance(_previousPoint.position, _targetPoint.position);
        _timeToPoint = distanceToPoint / _speed;
    }

}
