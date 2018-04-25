using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject ObjectPrefab { get; set; }

    private Queue<GameObject> m_DeadList;

    private int m_ObjectsInPool;
    private int m_ObjectsAliveInPool;

    private int m_CleanThreshold;
    private int m_IncreaseIncrement;
    private int m_anagerTicksBeforeClean;
    private int m_CurrentTick;

    private bool m_isAboveThreshold;

    public void Init(int PoolStartSize = 20, int IncreaseIncrement = 5, int ManagerTicksBeforeClean = 5, int CleanThreshold = 20)
    {
        ObjectPrefab.GetComponent<PoolObj>().Pool = this;
        this.m_CleanThreshold = CleanThreshold;
        this.m_IncreaseIncrement = IncreaseIncrement;
        this.m_anagerTicksBeforeClean = ManagerTicksBeforeClean;
        m_ObjectsAliveInPool = 0;
        m_DeadList = new Queue<GameObject>();
        for (int i = 0; i < PoolStartSize; i++)
        {
            GameObject NewPoolObj = Instantiate(ObjectPrefab, transform);
            NewPoolObj.GetComponent<PoolObj>().Pool = this;
            m_DeadList.Enqueue(NewPoolObj);
            m_ObjectsInPool++;
            NewPoolObj.SetActive(false);
        }
    }

    public GameObject GetFromPool()
    {
        if(m_ObjectsAliveInPool < m_ObjectsInPool)
        {
            m_ObjectsAliveInPool++;
            GameObject returnObject = m_DeadList.Dequeue();
            returnObject.SetActive(true);
            return (returnObject);
        }
        else
        {
            for (int i = 0; i < m_IncreaseIncrement; i++)
            {
                GameObject NewPoolObj = Instantiate(ObjectPrefab, transform);
                NewPoolObj.GetComponent<PoolObj>().Pool = this;
                m_DeadList.Enqueue(NewPoolObj);
                m_ObjectsInPool++;
                NewPoolObj.SetActive(false);
            }

            m_ObjectsAliveInPool++;
            GameObject returnObject = m_DeadList.Dequeue();
            returnObject.SetActive(true);
            return (returnObject);
        }
    }

    public void OnTick()
    {
        if (m_CurrentTick >= m_anagerTicksBeforeClean)
        {
            if (m_ObjectsInPool - m_ObjectsAliveInPool > m_CleanThreshold)
            {
                for (int i = 0; i < m_CleanThreshold; i++)
                {
                    GameObject returnObject = m_DeadList.Dequeue();
                    Destroy(returnObject);
                    m_ObjectsInPool--;
                }
                m_CurrentTick = 5;
            }
            else
            {
                m_CurrentTick = 0;
            }
        }
        m_CurrentTick++;
    }

    public void AddObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
        m_DeadList.Enqueue(obj);
        m_ObjectsAliveInPool--;
    }
}
