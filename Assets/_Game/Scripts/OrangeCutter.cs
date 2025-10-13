
using UnityEngine;

public class OrangeCutter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        string name = other.tag;
        Debug.Log(name);
        if (name == "Orange")
        {
            other.gameObject.SetActive(false);
        }
    }
}
