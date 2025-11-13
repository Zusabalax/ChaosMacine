using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string nome;
        public GameObject objectPrefab;
        public Transform parent;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                Transform parentTransform = pool.parent ? pool.parent : transform;

                GameObject obj = Instantiate(pool.objectPrefab, parentTransform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.nome, objectPool);
        }
    }

    /// <summary>
    /// Spawna um objeto do pool especificado.
    /// </summary>
    /// <param name="nome">O nome do pool de onde o objeto será retirado.</param>
    /// <param name="position">A posição para spawnar o objeto.</param>
    /// <param name="rotation">A rotação para spawnar o objeto.</param>
    /// <returns>O GameObject spawnado ou null se o pool não existir ou estiver vazio.</returns>
    public GameObject SpawnObject(string nome, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(nome))
        {
            Debug.LogWarning("O Pool com o nome " + nome + " não existe.");
            return null;
        }

        if (poolDictionary[nome].Count == 0)
        {
            Debug.LogWarning("O Pool com o nome " + nome + " está vazio. Considere aumentar seu tamanho.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[nome].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    /// <summary>
    /// Retorna um objeto para o seu respectivo pool.
    /// </summary>
    /// <param name="nome">O nome do pool para onde o objeto retornará.</param>
    /// <param name="objectToReturn">A referência do GameObject a ser retornado.</param>
    public void ReturnObjectToPool(string nome, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(nome))
        {
            Debug.LogWarning("Tentando retornar objeto para um pool inexistente: " + nome);
            Destroy(objectToReturn);
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[nome].Enqueue(objectToReturn);
    }
}