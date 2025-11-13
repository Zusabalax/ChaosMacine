using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private float speed;
    private string poolName;

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