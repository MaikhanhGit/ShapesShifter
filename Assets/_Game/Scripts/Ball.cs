using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Security.Claims;
using TMPro;
using Unity.VisualScripting;
using Cinemachine;

public class Ball : MonoBehaviour
{
    private Rigidbody _rb;
    private float _movementX;
    private float _movementY;
    private int _numChildren = 0;    
    private static float globalGravity = -9.81f;    
    private Vector3 _gravityVector;      

    [Header("Ball Movement")]
    [SerializeField] float _pushForce = 20f;
    [SerializeField] float _rollForce = 10f;
    [SerializeField] float _pushForceAfter = 2f;
    [SerializeField] float _jumpForce = 3f;
    [SerializeField] float _gravityScale = 1f;
    [SerializeField] float _fallGravityScale = 5f;            
    [SerializeField] private float _clampingValue = 3f;            

    [Header("Ground Detection")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] float _groundCheckDistance = 0f;
    private float bufferCheckDistance = 0.1f;
    public bool _isGrounded = false;

    [Header("Game Play")]
    [SerializeField] GameObject[] _geos = null;
    public int _currentNumChildren = 0;
        


    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _numChildren = gameObject.transform.childCount;       
        _currentNumChildren = _numChildren;
            
    }   

   void OnMove(InputValue movementValue)
    {
        Vector2 movementVector  = movementValue.Get<Vector2>();
        _movementX = movementVector.x;
        _movementY = movementVector.y;        
    }

  void OnJump()
    {
        if (_isGrounded)
        {            
            _rb.AddForce(Vector3.up *  _jumpForce, ForceMode.Impulse);        
        }               
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance, layerMask);
    }

    private void FixedUpdate()
    {
        // artificial gravity
        if (_rb.velocity.y > 0)
        {
            _gravityVector = globalGravity * _gravityScale * Vector3.up;
        }
        if (_rb.velocity.y <= 0)
        {
            _gravityVector = globalGravity * _fallGravityScale * Vector3.up;
            
        }
        _rb.AddForce(_gravityVector, ForceMode.Acceleration);        
        
        // Movement
        Vector3 movement = new Vector3(_movementX, 0f , _movementY);
        _currentNumChildren = gameObject.transform.childCount;
        Vector3 pushVector = movement * _pushForce * Time.fixedDeltaTime;
        Vector3 rollVector = movement * _rollForce * Time.fixedDeltaTime;

        //Clamping

        if (pushVector.x < -_clampingValue)
        {
            pushVector.x = -_clampingValue;
        }
        if (pushVector.x > _clampingValue)
        {
            pushVector.x = _clampingValue;
        }
        if (pushVector.z < -_clampingValue)
        {
            pushVector.z = -_clampingValue;
        }
        if (pushVector.z > _clampingValue)
        {
            pushVector.z = _clampingValue;
        }

        if (rollVector.x < -_clampingValue)
        {
            rollVector.x = -_clampingValue;
        }
        if (rollVector.x > _clampingValue)
        {
            rollVector.x = _clampingValue;
        }
        if (rollVector.z < -_clampingValue)
        {
            rollVector.z = -_clampingValue;
        }
        if (rollVector.z > _clampingValue)
        {
            rollVector.z = _clampingValue;
        }

        _rb.AddForce(pushVector, ForceMode.Acceleration);

        _rb.AddTorque(rollVector, ForceMode.Acceleration);        

        /*
                if (_currentNumChildren >= 2)
                {
                    _rb.AddForce(movement * _pushForce * Time.fixedDeltaTime);

                    _rb.AddTorque(movement * _rollForce * Time.fixedDeltaTime);
                    //_rb.velocity = movement * _pushForce;
                }

                if (_currentNumChildren < 2)
                {            
                    Vector3 gravityVector =  Vector3.up * (-9.81f);
                    _rb.AddForce(gravityVector * 100 * Time.fixedDeltaTime, ForceMode.Acceleration);
                    //_rb.angularDrag = 0f;
                    //_rb.drag = 0f;
                    //_rb.mass = 0f;
                    // _rb.automaticInertiaTensor = false;
                    //_rb.inertiaTensorRotation = Quaternion.identity;
                    //_rb.inertiaTensor = new Vector3(1f, 1f, 1f);
                    gameObject.transform.rotation = Quaternion.identity;
                    _rb.constraints = RigidbodyConstraints.FreezeRotationY;


                    _rb.velocity = new Vector3(_movementX * _pushForceAfter,
                        0f, _movementY * _pushForceAfter);
                }

                */
    }

    
    
}
