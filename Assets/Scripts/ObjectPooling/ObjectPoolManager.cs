using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {

    public static ObjectPoolManager s_Instance;
    public List<ObjectPool> ObjectPools;
    public event System.Action OnTick;

    [SerializeField]
    private WaitForSeconds m_TickInterval;

    // Use this for initialization
    void Awake()
    {
        m_TickInterval = new WaitForSeconds(0.5f);
        Init();
        ObjectPools = new List<ObjectPool>();
        StartCoroutine(Ticker());
    }

    //make instance
    private void Init()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ObjectPool GetObjectPool(GameObject Prefab)
    {
        if(Prefab.GetComponent<PoolObj>() == null)
        {
            Debug.LogError("POOLOBJECT COMPONENT NOT FOUND ON PREFAB");
            return null;
        }
        else
        {
            for (int i = 0; i < ObjectPools.Count; i++)
            {
                if(ObjectPools[i].ObjectPrefab == Prefab)
                {
                    return ObjectPools[i];
                }
            }

            GameObject newPoolObj = new GameObject(Prefab.name + " pool");
            newPoolObj.AddComponent<ObjectPool>();

            ObjectPool newPool = newPoolObj.GetComponent<ObjectPool>();
            newPool.ObjectPrefab = Prefab;
            OnTick += newPool.OnTick;
            newPool.Init();
            ObjectPools.Add(newPool);

            return newPool;
        }        
    }

    private IEnumerator Ticker()
    {
        while(true)
        {
            if (OnTick != null)
            {
                OnTick();
            }
            yield return m_TickInterval;
        }        
    }
}
