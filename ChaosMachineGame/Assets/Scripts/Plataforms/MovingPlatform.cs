using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private float speed;
    private string poolName;

    private float x_endPoint;


    private void Start()
    {
        x_endPoint=GameObject.FindWithTag("Finish").transform.position.x;
       
    }

    public void Initialize(float speed, float lifetime, string poolName)
    {
        this.speed = speed;
        this.poolName = poolName;

        CancelInvoke(nameof(Deactivate));
        Invoke(nameof(Deactivate), lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (transform.position.x <= x_endPoint)
            Deactivate();
    }

    private void Deactivate()
    {
        if (ObjectPooler.Instance != null)
        {
            ObjectPooler.Instance.ReturnObjectToPool(poolName, this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Deactivate));
    }
}