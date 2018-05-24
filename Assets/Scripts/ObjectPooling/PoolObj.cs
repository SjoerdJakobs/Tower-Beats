﻿using UnityEngine;

public class PoolObj : MonoBehaviour
{
    public PoolObjDataStruct ObjData { get; set; }
    
    public void ReturnToPool()
    {
        if (ObjData.Pool != null && this != null)
        {
            ObjData.Pool.AddObjectToPool(this);
        }
        else if(this != null)
        {
            Destroy(gameObject);
        }
    }
}

public struct PoolObjDataStruct
{
    public ObjectPool Pool;
    public GameObject GameObj;
    public Object GenericObj;

    

    public PoolObjDataStruct(ObjectPool pool = null, GameObject gameObj = null, Object genericObj = null)
    {
        Pool = pool;
        GameObj = gameObj;
        GenericObj = genericObj;
    }
}


public enum PooledSubObject : byte
{
    Default = 0,
    GameObject = 1,
    VisualEffect = 2,
    Enemy = 3,
    Rigidbody = 4
}
