using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInfo : MonoBehaviour {

    [SerializeField] private Text m_Coins;
    [SerializeField] private Image m_Lives;
    [SerializeField] private Text m_Song;
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
        m_Song.text = currentSong + "/" + maxSongs + "\n" + songName;
    }

    private void UpdatePreparationTime(int time)
    {
        if(time == 0)
        {
            m_PreparationTimer.text = "";
            m_PreparationTimer.gameObject.SetActive(false);
        }
        else
            m_PreparationTimer.text = "Prepare your defenses.\nSpawning in: " + time + " second(s)";
    }

    private void OnDestroy()
    {
        PlayerData.s_OnUpdateCoins -= UpdateCoinsUI;
        PlayerData.s_OnUpdateLives -= UpdateLivesUI;
        SongManager.s_OnChangeSong -= UpdateSongUI;
        GameManager.s_OnPreparationTimeUpdated -= UpdatePreparationTime;
    }
}