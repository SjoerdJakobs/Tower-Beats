using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class GetRMS : MonoBehaviour {

    public delegate void AudioCueEvent();
    public static AudioCueEvent s_BassCue;
    public static AudioCueEvent s_DrumCue;
    public static AudioCueEvent s_LeadCue;

    private PostProcessingProfile m_PostProcessingProfile;
    private float m_BloomIntensity;

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
    //private float refValue = 0.1f; // RMS value for 0 dB
    private float rmsValue;   // sound level - RMS

    private float[] samples; // audio samples

    [SerializeField]private AudioSource m_Source;

    private void OnEnable()
    {
        Sceneloader.s_OnSceneLoaded += SetSlider;
        if (Instrument == InstrumentGroup.Synth)
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
                    if(s_BassCue != null)
                        s_BassCue();
                    break;
                case InstrumentGroup.Drum:
                    //Debug.Log("Drum cue");
                    if(s_DrumCue != null)
                        s_DrumCue();
                    break;
                case InstrumentGroup.Synth:
                    //Debug.Log("Synth cue");
                    if (s_LeadCue != null)
                        s_LeadCue();
                    break;
            }
        }
    }

    void GetPostPostProcessingBehaviour()
    {
        m_PostProcessingProfile = Camera.main.GetComponent<PostProcessingBehaviour>().profile;
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
        if(m_PostProcessingProfile != null && Instrument == InstrumentGroup.Synth)
        {
            m_BloomIntensity = Mathf.Clamp(rmsValue * 60, 3.3f, 5.5f);
            //m_BloomIntensity = rmsValue * 60;
            BloomModel.Settings NewBloomSettings = new BloomModel.Settings();
            NewBloomSettings = m_PostProcessingProfile.bloom.settings;
            NewBloomSettings.bloom.intensity = m_BloomIntensity;
            m_PostProcessingProfile.bloom.settings = NewBloomSettings;
        }
    }

    private void OnDisable()
    {
        Sceneloader.s_OnSceneLoaded -= SetSlider;
    }
}
