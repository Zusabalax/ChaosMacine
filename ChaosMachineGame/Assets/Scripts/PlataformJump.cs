using DG.Tweening;

using UnityEngine;

public class PlataformJump : MonoBehaviour
{
    [SerializeField]
    private float _endValue,_duration;
    private void OnCollisionEnter2D(Collision2D collision)
    {
     
        
        if (collision.gameObject.tag == "Player")
        {
            PlayerControl.Jump = true;
           
           

        }
          
    }


    private void Update()
    {
       this.transform.DOMoveX(_endValue, _duration);    
    }
}
