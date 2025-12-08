using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WinnerBase : MonoBehaviour
{
    [SerializeField] private float _yOffset = 0.5f;    
    [SerializeField] private float _releaseDirectionVertMulti = 1.5f;
    [SerializeField] private float _releaseDirectionHoriMulti = 1.2f;
    private float _movementX;
    private float _movementY;
   
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

    [Header("Visuals")]
    [SerializeField] private AudioClip _SFXFunny = null;
    [SerializeField] private ParticleSystem _particles = null;

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
            AudioHelper.PlayClip2D(_SFXFunny, 1);                        
            _entered = true;

            _otherObj = other.gameObject;
            _otherRB = other.gameObject.GetComponent<Rigidbody>();
            _ball = other.gameObject.GetComponent<Ball>();            

            if(_ball._isGoldFramed == true && _ball._currentNumChildren == 26)
            {
                _particles.Play();
            }
            //Reposition Obj
            RepositionObject();
        }

    }


    private void RepositionObject()
    {
        if (_isReleased)
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
        ResetCameraFOV();
        _isReleased = true;

        if (_otherRB)
        {
            _otherRB.isKinematic = false;
            _otherRB.WakeUp();

            if (_movementX == 0)
            {
                _movementX = 1f;
            }
            if (_movementY == 0)
            {
                _movementY = 1f;
            }

            _releaseForce = new Vector3(_movementX * _releaseDirectionHoriMulti,
                   _releaseDirectionVertMulti, _movementY * _releaseDirectionHoriMulti);

            _otherRB.AddForce(_releaseForce, ForceMode.Impulse);          

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
