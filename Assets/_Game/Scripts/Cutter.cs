using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cutter : MonoBehaviour
{
    [SerializeField] private float _yOffset = 0.5f;
    [SerializeField] private GameObject[] _cutGeo = null;
    [SerializeField] private float _objReleaseForce = 100f;

    private float _movementX;
    private float _movementY;    
    private float _rotateX = 0f;
    private float _rotateY = 0f;
    private int _currentNumChildren = 0;
    private Rigidbody _otherRB = null;
    private GameObject _otherObj = null;
    private Ball _ball = null;
    private Vector3 _releaseForce = Vector3.zero;
    private bool _isCentered = false;    
    private bool _isTurnable = false;
    private bool _isReleased = false;
    private bool _entered = false;
    private bool _isReset = false;
    private bool _isReleaseHit = false;
    public bool _isCut = false;

    [Header("Visuals")]
    [SerializeField] private GameObject _virtualCamera = null;
    [SerializeField] private float _focusFOV = 13f;
    [SerializeField] private float _focusCameraY = 0.5f;
    private float _defaultFOV = 0f;
    private float _defaultCameraY = 0f;
    private CinemachineVirtualCamera _camera = null;
    private CinemachineTransposer _transposer = null;

    private void Start()
    {
        

        if (_virtualCamera)
        {            
            _camera = _virtualCamera.GetComponent<CinemachineVirtualCamera>();
            if (_camera)
            {                
                _defaultFOV = _camera.m_Lens.FieldOfView;              
            }
            
            if (_virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Follow.
                GetComponent<CinemachineTransposer>())
            {
                _defaultCameraY = _virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Follow.
                    GetComponent<CinemachineTransposer>().m_FollowOffset.y;               
            }            
            
        }
    }

    

    private void OnTriggerEnter(Collider other)
    {       
        if (other.gameObject.CompareTag("Player") && !_isReset && !_entered)
        {            
            _entered = true;
            
            _otherObj = other.gameObject;
            _otherRB = other.gameObject.GetComponent<Rigidbody>();
            _ball = other.gameObject.GetComponent<Ball>();

            if (_ball != null)
            {
                _currentNumChildren = _ball._currentNumChildren;
            }           

            //Reposition Obj
            RepositionObject();
        }              

    }
        

    private void RepositionObject()
    {
        if (_isCut || _isReleased)
        {
            return;
        }

        if (!_isCentered && !_isReleased) 
        {
            ZoomCameraIn();
            
            _otherRB.isKinematic = true;
            _otherRB.WakeUp();

            Vector3 newPos = new Vector3(this.transform.position.x,
                this.transform.position.y + _yOffset, this.transform.position.z);

            _otherObj.transform.position = newPos;
            _otherObj.transform.rotation = Quaternion.identity;
            
            _isTurnable = true;
            _isCentered = true;
        }               
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();        
        _movementX = movementVector.x;
        _movementY = movementVector.y;
    }

    private void OnRotate(InputValue rotateValue)
    {        
       if(_otherObj && _otherRB)
        {            
            if (_isTurnable)
            {
                Vector2 movementVector = rotateValue.Get<Vector2>();
                _rotateX = movementVector.x;
                _rotateY = movementVector.y;

                if (_rotateX > 0)
                {
                    _otherObj.transform.Rotate(Vector3.down, 90f, Space.World);

                }
                if (_rotateX < 0)
                {
                    _otherObj.transform.Rotate(Vector3.down, -90f, Space.World);
                }
                if (_rotateY > 0)
                {
                    _otherObj.transform.Rotate(Vector3.right, 90f, Space.World);
                }
                if (_rotateY < 0)
                {
                    _otherObj.transform.Rotate(Vector3.right, -90f, Space.World);
                }
            }            
        }       
    }

    private void OnCut() 
    {
        //Make sure player can only cut Once
        if(!_isCut)
        {
            for (int i = 0; i < _cutGeo.Length; i++)
            {
                Debug.Log(_cutGeo.Length);
                _cutGeo[i].GetComponent<CutGeo>().Cut();
            }
            //_cutGeo.GetComponent<CutGeo>().Cut();
        }
    }    

    private void OnReleaseObj()
    {
        if (_movementX <= 0)
        {
            _movementX = _objReleaseForce;

        }
        if (_movementY <= 0)
        {
            _movementY = _objReleaseForce;
        }

        _releaseForce = new Vector3(_movementX * _objReleaseForce * 3, _objReleaseForce, _movementY * _objReleaseForce);
        _isReleaseHit = true;
        ReleaseObj();
    }

    public void ReleaseObj()
    {
        ResetCameraFOV();

        if (!_isReleaseHit)
        {
            
            _releaseForce = new Vector3(_objReleaseForce * 120, _objReleaseForce * 110, _objReleaseForce * 110);
            Debug.Log("Cut Pushed: " + _releaseForce);

            if (!_isCut)
            {
                _otherRB.isKinematic = false;
                _otherRB.WakeUp();
                _otherRB.AddForce(_releaseForce);

                ResetValues();
            }
            else if (_isCut)
            {
                if (_otherRB)
                {
                    _otherRB.isKinematic = false;
                    //_otherRB.WakeUp();
                    _otherRB.AddForce(_releaseForce);

                    // TODO: add Disabled Visuals
                    gameObject.GetComponent<Cutter>().enabled = false;

                    DelayHelper.DelayAction(this, DestroyThis, 0.5f);
                }
            }
        }
        else if (_isReleaseHit)
        {
            Debug.Log("Pushed" + _releaseForce);
            if (!_isCut)
            {
                _otherRB.isKinematic = false;
                _otherRB.WakeUp();
                _otherRB.AddForce(_releaseForce);

                ResetValues();
            }
            else if (_isCut)
            {
                if (_otherRB)
                {
                    _otherRB.isKinematic = false;
                    //_otherRB.WakeUp();
                    _otherRB.AddForce(_releaseForce);

                    // TODO: add Disabled Visuals
                    gameObject.GetComponent<Cutter>().enabled = false;

                    DelayHelper.DelayAction(this, DestroyThis, 0.2f);
                }
            }        
            
        }
          
    }

    public void ResetValues()
    {        
        
        _isReset = true;        

        DelayHelper.DelayAction(this, SetReset, 0.5f);
    }

    private void SetReset()
    {        
        _isReset = false;
        _isCut = false;
        _isReleased = false;
        _isCentered = false;
        _isTurnable = false;
        _entered = false;
        _isReleaseHit = false;
        _otherObj = null;
        _otherRB = null;
        _ball = null;
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void ZoomCameraIn()
    {
        if (_camera)
        {
            _camera.m_Lens.FieldOfView = _focusFOV;            
        }
        if (_virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Follow.
            GetComponent<CinemachineTransposer>())
        {
            _virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Follow.
                GetComponent<CinemachineTransposer>().m_FollowOffset.y = _focusCameraY;
        }
    }

    private void ResetCameraFOV()
    {
        if (_camera)
        {
            _camera.m_Lens.FieldOfView = _defaultFOV;            
        }
        if (_virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Follow.
            GetComponent<CinemachineTransposer>())
        {
            _virtualCamera.GetComponent<CinemachineVirtualCamera>().m_Follow.
                GetComponent<CinemachineTransposer>().m_FollowOffset.y = _defaultCameraY;
        }
    }

}
