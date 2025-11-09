using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CutGeo : MonoBehaviour
{
    [SerializeField] private GameObject _cutter = null;
    [SerializeField] private string _colorToCut = null;
    [SerializeField] private GameObject _cutterArt = null;
    private bool _isCorrectColor = false;
    private GameObject _objectToCut = null;
        
    public void Cut()
    {
        if (_isCorrectColor && _objectToCut)
        {
            Debug.Log("cut");
            Destroy(_objectToCut);

            _cutter.GetComponent<Cutter>()._isCut = true;
            _cutter.GetComponent<Cutter>().ReleaseObj();

            ResetValues();

            //disable cutter after cut
            if( _cutterArt != null)
            {
                _cutterArt.SetActive(false);
            }
            this.gameObject.SetActive(false);
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
