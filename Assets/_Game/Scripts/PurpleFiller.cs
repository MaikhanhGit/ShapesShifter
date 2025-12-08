using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PurpleFiller : MonoBehaviour
{
    [SerializeField] private GameObject _rotatingArt = null;
    [SerializeField] private GameObject _artToDisable = null;
    [SerializeField] Vector3 _rotationVector = new Vector3(1f, 1f, 1f);
    [SerializeField] private float _rotationSpeed = 0.2f;
    [SerializeField] AudioClip _SFXPurpleFill = null;
    [SerializeField] ParticleSystem _particles = null;
    private bool _isUsed = false;
   
    private void FixedUpdate()
    {
        if (_rotatingArt)
        {
            _rotatingArt.gameObject.transform.Rotate(_rotationVector * 
                _rotationSpeed * Time.fixedDeltaTime);
        }
    }   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Ball player = other.gameObject.GetComponent<Ball>();
            _artToDisable.gameObject.SetActive(false);

            if (player._isPurpleCleared == true && player._isGoldFramed == false 
                && _isUsed == false)
            {                
                _isUsed = true;
                player.EnableGeoByTag("Purple");
                AudioHelper.PlayClip2D(_SFXPurpleFill, 25f);
                _particles.Play();
               // player._isPurpleCleared = false;                
            }            
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(_isUsed == false)
            {
                _artToDisable.gameObject.SetActive(true);
            }
        }
    }
}
