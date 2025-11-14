using System.Collections;
using UnityEngine;

public class BulletAtack : MonoBehaviour
{
    [SerializeField]
    private float speed,time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    IEnumerator TimeBullet()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
