using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPiece : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bool isCompatible = false;

            isCompatible = other.gameObject.GetComponent<Ball>().CheckColorPiece(gameObject.tag);

            if (isCompatible == true)
            {
                DelayHelper.DelayAction(this, DisableThisObject, 0.2f);
            }
        }
            
    }

    private void DisableThisObject()
    {
        Destroy(gameObject);
    }
}
