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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(m_AudioMixer.GetFloat("",) >= 0.2f)
        {

        }
	}
}
