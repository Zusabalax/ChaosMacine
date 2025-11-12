using DG.Tweening;
using UnityEngine;

public class PlataformJump : MonoBehaviour

{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
           PlayerControl.Jump = true;
    }

    private void Update()
    {
        this.transform.DOMove(Vector2.left,Time.deltaTime);
    }


}
