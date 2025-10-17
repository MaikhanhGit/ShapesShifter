using UnityEngine;
using UnityEngine.InputSystem;

public class Cutter : MonoBehaviour
{
    [SerializeField] private float _yOffset = 0.5f;
    [SerializeField] private GameObject _cutter = null;
    [SerializeField] private float _objReleaseForce = 100f;
    
    private float _rotateX = 0f;
    private float _rotateY = 0f;
    private Rigidbody _otherRB = null;
    private GameObject _otherObj = null;
    private Vector3 _releaseForce = Vector3.zero;
    private bool _isCentered = false;
    private bool _isCut = false;
    private bool _isTurnable = false;

    private void Start()
    {
        _releaseForce = new Vector3(_objReleaseForce / 2, _objReleaseForce, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        _otherRB = other.GetComponentInParent<Rigidbody>();
        _otherObj = _otherRB.gameObject;
        //Reposition Obj          
        RepositionObject();
        //Check Color

        // Cut
    }

    private void RepositionObject()
    {
        if (!_isCentered) 
        {
            _otherRB.isKinematic = true;           

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
            if (_isTurnable && _isCentered == true)
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
        if(_otherRB)
        {
            if (!_isCut)
            {                
                _cutter.gameObject.SetActive(true);
                _isCut = true;
                ReleaseObj();               
            }
        }              
    }

    private void OnReleaseObj()
    {
        if (_otherRB)
        {           
            ReleaseObj();
        }
      
    }

    private void ReleaseObj()
    {        
        _otherRB.isKinematic = false;
        _otherRB.AddForce(_releaseForce);       
        
        _isCentered = false;
        _isCut = false;
        _isTurnable = false;
        _otherObj = null;
        _otherRB = null;
 
    }

}
