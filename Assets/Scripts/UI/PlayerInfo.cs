using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInfo : MonoBehaviour {

    [SerializeField] private Text m_Coins;
    [SerializeField] private Image m_Lives;
    [SerializeField] private Text m_Song;

    private void Awake()
    {
        PlayerData.s_OnUpdateCoins += UpdateCoinsUI;
        PlayerData.s_OnUpdateLives += UpdateLivesUI;
        SongManager.s_OnChangeSong += UpdateSongUI;
    }

    void UpdateCoinsUI(float coins)
    {
        m_Coins.text = coins.ToString("N0"); //Makes sure the text wont show decimals
    }

    void UpdateLivesUI(float lives)
    {
        m_Lives.DOFillAmount(lives/10,0.2f);
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