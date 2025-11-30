using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleFiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Ball player = other.gameObject.GetComponent<Ball>();
            if (player._isPurpleCleared == true)
            {
                player.EnableGeoByTag("Purple");
                player._isPurpleCleared = false;
            }
            
        }
    }
}
