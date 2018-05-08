using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInfo : MonoBehaviour {

    [SerializeField] private Text m_Coins;
    [SerializeField] private Image m_Lives;
    [SerializeField] private Text m_Song;
    [SerializeField] private Image m_DamageIndicator;

    private void Awake()
    {
        PlayerData.s_OnUpdateCoins += UpdateCoinsUI;
        PlayerData.s_OnUpdateLives += UpdateLivesUI;
        SongManager.s_OnChangeSong += UpdateSongUI;

        Sequence healthBarStartSequence = DOTween.Sequence();
        //healthBarStartSequence.Append

    }

    void UpdateCoinsUI(float coins)
    {
        m_Coins.text = coins.ToString("N0"); //Makes sure the text wont show decimals
    }

    void UpdateLivesUI(float lives)
    {
        m_Lives.DOFillAmount(lives/10,0.2f);
        if(lives == 7)
        {
            Effects.YoyoImageColor(m_Lives, Effects.s_EffectColors["Yellow"], 3, 0.3f);
            //Effects.ChangeImageColor(m_Lives, Effects.s_EffectColors["Yellow"], 0.3f);
        }
        else if(lives == 3)
        {
            Effects.ChangeImageColor(m_Lives, Effects.s_EffectColors["Red"], 0.3f);
        }

        Effects.s_ImageFlash(m_DamageIndicator, 0.2f, 2, Effects.s_EffectColors["Red"]);
    }

    void UpdateSongUI(int currentSong, int maxSongs,string songName)
    {
        m_Song.text = currentSong + "/" + maxSongs + "\n" + songName;
    }

    private void OnDestroy()
    {
        PlayerData.s_OnUpdateCoins -= UpdateCoinsUI;
        PlayerData.s_OnUpdateLives -= UpdateLivesUI;
    }
}