using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class RMSSliders : MonoBehaviour
{
    public static Action s_HighlightSlider;

    [SerializeField] private List<RMSSlider> m_Sliders;

    private void Awake()
    {
        s_HighlightSlider += HighlightSlider;
    }

    public RMSSlider GetSlider(InstrumentGroup instrument)
    {
        return m_Sliders.Find(x => x.Instrument == instrument);
    }

    void HighlightSlider()
    {
        transform.DOScale(1.1f, 0.3f).SetLoops(4, LoopType.Yoyo);
    }
}
