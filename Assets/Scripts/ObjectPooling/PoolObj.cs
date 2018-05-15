using UnityEngine;

public class PoolObj : MonoBehaviour
{
    public ObjectPool Pool { get; set; }

    public void ReturnToPool()
    {
        if (Pool != null && this != null)
        {
            Pool.AddObjectToPool(this.gameObject);
        }
        else if(this != null)
        {
            Destroy(gameObject);
        }
    }
}
