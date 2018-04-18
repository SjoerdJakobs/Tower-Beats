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
        s_OnUpdateCoins += UpdateCoins;
        s_OnUpdateLives += UpdateLives;
    }

    private void Start()
    {
        s_OnUpdateCoins();
        //s_OnUpdateLives();
    }

    void UpdateCoins()
    {
        
        m_Coins.text = PlayerData.s_Instance.Coins.ToString("N0"); //Makes sure the text wont show decimals
    }

    void UpdateLives(float lives)
    {
        m_Lives.DOFillAmount(lives/9,0.2f);
        //m_Lives.text = PlayerData.s_Instance.Lives.ToString();
    }

    private void OnDestroy()
    {
        s_OnUpdateCoins -= UpdateCoins;
        s_OnUpdateLives -= UpdateLives;
    }
}
