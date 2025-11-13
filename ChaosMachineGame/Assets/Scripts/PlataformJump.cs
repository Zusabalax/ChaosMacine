using DG.Tweening;

using UnityEngine;

public class PlataformJump : MonoBehaviour
{
    [SerializeField]
    private float _endValue,_duration;
   


    private void Update()
    {
       this.transform.DOMoveX(_endValue, _duration);    
    }
}
