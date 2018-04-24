using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundAnalyzerTest : MonoBehaviour {

    public delegate void SoundEvent();
    public static SoundEvent OnBass;
    public static SoundEvent OnDrum;
    [SerializeField]private AudioMixer m_AudioMixer;
    [SerializeField] private AudioSource m_Source;

    private float m_Volume;

	// Update is called once per frame
	void Update () {
        GetValue();
	}

    void GetValue()
    {
        m_AudioMixer.GetFloat("Bass", out m_Volume);
        Debug.Log(m_Volume);
    }
}
