using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    ENEMY_SPAWN,
    ENEMY_HIT,
    BassTurretFX_Attack,
    LASER_TURRET_ATTACK,
    SHOOT_TURRET_ATTACK,
    BassTurretFX_Spawn,
    BassTurretFX_Disappear,
    TURRET_SPAWN,
	LASERBEAM_HIT,
    EMPTY
}

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager s_Instance;
    [SerializeField] private VisualEffect m_EffectPrefab;

    private List<VisualEffect> m_EffectsPool = new List<VisualEffect>();

    private void Awake()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            Destroy(gameObject);

        VisualEffect.s_OnEffectCompleted += OnEffectCompleted;

        InitEffectsPool(10);
    }

    private void InitEffectsPool(int amountOfEffects)
    {
        for (int i = 0; i < amountOfEffects; i++)
            AddEffectToPool();
    }

    private VisualEffect AddEffectToPool()
    {
        VisualEffect effect = Instantiate(m_EffectPrefab, transform, false);
        effect.gameObject.SetActive(false);
        m_EffectsPool.Add(effect);
        return effect;
    }

    private void OnDestroy()
    {
        VisualEffect.s_OnEffectCompleted -= OnEffectCompleted;
    }

    public void SpawnEffect(EffectType type, bool loop, Vector2 position)
    {
        for (int i = 0; i < m_EffectsPool.Count; i++)
        {
            if(!m_EffectsPool[i].InUse)
            {
                InitEffect(m_EffectsPool[i], type, loop, position);
                return;
            }
        }
        VisualEffect effect = AddEffectToPool();
        InitEffect(effect, type, loop, position);
    }

    private void InitEffect(VisualEffect effect, EffectType type, bool loop, Vector2 position)
    {
        effect.gameObject.SetActive(true);
        effect.Init(type, loop);
        effect.transform.position = position;
    }

    private void OnEffectCompleted(VisualEffect effect)
    {
        for (int i = 0; i < m_EffectsPool.Count; i++)
        {
            if (effect == m_EffectsPool[i])
            {
                m_EffectsPool[i].gameObject.SetActive(false);
                m_EffectsPool[i].InUse = false;
            }
        }
    }
}