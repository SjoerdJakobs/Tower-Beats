using System.Collections.Generic;
using UnityEngine;

public class TestObjectPool : MonoBehaviour {

    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;

    GameObject test;

    public List<ObjectPool> pools;

    // Use this for initialization
    void Start () {
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj1,20,5,5,20,false,PooledSubObject.Rigidbody));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj1));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj1));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj1));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj1));

        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj2));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj2));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj2));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj2));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj2));

        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj3));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj3));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj3));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj3));
        pools.Add(ObjectPoolManager.s_Instance.GetObjectPool(obj3));
    }

    public FloatRange timeBetweenSpawns, scale, randomVelocity, angularVelocity;

    public float velocity;

    public Material stuffMaterial;

    float timeSinceLastSpawn;
    float currentSpawnDelay;

    void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= currentSpawnDelay)
        {
            timeSinceLastSpawn -= currentSpawnDelay;
            currentSpawnDelay = timeBetweenSpawns.RandomInRange;
            SpawnStuff();
        }
    }

    void SpawnStuff()
    {
        int poolInt = Random.Range(0, pools.Count);

        ObjectPool pool = pools[poolInt];
        thingInPool thing = pool.GetFromPool().GetComponent<thingInPool>();
        Rigidbody R = thing.GenericObj as Rigidbody;
        //R.isKinematic = false;

        thing.transform.localPosition = transform.position;
        thing.transform.localScale = Vector3.one * scale.RandomInRange;
        thing.transform.localRotation = Random.rotation;

        thing.Body.velocity = transform.up * velocity +
            Random.onUnitSphere * randomVelocity.RandomInRange;
        thing.Body.angularVelocity =
            Random.onUnitSphere * angularVelocity.RandomInRange;

        thing.SetMaterial(stuffMaterial);

        if(poolInt < 5)
        {
            thing.StopRigidBody(.5f);
        }
    }
}

[System.Serializable]
public struct FloatRange
{

    public float min, max;

    public float RandomInRange
    {
        get
        {
            return Random.Range(min, max);
        }
    }

    public Vector3 RandomInRange3
    {
        get
        {
            Vector3 v;
            v.x = RandomInRange;
            v.y = RandomInRange;
            v.z = RandomInRange;
            return v;
        }
    }
}
