using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cutter : MonoBehaviour
{
    [SerializeField] private float _yOffset = 0.5f;
    [SerializeField] private GameObject _cutter = null;
    [SerializeField] private float _objReleaseForce = 100f;    
    
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
    public bool _isCut = false;
    

    private void Start()
    {
        _releaseForce = new Vector3(_objReleaseForce / 2, _objReleaseForce, 0f);
    }

    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(_isReset);
        if (other.gameObject.CompareTag("Player") && !_isReset && !_entered)
        {
            Debug.Log("Enter");            
            _entered = true;
            _otherRB = other.GetComponentInParent<Rigidbody>();
            _otherObj = other.gameObject;
            _ball = _otherObj.GetComponent<Ball>();

            if (_ball != null)
            {
                _currentNumChildren = _ball._currentNumChildren;
            }           

            //Reposition Obj
            RepositionObject();
        }              

    }

    private void OnTriggerExit(Collider other)
    {        
       if(_entered)
        {
            Debug.Log("Returning");
            
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
            Debug.Log("reposition");
            
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

    private void OnRotate(InputValue rotateValue)
    {        
       if(_otherObj && _otherRB)
        {
            Debug.Log("Turning");
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
            _cutter.GetComponent<CutGeo>().Cut();
        }
    }    

    private void OnReleaseObj()
    {
        if (_isCentered)
        {            
            ReleaseObj();            
        }
      
    }

    public void ReleaseObj()
    {
        if(!_isCut)
        {
            Debug.Log("Release Not Cut");
            _otherRB.isKinematic = false;
            _otherRB.WakeUp();
            _otherRB.AddForce(_releaseForce);

            ResetValues();                        
        }
        else if(_isCut)
        {
            Debug.Log("Release");
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

    public void ResetValues()
    {
        
        
        _isReset = true;
        

        DelayHelper.DelayAction(this, SetReset, 0.5f);
    }

    private void SetReset()
    {
        Debug.Log("Reset");
        _isReset = false;
        _isCut = false;
        _isReleased = false;
        _isCentered = false;
        _isTurnable = false;
        _entered = false;
        _otherObj = null;
        _otherRB = null;
        _ball = null;
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
