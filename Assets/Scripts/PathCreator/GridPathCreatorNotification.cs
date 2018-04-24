using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GridPathCreatorNotification : MonoBehaviour
{
    public enum NotificationType
    {
        LOG,
        WARNING,
        ERROR
    }

    [SerializeField]private Color m_GreenColor;
    [SerializeField]private Color m_YellowColor;
    [SerializeField]private Color m_RedColor;
    [Space]
    [SerializeField]private Image m_NotificationIcon;
    [SerializeField]private Text m_NotificationText;

    private RectTransform m_RectTransform;

    private Sequence m_ShowSequence;
    private Sequence m_HideSequence;

    private void Awake()
    {
        m_RectTransform = transform as RectTransform;
        SetStartingState();
    }

    /// <summary>
    /// Sets the starting state of the animated objects
    /// </summary>
    private void SetStartingState()
    {
        m_NotificationText.DOFade(0, 0);
        m_RectTransform.DOSizeDelta(new Vector2(0, m_RectTransform.sizeDelta.y), 0);
    }

    /// <summary>
    /// Shows a notification
    /// </summary>
    /// <param name="type">Type of notification</param>
    /// <param name="text">Text to show</param>
    public void ShowNotification(NotificationType type, string text)
    {
        switch(type)
        {
            case NotificationType.LOG:
                m_NotificationIcon.color = m_GreenColor;
                break;
            case NotificationType.WARNING:
                m_NotificationIcon.color = m_YellowColor;
                break;
            case NotificationType.ERROR:
                m_NotificationIcon.color = m_RedColor;
                break;
        }

        m_NotificationText.text = text;



        DOTween.KillAll(false);
        SetStartingState();

        Sequence m_ShowSequence = DOTween.Sequence();
        m_ShowSequence.Append(m_RectTransform.DOSizeDelta(new Vector2(500, m_RectTransform.sizeDelta.y), 0.75f)).SetEase(Ease.InOutCubic);
        m_ShowSequence.Append(m_NotificationText.DOFade(1, 0.33f));
        m_ShowSequence.AppendCallback(() => HideNotification(2f));
    }

    /// <summary>
    /// Hides the notification
    /// </summary>
    /// <param name="interval">Interval beteen Show and Hide</param>
    private void HideNotification(float interval)
    {
        Sequence m_HideSequence = DOTween.Sequence();
        m_HideSequence.AppendInterval(interval);
        m_HideSequence.Append(m_NotificationText.DOFade(0, 0.33f));
        m_HideSequence.Append(m_RectTransform.DOSizeDelta(new Vector2(0, m_RectTransform.sizeDelta.y), 0.75f)).SetEase(Ease.InOutCubic);
    }
}
