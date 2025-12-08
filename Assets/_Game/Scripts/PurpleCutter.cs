using UnityEngine;

public class PurpleCutter : MonoBehaviour
{
    [SerializeField] private bool _disableAfterCut = true;
    [SerializeField] private bool _keepCutting = false;
    [SerializeField] private GameObject _ball = null;
    [SerializeField] private AudioClip _purpleCut = null;
    [SerializeField] ParticleSystem _particles = null;
    private bool _isCut = false;


    
    private void OnTriggerEnter(Collider other)
    {       
        if(_isCut == false)
        {            
            string name = other.tag;         

            if (name == "Purple")
            {
                AudioHelper.PlayClip2D(_purpleCut, 0.7f);
                _particles.Play();
                _ball.gameObject.GetComponent<Ball>().CutGeoByTag("Purple");
                //_ball.gameObject.GetComponent<Ball>()._isPurpleCleared = true;

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

