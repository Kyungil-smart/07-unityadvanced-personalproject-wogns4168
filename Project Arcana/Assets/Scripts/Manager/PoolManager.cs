using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    
    private Dictionary<GameObject, Queue<GameObject>> _pools = new Dictionary<GameObject, Queue<GameObject>>();
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    public void CreatePool(GameObject prefab, int size)
    {
        if (_pools.ContainsKey(prefab)) return;
        Queue<GameObject> objectPool = new Queue<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        _pools.Add(prefab, objectPool);
    }

    public GameObject Spawn(GameObject prefab)
    {
        if (!_pools.ContainsKey(prefab)) CreatePool(prefab, 1);

        GameObject obj = _pools[prefab].Count > 0 ? _pools[prefab].Dequeue() : Instantiate(prefab, transform);
        
        if (obj.TryGetComponent<IPoolable>(out var poolable)) poolable.OnSpawn();
        return obj;
    }

    public void Despawn(GameObject obj, GameObject prefab)
    {
        if (obj.TryGetComponent<IPoolable>(out var poolable)) poolable.OnDespawn();

        if (!_pools.ContainsKey(prefab)) _pools.Add(prefab, new Queue<GameObject>());
        _pools[prefab].Enqueue(obj);
    }
}