using UnityEngine;

public class RealocatePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private Transform  _playerPosition;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
               collision.gameObject.transform.position = _playerPosition.position;
    }
}
