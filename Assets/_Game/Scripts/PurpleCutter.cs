using UnityEngine;

public class PurpleCutter : MonoBehaviour
{
    [SerializeField] private bool _disableAfterCut = true;
    [SerializeField] private bool _keepCutting = false;
    private bool _isCut = false;
    private int _currentChildCount = 0;
    private Ball _ball = null;

    private void OnTriggerEnter(Collider other)
    {        
        if(_isCut == false)
        {            
            string name = other.tag;         

            if (name == "Purple")
            {
                //other.gameObject.SetActive(false);
                Destroy(other.gameObject);

                if (_keepCutting == false)
                {
                    _isCut = true;
                }                

                if (_disableAfterCut == true)
                {
                    this.gameObject.SetActive(false);
                }                
            }           
        }        
    }
}

