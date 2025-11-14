using DG.Tweening;

using UnityEngine;

public class PlataformJump : MonoBehaviour
{
    [SerializeField]
    private float speed;
   


    private void Update()
    {
       this.transform.Translate(-speed*Time.deltaTime,0,0);    
    }
}
