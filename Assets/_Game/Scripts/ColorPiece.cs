using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorPiece : MonoBehaviour
{
    //[SerializeField] GameObject _art = null;    
    [SerializeField] Vector3 _rotationVector = new Vector3(1f, 1f, 1f);
    [SerializeField] float _rotationSpeed = 1f;
    [SerializeField] AudioClip _SFXFilled = null;
    [SerializeField] ParticleSystem _particles = null;


    private void FixedUpdate()
    {
        //Rigidbody rb = _art.GetComponent<Rigidbody>();
        gameObject.transform.Rotate(_rotationVector * _rotationSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool isCompatible;

            isCompatible = other.gameObject.GetComponent<Ball>().CheckColorPiece(gameObject.tag);

            if (isCompatible == true)
            {
                AudioHelper.PlayClip2D(_SFXFilled, 0.35f);
                _particles.Play();
                DelayHelper.DelayAction(this, DisableThisObject, 0.3f);
            }
        }
            
    }

    private void DisableThisObject()
    {
        Destroy(gameObject);
    }
}
