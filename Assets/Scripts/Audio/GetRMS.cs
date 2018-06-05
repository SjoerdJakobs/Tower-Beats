using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;
using UnityEngine.UI;

//Used to identify the type of instrument
public enum InstrumentGroup
{
    Bass,
    Drum,
    Lead
}

public class GetRMS : MonoBehaviour {

    public delegate void AudioCueEvent();
    public static AudioCueEvent s_BassCue;
    public static AudioCueEvent s_DrumCue;
    public static AudioCueEvent s_LeadCue;

    public static AudioCueEvent s_OnBassLost;

    private Bloom m_BloomProfile;
    private float m_BloomIntensity;

    private float m_Timer;

    public InstrumentGroup Instrument;

    public RMSSlider Slider { get; set; }

    private int qSamples = 1024;  // array size
    //private float refValue = 0.1f; // RMS value for 0 dB
    private float rmsValue;   // sound level - RMS

    private float[] samples; // audio samples

    [SerializeField]private AudioSource m_Source;

    private void OnEnable()
    {
        Sceneloader.s_OnSceneLoaded += SetSlider;
        if (Instrument == InstrumentGroup.Lead)
        {
            Sceneloader.s_OnSceneLoaded += GetPostPostProcessingBehaviour;
        }
    }

    void Start()
    {

        //PostProcessingBehaviour filters = GetComponent<PostProcessingBehaviour>();
        samples = new float[qSamples];
    }

    void GetVolume()
    {
        m_Timer += Time.deltaTime;

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
                    //Debug.Log("Bass cue");
                    m_Timer = 0f;
                    if(s_BassCue != null)
                        s_BassCue();
                    break;
                case InstrumentGroup.Drum:
                    //Debug.Log("Drum cue");
                    if(s_DrumCue != null)
                        s_DrumCue();
                    break;
                case InstrumentGroup.Lead:
                    //Debug.Log("Synth cue");
                    if (s_LeadCue != null)
                        s_LeadCue();
                    break;
            }
        }
        else
        {
            if(Instrument == InstrumentGroup.Bass)
            {
                if(m_Timer > 0.5f && s_OnBassLost != null)
                    s_OnBassLost();
            }
        }
    }

    void GetPostPostProcessingBehaviour()
    {
        m_BloomProfile = Camera.main.GetComponent<Bloom>();
    }

    void SetSlider()
    {
        Slider = FindObjectOfType<RMSSliders>().GetSlider(Instrument);
        Slider.SetThreshold(0.06f * 6f);
    }

    void Update()
    {
        GetVolume();
        //transform.localScale.y = volume * rmsValue;
        if (Slider != null)
            Slider.SetFill((rmsValue * 6));
        if(m_BloomProfile != null && Instrument == InstrumentGroup.Lead)
        {
            m_BloomIntensity = Mathf.Clamp(1+(rmsValue * 6), 1f, 2f);
            //m_BloomIntensity = rmsValue * 60;
            m_BloomProfile.intensity = m_BloomIntensity;
        }
    }

    private void OnDisable()
    {
        Sceneloader.s_OnSceneLoaded -= SetSlider;
    }
}
