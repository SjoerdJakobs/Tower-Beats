using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject ObjectPrefab;
    public Queue<GameObject> DeadList;

    public int ObjectsInPool;
    public int ObjectsAliveInPool;

    public int CleanThreshold;
    public int IncreaseIncrement;
    public int ManagerTicksBeforeClean;
    public int CurrentTick;

    public bool isAboveThreshold;

    public void Init(int PoolStartSize = 20, int IncreaseIncrement = 5, int ManagerTicksBeforeClean = 5, int CleanThreshold = 20)
    {
        ObjectPrefab.GetComponent<PoolObj>().Pool = this;
        this.CleanThreshold = CleanThreshold;
        this.IncreaseIncrement = IncreaseIncrement;
        this.ManagerTicksBeforeClean = ManagerTicksBeforeClean;
        ObjectsAliveInPool = 0;
        DeadList = new Queue<GameObject>();
        for (int i = 0; i < PoolStartSize; i++)
        {
            GameObject NewPoolObj = Instantiate(ObjectPrefab, transform);
            NewPoolObj.GetComponent<PoolObj>().Pool = this;
            DeadList.Enqueue(NewPoolObj);
            ObjectsInPool++;
            NewPoolObj.SetActive(false);
        }
    }

    public GameObject GetFromPool()
    {
        if(ObjectsAliveInPool < ObjectsInPool)
        {
            ObjectsAliveInPool++;
            GameObject returnObject = DeadList.Dequeue();
            returnObject.SetActive(true);
            return (returnObject);
        }
        else
        {
            for (int i = 0; i < IncreaseIncrement; i++)
            {
                GameObject NewPoolObj = Instantiate(ObjectPrefab, transform);
                NewPoolObj.GetComponent<PoolObj>().Pool = this;
                DeadList.Enqueue(NewPoolObj);
                ObjectsInPool++;
                NewPoolObj.SetActive(false);
            }

            ObjectsAliveInPool++;
            GameObject returnObject = DeadList.Dequeue();
            returnObject.SetActive(true);
            return (returnObject);
        }
    }

    public void OnTick()
    {
        if (CurrentTick >= ManagerTicksBeforeClean)
        {
            if (ObjectsInPool - ObjectsAliveInPool > CleanThreshold)
            {
                for (int i = 0; i < CleanThreshold; i++)
                {
                    GameObject returnObject = DeadList.Dequeue();
                    Destroy(returnObject);
                    ObjectsInPool--;
                }
                CurrentTick = 5;
            }
            else
            {
                CurrentTick = 0;
            }
        }
        CurrentTick++;
    }

    public void AddObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        DeadList.Enqueue(obj);
        ObjectsAliveInPool--;
    }
}
