using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInfo : MonoBehaviour {

    public delegate void InfoUpdate();
    public delegate void LivesUpdate(float lives);
    public static InfoUpdate s_OnUpdateCoins;
    public static LivesUpdate s_OnUpdateLives;

    [SerializeField] private Text m_Coins;
    [SerializeField] private Image m_Lives;

    private void Awake()
    {
        s_OnUpdateCoins += UpdateCoinsUI;
        s_OnUpdateLives += UpdateLivesUI;
    }

    private void Start()
    {
        s_OnUpdateCoins();
    }

    void UpdateCoinsUI()
    {
        m_Coins.text = PlayerData.s_Instance.Coins.ToString("N0"); //Makes sure the text wont show decimals
    }

    void UpdateLivesUI(float lives)
    {
        m_Lives.DOFillAmount(lives/9,0.2f);
    }

    private void OnDestroy()
    {
        s_OnUpdateCoins -= UpdateCoinsUI;
        s_OnUpdateLives -= UpdateLivesUI;
    }
}
