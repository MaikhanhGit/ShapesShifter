using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CutGeo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("CutterShell") && !other.CompareTag("Purple"))
        {
            string cutName = other.tag;
            //GameObjectUtility.DuplicateGameObject(other.gameObject);

            other.gameObject.SetActive(false);
            //disable cutter after cut
           this.gameObject.SetActive(false);
        }
        
    }
}
