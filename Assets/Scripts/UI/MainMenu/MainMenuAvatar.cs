using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuAvatar : MonoBehaviour
{
    [SerializeField] private Image m_EyeLids;
    [SerializeField] private Image m_OpenMouth;

    private Coroutine m_Blinking;

    private void Start()
    {
        if(m_Blinking == null)
            m_Blinking = StartCoroutine(RandomBlink());
    }

    private void OnDestroy()
    {
        StopCoroutine(m_Blinking);
    }

    private IEnumerator RandomBlink()
    {
        m_EyeLids.enabled = true;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.4f));
        m_EyeLids.enabled = false;
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));
        m_Blinking = StartCoroutine(RandomBlink());
    }

    public void SetMouthState(bool open)
    {
        if(open)
        {
            StopCoroutine(m_Blinking);
            m_EyeLids.enabled = true;
        }
        else
        {
            m_Blinking = StartCoroutine(RandomBlink());
        }
        m_OpenMouth.enabled = open;
    }
}
