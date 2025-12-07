using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sucking : MonoBehaviour
{
    [SerializeField] float _yOffset = 0.2f;
    [SerializeField] float _objReleaseForce = 250f;
    [SerializeField] float _topPlatformReleaseForce = -1f;
    [SerializeField] float _topPlatfReleaseHoriMul = -500f;
    [SerializeField] Vector3 _rotationVector = new Vector3 (1f, 1f, 1f);
    [SerializeField] float _rotationSpeed = 1f;
    [SerializeField] bool _isTopPlatform = false;
    private GameObject _otherObj = null;
    private Rigidbody _otherRB = null;
    private bool _isCentered = false;
    private bool _isReset = false;
    private bool _isHolding = false;
    private Vector3 _releaseForce = new Vector3(1f, 1f, 1f);
    private float _movementX;
    private float _movementY;


    private void Start()
    {
        if (_isTopPlatform == true)
        {
            _yOffset *= (-1f);
            //_objReleaseForce *= (-1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !_isReset)
        {          
            _otherObj = other.gameObject;
            _otherRB = other.GetComponent<Rigidbody>();

            StartPositioning();
        }
        
    }
    private void StartPositioning()
    {
        if (!_isCentered)
        {
            _otherRB.isKinematic = true;
            _otherRB.WakeUp();
            
            _otherObj.transform.rotation = Quaternion.identity;            

            _isCentered = true;
            _isHolding = true;
        }
       
    }

    private void FixedUpdate()
    {        
        if (_otherRB && _isCentered && _isHolding)
        {
            _otherRB.transform.Rotate(_rotationVector * _rotationSpeed * Time.fixedDeltaTime);

            Vector3 newPos = new Vector3(this.transform.position.x,
                   this.transform.position.y + _yOffset, this.transform.position.z);

            _otherObj.transform.position = newPos;
        }
       
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        _movementX = movementVector.x;
        _movementY = movementVector.y;
    }

    private void OnReleaseObj()
    {       
        if (_isCentered)
        {
            _isHolding = false;

            if (!_isTopPlatform)
            {
                _releaseForce = new Vector3(_movementX * _objReleaseForce,
                _objReleaseForce * 2, _movementY * _objReleaseForce);
            }
            else if (_isTopPlatform)
            {                
                _releaseForce = new Vector3(_movementX * _topPlatformReleaseForce * _topPlatfReleaseHoriMul,
               -0.1f, _movementY * _topPlatformReleaseForce * _topPlatfReleaseHoriMul);
                Debug.Log(_releaseForce);
            }
            

            ReleaseObj();

           
        }

    }

    private void ReleaseObj()
    {             
        _otherRB.isKinematic = false;
        _otherRB.WakeUp();

        _otherRB.AddForce(_releaseForce);

        ResetValues();
    }

    private void ResetValues()
    {
        _isReset = true;

        DelayHelper.DelayAction(this, SetReset, 0.5f);
    }

    private void SetReset()
    {
        _isReset = false;
        _isCentered = false;
        _isHolding = false;
        _otherObj = null;
        _otherRB = null;       
    }
}
