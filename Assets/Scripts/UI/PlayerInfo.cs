using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInfo : MonoBehaviour {

    [SerializeField] private Text m_Coins;
    [SerializeField] private Image m_Lives;
    [SerializeField] private Text m_SongText;
    [SerializeField] private Image m_DamageIndicator;
    [SerializeField] private Text m_PreparationTimer;

    private void Awake()
    {
        PlayerData.s_OnUpdateCoins += UpdateCoinsUI;
        PlayerData.s_OnUpdateLives += UpdateLivesUI;
        SongManager.s_OnChangeSong += UpdateSongUI;
        GameManager.s_OnPreparationTimeUpdated += UpdatePreparationTime;

        m_Lives.fillAmount = 0; // Makes sure the bar is empty at the start before the filling tween starts
        Sequence healthBarStartSequence = DOTween.Sequence();
        healthBarStartSequence.AppendInterval(0.3f);
        healthBarStartSequence.Append(m_Lives.DOFillAmount(1, 3f)).SetEase(Ease.Linear);
        healthBarStartSequence.Insert(0.3f, m_Lives.DOColor(Color.red, 0.2f));
        healthBarStartSequence.Insert(0.7f, m_Lives.DOColor(Effects.s_EffectColors["Yellow"], 0.2f));
        healthBarStartSequence.Insert(1.8f, m_Lives.DOColor(Color.green, 0.2f));
    }

    void UpdateCoinsUI(float coins)
    {
        m_Coins.text = coins.ToString("N0"); //Makes sure the text wont show decimals
    }

    void UpdateLivesUI(float lives)
    {
        m_Lives.DOFillAmount(lives/10,0.3f);
        if(lives > 7)
        {
            Effects.ChangeImageColor(m_Lives, Color.green, 0.3f);
        }
        else if(lives <= 7 && lives > 3)
        {
            Effects.ChangeImageColor(m_Lives, Effects.s_EffectColors["Yellow"], 0.3f);
            //Effects.ChangeImageColor(m_Lives, Effects.s_EffectColors["Yellow"], 0.3f);
        }
        else if(lives <= 3)
        {
            Effects.ChangeImageColor(m_Lives, Effects.s_EffectColors["Red"], 0.3f);
        }

        //Effects.s_ImageFlash(m_DamageIndicator, 0.2f, 2, Effects.s_EffectColors["Red"]);
    }

    void UpdateSongUI(int currentSong, int maxSongs,string songName)
    {
        m_SongText.text = currentSong + "/" + maxSongs + "  " + songName;
    }

    private void UpdatePreparationTime(int time)
    {
        m_PreparationTimer.color = new Color(m_PreparationTimer.color.r, m_PreparationTimer.color.g, m_PreparationTimer.color.b, 0);
        m_PreparationTimer.transform.localScale = new Vector3(1.3f, 1.3f);

        m_PreparationTimer.transform.DOScale(1, 0.2f).SetEase(Ease.OutExpo);
        m_PreparationTimer.DOFade(1, 0.1f);

        m_PreparationTimer.transform.DOScale(0.7f, 0.2f).SetEase(Ease.InExpo).SetDelay(0.55f);
        m_PreparationTimer.DOFade(0, 0.1f).SetDelay(0.65f);

        if(time == 0)
        {
            m_PreparationTimer.text = "";
        }
        else
            m_PreparationTimer.text = time.ToString();
    }

    private void OnDestroy()
    {
        PlayerData.s_OnUpdateCoins -= UpdateCoinsUI;
        PlayerData.s_OnUpdateLives -= UpdateLivesUI;
        SongManager.s_OnChangeSong -= UpdateSongUI;
        GameManager.s_OnPreparationTimeUpdated -= UpdatePreparationTime;
    }
}