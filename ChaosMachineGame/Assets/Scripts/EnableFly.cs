using UnityEngine;

public class EnableFly : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            StateMachine.Instance.TransitionToFly();
    }
}
