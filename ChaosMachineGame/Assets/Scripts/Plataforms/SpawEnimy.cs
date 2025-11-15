using System.Collections;
using UnityEngine;

public class SpawEnimy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private Transform spownPoint;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float spawnInterval = 3f;

    [SerializeField]
    private float randon;

    private Vector3 randomVector;

    private void Start()
    {
        StartCoroutine(SpwnEnimyu());
    }

    IEnumerator SpwnEnimyu()
            {
        while (true)
        {
            randomVector = new Vector3(Random.Range(-randon, randon), 0, 0);
            yield return new WaitForSeconds(spawnInterval);
            Instantiate(enemyPrefab, spownPoint.position+ randomVector, Quaternion.identity);
        }
    }
}
