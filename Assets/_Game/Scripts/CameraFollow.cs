using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _followSpeed = 2f;
    [SerializeField] private Transform _target = null;
    [SerializeField] private float _yOffset = 1f;
    [SerializeField] private float _xOffset = 2f;
    [SerializeField] private float _zOffset = -10f;

    private void Update()
    {
        Vector3 newPos = new Vector3(_target.position.x + _xOffset, 
            _target.position.y + _yOffset, _zOffset);
        transform.position = Vector3.Slerp(transform.position, newPos, _followSpeed*Time.deltaTime);
    }
}
