using UnityEngine;

public class ClearAudioClips : MonoBehaviour {

    public delegate void ClearAudioEvent();
    public static ClearAudioEvent s_OnClearAudio;

    public void ClearAudio()
    {
        if(s_OnClearAudio != null)
        {
            s_OnClearAudio();
        }
    }
}
