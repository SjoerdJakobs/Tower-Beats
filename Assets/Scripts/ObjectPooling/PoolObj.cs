using UnityEngine;

public class PoolObj : MonoBehaviour
{
    public ObjectPool Pool { get; set; }

    public void ReturnToPool()
    {
        if (Pool != null)
        {
            Pool.AddObjectToPool(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
