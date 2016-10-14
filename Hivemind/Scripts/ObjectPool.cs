using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;
    public bool allowGrow = false;

    List<GameObject> objectPool;
    GameObject poolGroup;

    bool isPoolCreated = false;

    public static ObjectPool current;

    void Awake()
    {
        current = this;
    }

    void Start()
    {

    }

    /// <summary>
    /// Initializes the pool.
    /// </summary>
    /// <param name="prefab"></param>
    public void InitializePool(GameObject prefab, int poolSize = 10)
    {
        this.poolSize = poolSize;
        this.prefab = prefab;

        poolGroup = GameObject.Find("ObjectPool");

        objectPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.transform.parent = poolGroup.transform;
            obj.SetActive(false);
            objectPool.Add(obj);
        }

        isPoolCreated = true;

    }

    /// <summary>
    /// Spawns an object from pool.
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (objectPool == null && !isPoolCreated)
        {
            InitializePool(prefab);
        }

        GameObject objectToSpawn = null;

        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                objectToSpawn = objectPool[i];
                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;
            }
        }

        if (allowGrow && objectToSpawn == null)
        {
            objectToSpawn = (GameObject)Instantiate(prefab, position, rotation);
            objectToSpawn.transform.parent = poolGroup.transform;
            objectPool.Add(objectToSpawn);
        }

        if (objectToSpawn != null) objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    /// <summary>
    /// Despawns an object to pool.
    /// </summary>
    /// <param name="obj"></param>
    public void Despawn(GameObject obj)
    {
        objectPool.Add(obj);
        obj.SetActive(false);
    }

    /// <summary>
    /// Clears pool.
    /// </summary>
    public void ClearPool()
    {

        for (int i = objectPool.Count - 1; i > 0; i--)
        {
            GameObject obj = objectPool[i];
            objectPool.RemoveAt(i);
            Destroy(obj);
        }
        objectPool = null;

        isPoolCreated = false;

    }

    /// <summary>
    /// Checks for existing pool.
    /// </summary>
    /// <returns></returns>
    public bool IsPoolCreated()
    {
        return isPoolCreated;
    }
}
