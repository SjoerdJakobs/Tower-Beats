using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform[] m_Buttons;
    [SerializeField] private Credits m_Credits;
    [SerializeField] private RectTransform m_PlatformContainer;
    [SerializeField] private CanvasGroup m_PlatformAlpha;

    public void LoadScene(string scene)
    {
        Sceneloader.s_Instance.LoadScene(scene);
        SoundManager.s_Instance.PlaySound(SoundNames.BUTTON_CLICK);
    }

    public void ShowCredits()
    {
        StartCoroutine(AnimateButtons(false));
        m_Credits.gameObject.SetActive(true);
        SoundManager.s_Instance.PlaySound(SoundNames.BUTTON_CLICK);
    }

    public void HideCredits()
    {
        ShowMainButtons();
        m_Credits.gameObject.SetActive(false);
        SoundManager.s_Instance.PlaySound(SoundNames.BUTTON_CLICK);
    }

    private void Start()
    {
        SoundManager.s_Instance.PlaySound(SoundNames.BACKGROUND_MUSIC, true);
        ShowMainButtons();
        ShowDonDiablo();
    }


    public void QuitGame()
    {
        SoundManager.s_Instance.PlaySound(SoundNames.BUTTON_CLICK);
        Application.Quit();
    }

    private void SetDonDiabloDefaultState()
    {
        m_PlatformContainer.anchoredPosition = new Vector2(-25f, -400f);
        m_PlatformAlpha.alpha = 0;
    }

    private void ShowDonDiablo()
    {
        SetDonDiabloDefaultState();
        m_PlatformContainer.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutExpo);
        m_PlatformAlpha.DOFade(1, 0.33f);
    }

    public void ShowMainButtons()
    {
        SetButtonsDefaultState();
        StartCoroutine(AnimateButtons(true));
    }

    private void SetButtonsDefaultState()
    {
        for (int i = 0; i < m_Buttons.Length; i++)
            m_Buttons[i].localScale = Vector2.zero;
    }

    private IEnumerator AnimateButtons(bool animateIn)
    {
        for (int i = 0; i < m_Buttons.Length; i++)
        {
            m_Buttons[i].DOScale((animateIn ? 1 : 0), 0.5f).SetEase((animateIn ? Ease.OutExpo : Ease.InExpo));
            yield return new WaitForSeconds(0.05f);
        }
    }
}
