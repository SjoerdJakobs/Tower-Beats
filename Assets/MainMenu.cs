using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform[] m_Buttons;

    public void LoadLevelSelect()
    {
        Sceneloader.s_Instance.LoadScene("LevelSelection");
    }

    private void Start()
    {
        SetButtonsDefaultState();
        StartCoroutine(AnimateButtons());
    }

    private void SetButtonsDefaultState()
    {
        for (int i = 0; i < m_Buttons.Length; i++)
            m_Buttons[i].localScale = Vector2.zero;
    }

    private IEnumerator AnimateButtons()
    {
        for (int i = 0; i < m_Buttons.Length; i++)
        {
            m_Buttons[i].DOScale(1, 0.5f).SetEase(Ease.OutExpo);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
