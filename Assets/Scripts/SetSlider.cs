using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSlider : MonoBehaviour {

    [SerializeField] private string m_TrackName;

    private void Awake()
    {
        GameObject sliderGO = GameObject.Find(m_TrackName);

        Slider slider = GetComponent<Slider>();

        sliderGO.GetComponent<GetRMS>().Slider = slider;
    }
}
