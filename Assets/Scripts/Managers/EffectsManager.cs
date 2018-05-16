using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    ENEMY_SPAWN
}

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager s_Instance;
    private ObjectPool m_ObjectPool;
    [SerializeField] private SkeletonAnimation m_Prefab;

    private void Awake()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            Destroy(gameObject);

        m_ObjectPool = ObjectPoolManager.s_Instance.GetObjectPool(m_Prefab.gameObject, 10, 5, 5, 10);
    }

    public void SpawnEffect(EffectType type, Vector2 position)
    {
        SkeletonAnimation anim = m_ObjectPool.GetFromPool().GetComponent<SkeletonAnimation>();

        switch(type)
        {
            case EffectType.ENEMY_SPAWN:
                anim.AnimationName = "Spawn FX";
            break;
        }

        anim.transform.position = position;
    }
}
