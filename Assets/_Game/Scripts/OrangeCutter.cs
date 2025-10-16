
using UnityEngine;

public class OrangeCutter : MonoBehaviour
{

    private bool _isCut = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isCut == false)
        {
            string name = other.tag;

            if (name == "Orange")
            {
                other.gameObject.SetActive(false);
                _isCut = true;
                // this.gameObject.SetActive(false);
            }


        }
    }
}
