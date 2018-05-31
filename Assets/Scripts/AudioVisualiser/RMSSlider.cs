using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RMSSlider : MonoBehaviour
{
    [SerializeField] private InstrumentGroup m_Instrument;
    public InstrumentGroup Instrument { get { return m_Instrument; } }
    [SerializeField] private Image m_FillMeter;
    [SerializeField] private Slider m_Threshold;

    /// <summary>
    /// Set the fill amount of the RMS Slider
    /// </summary>
    /// <param name="fillAmount">Amount that needs to be filled</param>
    public void SetFill(float fillAmount)
    {
        m_FillMeter.fillAmount = fillAmount;
    }

    /// <summary>
    /// Set the threshold of the RMS Slider
    /// </summary>
    /// <param name="thresholdAmount">Threshold value (min 0 / max 1)</param>
    public void SetThreshold(float thresholdAmount)
    {
        m_Threshold.value = thresholdAmount;
    }
}
