using System.Collections.Generic;
using System.Linq; // Used for the Sum() function
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [System.Serializable]
    public class PlatformChance
    {
        [Tooltip("The name must match the 'tag' in the ObjectPooler.")]
        public string name;

        [Tooltip("The chance for this platform to be chosen (in %). Maximum 80%.")]
        [Range(0f, 80f)]
        public float chance;
    }

    [Header("Configuração de movimento")]
    [Tooltip("The starting speed of all platforms.")]
    public float initialSpeed = 5f;
    [Tooltip("The maximum speed the platforms can reach.")]
    public float maxSpeed = 20f;
    [Tooltip("How much the speed increases per second. This affects all new platforms.")]
    public float acceleration = 0.2f;

    [Header("Configuração de spawn")]
    [Tooltip("The time interval (in seconds) between each new platform spawn.")]
    public float spawnInterval = 2f;
    [Tooltip("The base point in space where new platforms will be spawned.")]
    public Transform spawnPoint;
    [Tooltip("The minimum and maximum random Y-axis offset added to the spawn point's position.")]
    public Vector2 spawnYOffsetRange = new Vector2(-2f, 2f);

    [Header("Tempo de vida da plataforma")]
    [Tooltip("How long (in seconds) a platform will exist before being deactivated.")]
    public float platformLifetime = 10f;

    [Header("Tipo de plataforma")]
    [Tooltip("Configure the different platform types and their chances of appearing.")]
    public List<PlatformChance> platformTypes;

    public float CurrentSpeed { get; private set; }

    private float spawnTimer;
    private ObjectPooler objectPooler;
    private float totalChance;

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        CurrentSpeed = initialSpeed;
        spawnTimer = spawnInterval;
        CalculateTotalChance();
    }

    void Update()
    {
        Accelerate();
        HandleSpawnTimer();
    }

    private void Accelerate()
    {
        if (CurrentSpeed < maxSpeed)
        {
            CurrentSpeed += acceleration * Time.deltaTime;
          //  Debug.Log(CurrentSpeed);
        }
        else
        {
            CurrentSpeed = maxSpeed;
          //  Debug.Log(CurrentSpeed);
        }


    }

    private void HandleSpawnTimer()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnPlatform();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnPlatform()
    {
        if (platformTypes == null || platformTypes.Count == 0)
        {
            Debug.LogError("The 'platformTypes' list is empty! Cannot spawn a platform.");
            return;
        }

        string platformName = ChoosePlatformByChance();
        if (string.IsNullOrEmpty(platformName)) return;

        float randomYOffset = Random.Range(spawnYOffsetRange.x, spawnYOffsetRange.y);
        Vector3 spawnPosition = spawnPoint.position + new Vector3(0, randomYOffset, 0);

        GameObject platformInstance = objectPooler.SpawnObject(platformName, spawnPosition, spawnPoint.rotation);

        if (platformInstance != null)
        {
            MovingPlatform movingPlatform = platformInstance.GetComponent<MovingPlatform>();
            if (movingPlatform != null)
            {
                movingPlatform.Initialize(CurrentSpeed, platformLifetime, platformName);
            }
            else
            {
                Debug.LogWarning($"Spawned platform '{platformName}' is missing the MovingPlatform script!");
            }
        }
    }

    private string ChoosePlatformByChance()
    {
        float randomPoint = Random.Range(0, totalChance);
        foreach (PlatformChance platform in platformTypes)
        {
            if (randomPoint <= platform.chance)
            {
                return platform.name;
            }
            else
            {
                randomPoint -= platform.chance;
            }
        }
        return null;
    }

    private void OnValidate()
    {
        CalculateTotalChance();
        if (totalChance > 100f)
        {
            Debug.LogWarning("The sum of all platform chances is " + totalChance + "%, which is greater than 100%. Please adjust the values.");
        }
    }

    private void CalculateTotalChance()
    {
        if (platformTypes != null)
        {
            totalChance = platformTypes.Sum(p => p.chance);
        }
    }
}