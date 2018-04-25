using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRMS : MonoBehaviour {

    public delegate void AudioCueEvent();
    public static AudioCueEvent s_BassCue;
    public static AudioCueEvent s_DrumCue;
    public static AudioCueEvent s_SynthCue;

    //Used to identify the type of instrument
    public enum InstrumentGroup
    {
        BASS,
        DRUM,
        SYNTH,
        VOCAL
    }

    public InstrumentGroup Instrument;

    private int qSamples = 1024;  // array size
    private float refValue = 0.1f; // RMS value for 0 dB
    private float rmsValue;   // sound level - RMS
    private float dbValue;    // sound level - dB
    private float volume = 2; // set how much the scale will vary

    private float[] samples; // audio samples

    [SerializeField]private AudioSource m_Source;
 
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
        dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
        if (dbValue < -160) dbValue = -160; // clamp it to -160dB min

        //Debug.Log(rmsValue);

        if(rmsValue >= 0.06)
        {
            switch (Instrument)
            {
                case InstrumentGroup.BASS:
                    Debug.Log("Bass cue");
                    if(s_BassCue != null)
                    {
                        s_BassCue();
                    }
                    break;
                case InstrumentGroup.DRUM:
                    Debug.Log("Drum cue");
                    if(s_DrumCue != null)
                    {
                        s_DrumCue();
                    }
                    break;
            }
        }
    }

    void Update()
    {
        GetVolume();
        //transform.localScale.y = volume * rmsValue;
    }
}
