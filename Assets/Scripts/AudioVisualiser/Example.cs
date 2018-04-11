/*
 * Copyright (c) 2015 Allan Pichardo
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System;


public class Example : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _objects;
    [SerializeField]
    private int _scalerMod = 250;
    [SerializeField]
    private LinkedObj[] _linkedObjePillar;


    void Start ()
	{
		//Select the instance of AudioProcessor and pass a reference
		//to this object
		AudioProcessor processor = FindObjectOfType<AudioProcessor> ();
		processor.onBeat.AddListener (onOnbeatDetected);
		processor.onSpectrum.AddListener (onSpectrum);
	}

	//this event will be called every time a beat is detected.
	//Change the threshold parameter in the inspector
	//to adjust the sensitivity
	void onOnbeatDetected ()
	{
		Debug.Log ("Beat!!!");
	}

	//This event will be called every frame while music is playing
	void onSpectrum (float[] spectrum)
	{
		//The spectrum is logarithmically averaged
		//to 12 bands

		for (int i = 0; i < spectrum.Length; ++i) {
			//Vector3 start = new Vector3 (i, 0, 0);
			//Vector3 end = new Vector3 (i, spectrum [i]*2, 0);
            float spectrumF = spectrum[i] * _scalerMod;
            for (int j = 0; j < _linkedObjePillar.Length; j++)
            {
                if (i == _linkedObjePillar[j]._spherePillarLink)
                {
                    if (spectrumF > _linkedObjePillar[j]._sphereThreshHold)
                    {
                        _linkedObjePillar[j]._linkedObjePillar.transform.localScale = new Vector3(spectrumF, spectrumF, spectrumF) * _linkedObjePillar[j]._sphereMultiplier;
                    }
                }
                else if (_linkedObjePillar[j]._linkedObjePillar.transform.localScale.x > 0.9f)
                {
                    _linkedObjePillar[j]._linkedObjePillar.transform.localScale = new Vector3(_linkedObjePillar[j]._linkedObjePillar.transform.localScale.x - 0.01f, _linkedObjePillar[j]._linkedObjePillar.transform.localScale.y - 0.01f, _linkedObjePillar[j]._linkedObjePillar.transform.localScale.z - 0.01f);
                }
            }
            _objects[i].transform.localScale = new Vector3(1, spectrumF, 1);
            _objects[i].transform.position = new Vector3(_objects[i].transform.position.x, spectrumF/2, _objects[i].transform.position.z);

            //Debug.DrawLine (start, end);
		}
	}
}


[System.Serializable]
public struct LinkedObj
{
    public GameObject _linkedObjePillar;
    public int _spherePillarLink;
    public float _sphereThreshHold;
    public float _sphereMultiplier;
}