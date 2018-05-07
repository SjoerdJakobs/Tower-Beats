using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShowHitIndicator : MonoBehaviour {

    public static ShowHitIndicator s_Instance;

    [SerializeField]private Image m_HitIndicator;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void HitIndicator()
    {
        Sequence hitIndicatorSequence = DOTween.Sequence();
        hitIndicatorSequence.Append(m_HitIndicator.DOFade(1, 0.2f));
        hitIndicatorSequence.Append(m_HitIndicator.DOFade(0, 0.2f));
    }
}