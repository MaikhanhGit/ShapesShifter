using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cutter : MonoBehaviour
{
    [SerializeField] private float _yOffset = 0.5f;
    [SerializeField] private GameObject _cutter = null;
    private float _rotateX;
    private float _rotateY;
    private Rigidbody _otherRB;
    private GameObject _otherObj;
    private bool _isCentered = false;

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
            _isCentered = true;
        }
        
       
    }

    private void OnRotate(InputValue rotateValue)
    {
        Vector2 movementVector = rotateValue.Get<Vector2>();
        _rotateX = movementVector.x;
        _rotateY = movementVector.y;

        if (_rotateX > 0)
        {            
            //Vector3 newRot = new Vector3(0f, -90f, 0f);
            //RotateObj(newRot);
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

    private void OnCut(InputValue cutValue) 
    { 
        _cutter.gameObject.SetActive(true);
    }

}
