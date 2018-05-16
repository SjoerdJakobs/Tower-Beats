using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRMS : MonoBehaviour {

    public delegate void AudioCueEvent();
    public static AudioCueEvent s_BassCue;
    public static AudioCueEvent s_DrumCue;
    public static AudioCueEvent s_SynthCue;

    //Used to identify the type of instrument
    public enum InstrumentGroup
    {
        Bass,
        Drum,
        Synth,
        Vocal
    }

    public InstrumentGroup Instrument;

    private Slider m_Slider;

    public Slider Slider
    {
        get { return m_Slider; }
        set { m_Slider = value; }
    }

    private int qSamples = 1024;  // array size
    private float refValue = 0.1f; // RMS value for 0 dB
    private float rmsValue;   // sound level - RMS

    private float[] samples; // audio samples

    [SerializeField]private AudioSource m_Source;

    private void OnEnable()
    {
        Sceneloader.s_OnSceneLoaded += SetSlider;
    }

    void Start()
    {
        samples = new float[qSamples];
    }

    void GetVolume()
    {
        m_Source.GetOutputData(samples, 0); // fill array with samples
        int i;
        float sum = 0;

        for (i = 0; i < qSamples; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }
        rmsValue = Mathf.Sqrt(sum / qSamples); // rms = square root of average

        if(rmsValue >= 0.06)
        {
            switch (Instrument)
            {
                case InstrumentGroup.Bass:
                    Debug.Log("Bass cue");
                    if(s_BassCue != null)
                        s_BassCue();
                    break;
                case InstrumentGroup.Drum:
                    Debug.Log("Drum cue");
                    if(s_DrumCue != null)
                        s_DrumCue();
                    break;
                case InstrumentGroup.Synth:
                    Debug.Log("Synth cue");
                    if (s_SynthCue != null)
                        s_SynthCue();
                    break;
            }
        }
    }

    void SetSlider()
    {
        m_Slider = GameObject.Find(Instrument.ToString() + "Slider").GetComponent<Slider>();
    }

    void Update()
    {
        GetVolume();
        //transform.localScale.y = volume * rmsValue;
        if(m_Slider != null)
            m_Slider.value = (rmsValue * 6);
    }

    private void OnDisable()
    {
        Sceneloader.s_OnSceneLoaded -= SetSlider;
    }
}
