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
        }

        m_Animation.loop = loop;
        m_Animation.AnimationState.Complete += OnEffectComplete;
    }

    private void OnEffectComplete(TrackEntry trackEntry)
    {
        m_Animation.AnimationState.Complete -= OnEffectComplete;
        if (s_OnEffectCompleted != null) s_OnEffectCompleted(this);
    }
}
