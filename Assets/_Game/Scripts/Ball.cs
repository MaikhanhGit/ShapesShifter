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
    private bool _isCube = false;

    [Header("Ball Movement")]
    [SerializeField] float _pushForce = 20f;
    [SerializeField] float _rollForce = 10f;
    [SerializeField] float _cubePushForce = 10f;
    [SerializeField] float _cubeRollForce = 10f;
    [SerializeField] float _jumpForce = 3f;
    [SerializeField] float _jumpVertMul = 2f;
    [SerializeField] float _cubeJumpVertMul = 5f;
    [SerializeField] float _cubeJumpForce = 10f;
    [SerializeField] float _gravityScale = 1f;
    [SerializeField] float _fallGravityScale = 5f;            
    [SerializeField] private float _originalClampingValue = 3f;
    [SerializeField] private float _cubeClampingValue = 1.5f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float _groundCheckDistance = 1f;
    [SerializeField] private float _cubeGroundCheckDistance = -0.1f;
    private float bufferCheckDistance = 0.1f;
    private float _clampingValue;
    public bool _isGrounded = false;

    [Header("Game Play")]
    [SerializeField] public GameObject[] _geos = null;
    [SerializeField] private GameObject _goldPieces = null;
    public int _currentNumChildren = 0;
    public bool _isPurpleCleared = false;
    private bool _isDragZeroOut = false;
    private Vector3 _jumpVector = new Vector3(1f, 1f, 1f);



    // Start is called before the first frame update
    void Start()
    {
        _clampingValue = _originalClampingValue;
        _rb = GetComponent<Rigidbody>();
        _numChildren = gameObject.transform.childCount;       
        _currentNumChildren = _numChildren;
        Debug.Log("NumChild: " + _currentNumChildren);
    }   

   void OnMove(InputValue movementValue)
    {
        Vector2 movementVector  = movementValue.Get<Vector2>();
        _movementX = movementVector.x;
        _movementY = movementVector.y;        
    }

  void OnJump()
    {
        Vector3 jumpVector = new Vector3(_movementX, _jumpVertMul, _movementY);
        Vector3 cubeJumpVector = new Vector3(_movementX, _cubeJumpVertMul, _movementY);

        if (_isGrounded && !_isCube)
        {                        
            _rb.AddForce(jumpVector *  _jumpForce, ForceMode.Impulse);            
        }

        if (_isGrounded && _isCube)
        {                      
            _rb.AddForce(cubeJumpVector * _cubeJumpForce, ForceMode.Impulse);            
        }
        
    }

    private void Update()
    {
        if (_currentNumChildren < 3)
        {
            _isCube = true;

            if (_isDragZeroOut == false)
            {
                gameObject.GetComponent<Rigidbody>().drag = 0f;
                gameObject.GetComponent<Rigidbody>().angularDrag = 0f;
                gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                
                _isDragZeroOut = true;
            }
        }
        else if (_currentNumChildren >= 3)
        {
            _isCube = false;

            if (_isDragZeroOut == true)
            {
                gameObject.GetComponent<Rigidbody>().drag = 0.1f;
                gameObject.GetComponent<Rigidbody>().angularDrag = 0.01f;
                gameObject.GetComponent<Rigidbody>().freezeRotation = false;

                _isDragZeroOut = false;
            }
        }



        if (_isCube == false)
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down,
                _groundCheckDistance, layerMask);

            if (_isGrounded)
            {
                _clampingValue = _originalClampingValue;
            }
            else if (!_isGrounded)
            {
                _clampingValue = _clampingValue / 2;
            }
        }
        else if (_isCube == true)
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 
                _cubeGroundCheckDistance, layerMask);

            if (_isGrounded)
            {
                _clampingValue = _cubeClampingValue;
            }
            else if (!_isGrounded)
            {
                _clampingValue = _cubeClampingValue / 2;
            }
        }
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

        // Update NumChildren left
        //_currentNumChildren = gameObject.transform.childCount;
        
       
        

        // Movement
        Vector3 movement = new Vector3(_movementX, 0f , _movementY);        
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

        if (!_isCube)
        {
            _rb.AddForce(pushVector, ForceMode.Acceleration);

            _rb.AddTorque(rollVector, ForceMode.Acceleration);
        }
        else if (_isCube)
        {
            pushVector = pushVector * _cubePushForce;
            rollVector = rollVector * _cubeRollForce;

            _rb.AddForce(pushVector, ForceMode.Force);

            //_rb.AddTorque(rollVector, ForceMode.Force);
        }     
    }
    

    public void CutGeoByTag(string geoTag)
    {
        for (int i = 0; i < _geos.Length; i++)
        {
            GameObject geo = _geos[i];
            if (geo.CompareTag(geoTag))
            {
                geo.SetActive(false);
                _currentNumChildren -= 1;
                Debug.Log("Current NumChildren: " + _currentNumChildren);

            }
        }
    }

    public bool CheckColorPiece(string geoTag)
    {
        bool isCompatible = false;

        for (int i = 0; i < _geos.Length; i++)
        {
            GameObject geo = _geos[i];
            if (geo.CompareTag(geoTag))
            {
                if (geo.activeSelf == false)
                {
                    geo.SetActive(true);
                    isCompatible = true;
                    _currentNumChildren += 1;
                }            
            }
        }
        return isCompatible;
        
    }

   public void EnableGeoByTag(string geotag)
    {
        for (int i = 0; i < _geos.Length; i++)
        {
            GameObject geo = _geos[i];
            if (geo.CompareTag(geotag) == true && geo.activeSelf == false)
            {
                geo.SetActive(true);
                _currentNumChildren += 1;
                Debug.Log("Current NumChildren: " +  _currentNumChildren);
            }
        }
    }

    public void EnableGoldPieces()
    {
        if (_goldPieces)
        {
           _goldPieces.gameObject.SetActive(true);
            _currentNumChildren += 12;
        }
       
    }

    public bool CheckPurpleStatus()
    {
        return _isPurpleCleared;
    }
}
