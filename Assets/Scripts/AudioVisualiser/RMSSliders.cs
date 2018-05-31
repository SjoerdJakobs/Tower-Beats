using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RMSSliders : MonoBehaviour
{
    [SerializeField] private List<RMSSlider> m_Sliders;

    public RMSSlider GetSlider(InstrumentGroup instrument)
    {
        return m_Sliders.Find(x => x.Instrument == instrument);
    }
}
