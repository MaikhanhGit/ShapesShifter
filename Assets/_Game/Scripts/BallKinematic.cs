using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallKinematic : MonoBehaviour
{
    private Rigidbody _rb;
    private float _movementX;
    private float _movementY;
    [SerializeField] private float _speed = 10f;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        _movementX = movementVector.x;
        _movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        // movement
        Vector3 movement = new Vector3(_movementX, 0f, _movementY);

        transform.position += movement * _speed * Time.fixedDeltaTime;

        // rotation
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement).normalized;
        transform.Rotate(rotationAxis, movement.magnitude * _speed * Time.fixedDeltaTime);  
    }
}
