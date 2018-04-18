using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public List<PoolObj> Pool;

    public ObjectPool(PoolObj poolObj)
    {
        Pool = new List<PoolObj>();
    }
}

public class PoolObj
{

}
