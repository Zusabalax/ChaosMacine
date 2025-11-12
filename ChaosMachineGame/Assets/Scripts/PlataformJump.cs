using UnityEngine;

public class PlataformJump : MonoBehaviour

{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
           PlayerControl.Jump = true;
    }
    

}
