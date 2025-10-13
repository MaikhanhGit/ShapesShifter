using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutGeo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("CutterShell"))
        {
            string cutName = other.tag;
            other.gameObject.SetActive(false);
        }
        
    }
}
