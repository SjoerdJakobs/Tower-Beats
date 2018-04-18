using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInfo : MonoBehaviour {

    [SerializeField] private Text m_Coins;
    [SerializeField] private Image m_Lives;

    private void Awake()
    {
        PlayerData.s_OnUpdateCoins += UpdateCoinsUI;
        PlayerData.s_OnUpdateLives += UpdateLivesUI;
    }

    void UpdateCoinsUI(float coins)
    {
        m_Coins.text = coins.ToString("N0"); //Makes sure the text wont show decimals
    }

    void UpdateLivesUI(float lives)
    {
        m_Lives.DOFillAmount(lives/9,0.2f);
    }

    private void OnDestroy()
    {
        PlayerData.s_OnUpdateCoins -= UpdateCoinsUI;
        PlayerData.s_OnUpdateLives -= UpdateLivesUI;
    }
}