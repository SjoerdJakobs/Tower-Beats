using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSampler : MonoBehaviour
{

    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField]private float[] m_Samples = new float[512];
    public float[] Samples { get { return m_Samples; } }

	private void Update()
    {
        m_AudioSource.GetSpectrumData(m_Samples, 0, FFTWindow.Blackman);
    }
}
