using UnityEngine;

public class PurpleCutter : MonoBehaviour
{
    [SerializeField] bool _disableAfterCut = true;
    private bool _isCut = false;

    private void OnTriggerEnter(Collider other)
    {
        if(_isCut == false)
        {            
            string name = other.tag;         

            if (name == "Purple")
            {
                other.gameObject.SetActive(false);

                _isCut = true;

                if (_disableAfterCut == true)
                {
                    this.gameObject.SetActive(false);
                }                
            }           
        }        
    }
}

