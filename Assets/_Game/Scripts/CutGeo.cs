using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CutGeo : MonoBehaviour
{
    [SerializeField] private GameObject _cutter = null;
    [SerializeField] private string _colorToCut = null;
    [SerializeField] private GameObject _ball = null;
    [SerializeField] AudioClip _SFXcutting = null;
    [SerializeField] AudioClip _SFXInvalid = null;
    [SerializeField] private ParticleSystem _cutParticles;

    private bool _isCorrectColor = false;
    private GameObject _objectToCut = null;
    private bool _disableWhenDone = true;
        
    public void Cut()
    {
        if (_isCorrectColor && _objectToCut)
        {
            //Destroy(_objectToCut);
            AudioHelper.PlayClip2D(_SFXcutting, 0.5f);
            if( _cutParticles != null)
            {
                _cutParticles.Play();
            }            

            _ball.gameObject.GetComponent<Ball>().CutGeoByTag(_objectToCut.tag);
            
            _cutter.GetComponent<Cutter>()._isCut = true;
            _cutter.GetComponent<Cutter>().ReleaseObj();           
          
            ResetValues();
           
        }
        else
        {
            AudioHelper.PlayClip2D(_SFXInvalid, 1);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_colorToCut))
        {            
            _isCorrectColor = true;
            _objectToCut = other.gameObject;            
        }
        else
        {
            ResetValues();
        }
    }

    private void OnTriggerExit(Collider other)
    {        
        ResetValues();
    }

    private void ResetValues()
    {
        _isCorrectColor = false;
        _objectToCut = null;
    }

   
}
