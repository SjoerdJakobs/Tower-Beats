using UnityEngine;

public class PoolObj : MonoBehaviour
{
    public ObjectPool Pool;
    public Object GenericObj;
    //public PoolObjDataStruct ObjData { get; set; }
    
    public void ReturnToPool()
    {
        if (Pool != null && this != null)
        {
            if (GenericObj is Enemy)
            {
                Enemy testEnemy = GenericObj as Enemy;
                Debug.Log("return to pool" + testEnemy.IsAlive);
            }
            Pool.AddObjectToPool(this);
        }
        else if(this != null)
        {
            Destroy(gameObject);
        }
    }
}


public enum PooledSubObject
{
    Default = 0,
    GameObject = 1,
    VisualEffect = 2,
    Enemy = 3,
    Rigidbody = 4,
    TowerProjectile = 5,
    AnimProjectile = 6
}
