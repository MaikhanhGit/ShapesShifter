using System;
using UnityEngine;


public class Detector : MonoBehaviour
{   
    private void OnTriggerEnter(Collider other)
    {
        string name = other.tag;
        Debug.Log(name);
        if(name == "Orange")
        {
            other.gameObject.SetActive(false);
        }
    }
}
