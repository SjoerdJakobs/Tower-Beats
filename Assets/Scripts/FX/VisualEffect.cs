using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using System;

public class VisualEffect : MonoBehaviour
{
    private SkeletonAnimation m_Animation;

    public delegate void EffectCompleted(VisualEffect effect);
    public static EffectCompleted s_OnEffectCompleted;

    public bool InUse { get; set; }

    private void Awake()
    {
        m_Animation = GetComponent<SkeletonAnimation>();
    }

    public void Init(EffectType type, bool loop)
    {
        InUse = true;

        switch (type)
        {
            case EffectType.ENEMY_SPAWN:
                m_Animation.AnimationName = "Spawn FX";
                break;
            case EffectType.ENEMY_HIT:
                m_Animation.AnimationName = "Hit FX";
                break;
            case EffectType.BassTurretFX_Spawn:
                m_Animation.AnimationName = "BassTurretFX_Spawn";
                break;
            case EffectType.BassTurretFX_Attack:
                m_Animation.state.SetAnimation(0, "BassTurretFX_Attack", loop);
                break;
            case EffectType.BassTurretFX_Disappear:
                m_Animation.AnimationName = "BassTurretFX_Disappear";
                break;
            case EffectType.TURRET_SPAWN:
                m_Animation.AnimationName = "Spawn_Turret FX";
                break;
            case EffectType.EMPTY:
            default:
                m_Animation.state.SetAnimation(0, "1_Empty", loop);
                break;
        }

        m_Animation.loop = loop;
        m_Animation.AnimationState.Complete += OnEffectComplete;
    }

    public void Stop()
    {
        m_Animation.state.ClearTracks();
    }

    private void OnEffectComplete(TrackEntry trackEntry)
    {
        m_Animation.AnimationState.Complete -= OnEffectComplete;
        if (s_OnEffectCompleted != null) s_OnEffectCompleted(this);
    }
}
