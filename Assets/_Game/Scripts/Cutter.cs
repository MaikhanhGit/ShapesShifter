using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cutter : MonoBehaviour
{
    [SerializeField] private float _yOffset = 0.5f;
    [SerializeField] private GameObject[] _cutGeo = null;  
    [SerializeField] private float _aftercutReleaseMulti = 10f;
    [SerializeField] private float _releaseDirectionVertMulti = 1.5f;
    [SerializeField] private float _releaseDirectionHoriMulti = 1.2f;
    private float _movementX;
    private float _movementY;    
    private float _rotateX = 0f;
    private float _rotateY = 0f;
    private int _currentNumChildren = 0;
    private Rigidbody _otherRB = null;
    private GameObject _otherObj = null;
    private Ball _ball = null;
    private Vector3 _releaseForce = new Vector3(1f, 1f, 1f);
    
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
    private float _defaultCameraOffsetY = 0.0f;
    private float _defaultVerDamp = 0;
    private float _defaultHorDamp = 0;
    private CinemachineVirtualCamera _camera = null;
    private CinemachineTransposer _camTransposer = null;
    private CinemachineComposer _camComposer = null;

    [Header("Audio")]
    [SerializeField] AudioClip _SFXRotate = null;
    [SerializeField] AudioClip _SFXrepel = null;
    [SerializeField] AudioClip _SFXsuck = null;
    private void Start()
    {
       
        if (_virtualCamera)
        {            
            _camera = _virtualCamera.GetComponent<CinemachineVirtualCamera>();
            _camTransposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
            _camComposer = _camera.GetCinemachineComponent<CinemachineComposer>();

            if (_camera)
            {                
                _defaultFOV = _camera.m_Lens.FieldOfView;              
            }
            
            if (_camTransposer)
            {               
                _defaultCameraOffsetY = _camTransposer.m_FollowOffset.y;               
            }

            if (_camComposer)
            {
                _defaultVerDamp = _camComposer.m_VerticalDamping;
                _defaultHorDamp = _camComposer.m_HorizontalDamping;
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
        AudioHelper.PlayClip2D(_SFXsuck, 0.2f);

        if (_isCut || _isReleased)
        {
            return;
        }

        if (!_isCentered && !_isReleased && _otherRB) 
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
                AudioHelper.PlayClip2D(_SFXRotate, 0.3f);

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
                _cutGeo[i].GetComponent<CutGeo>().Cut();
            }
            
        }
    }    

    private void OnReleaseObj()
    {
        if (_isCentered)
        {
            if (_isReleased == false)
            {
                if (_movementX <= 0)
                {
                    //_movementX = _objReleaseForce;

                }
                if (_movementY <= 0)
                {
                    //_movementY = _objReleaseForce;
                }                

                _isReleaseHit = true;

                ReleaseObj();                
            }
        }
        
        
    }

    public void ReleaseObj()
    {
        AudioHelper.PlayClip2D(_SFXrepel, 1);

        ResetCameraFOV();
        _isReleased = true;        

        if (_otherRB)
        {            
            _otherRB.isKinematic = false;
            _otherRB.WakeUp();

            if(_movementX == 0)
            {
                _movementX = 1f;
            }
            if (_movementY == 0)
            {
                _movementY = 1f;
            }

            if(_isCut == false)
            {
                _releaseForce = new Vector3(_movementX * _releaseDirectionHoriMulti,
                   _releaseDirectionVertMulti, _movementY * _releaseDirectionHoriMulti);
                
                _otherRB.AddForce(_releaseForce, ForceMode.Impulse);
            }
           

            if (_isCut == true)
            {
                _releaseForce = new Vector3(_movementX * _releaseDirectionHoriMulti * _aftercutReleaseMulti,
                  _releaseDirectionVertMulti * _aftercutReleaseMulti *10,
                  _movementY * _releaseDirectionHoriMulti * _aftercutReleaseMulti);
                
                _otherRB.AddForce(_releaseForce, ForceMode.Impulse);

                DelayHelper.DelayAction(this, DestroyThis, 0.5f);                
            }

            ResetValues();
        }       
          
    }

    public void ResetValues()
    {        
        
        _isReset = true;        

        DelayHelper.DelayAction(this, SetReset, 0.2f);
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
       
        if (_camTransposer)
        {            
            _camTransposer.m_FollowOffset.y = _focusCameraY;
            
        }

        if (_camComposer)
        {           
            _camComposer.m_VerticalDamping = 0.0f;

            _camComposer.m_HorizontalDamping = 0.0f;
        }

        if (_camera)
        {
            _camera.m_Lens.FieldOfView = _focusFOV;            
        }

        
    }

    private void ResetCameraFOV()
    {      
        if (_camTransposer)
        {
            _camTransposer.m_FollowOffset.y = _defaultCameraOffsetY;
        }

        if (_camComposer)
        {
            _camComposer.m_VerticalDamping = _defaultVerDamp;

            _camComposer.m_HorizontalDamping = _defaultHorDamp;
        }

        if (_camera)
        {
            _camera.m_Lens.FieldOfView = _defaultFOV;
        }
    }

}
