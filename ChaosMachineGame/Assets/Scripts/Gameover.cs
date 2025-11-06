using Unity.VisualScripting;
using UnityEngine;

public class Gameover : MonoBehaviour
{
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.SetActive(false);
    }
}
