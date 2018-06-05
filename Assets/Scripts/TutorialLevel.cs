using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialLevel : MonoBehaviour {


    [SerializeField] private List<Transform> m_BeatMeters = new List<Transform>();
    private bool m_BasicInfoFinished;
    private bool m_TileTutorialFinished;
    private bool m_BeatMeterTutorialFinished;
    private bool m_TowerMenuTutorialFinished;

	// Use this for initialization
	void Start () {
        StartCoroutine(StartTutorial());
	}
	
    IEnumerator StartTutorial()
    {
        yield return new WaitUntil(() => HexGrid.s_Instance != null);
        yield return new WaitUntil(() => HexGrid.s_Instance.GridCreated == true);

        CameraMovement.s_Instance.ScrollCameraToPosition(HexGrid.s_Instance.GetMiddlepointTile(), 0f, false);
        CameraMovement.s_Instance.ZoomAnimated(1, 0, DG.Tweening.Ease.Linear);
        yield return new WaitUntil(() => MapLoader.s_Instance != null);
        yield return new WaitUntil(() => MapLoader.s_Instance.MapLoaded);

        yield return new WaitForSeconds(0.5f);
        CameraMovement.s_Instance.ZoomAnimated(0, 1, DG.Tweening.Ease.InOutQuad);
        NotificationManager.s_Instance.EnqueueNotification("Your objective is to defend this tower in Hexagonia.", 3);
        CameraMovement.s_Instance.ScrollCameraToPosition(MapLoader.s_Instance.HeadQuarters, 1, false);

        yield return new WaitForSeconds(2.5f);
        NotificationManager.s_Instance.EnqueueNotification("Enemies will spawn from this red tile", 2);
        CameraMovement.s_Instance.ScrollCameraToPosition(MapLoader.s_Instance.Path[0], 1, false);

        yield return new WaitForSeconds(2.5f);
        m_BasicInfoFinished = true;
    }

    void MechanicsTutorial()
    {
        NotificationManager.s_Instance.EnqueueNotification("You can place towers by clicking on a white tile", 2);
        CameraMovement.s_Instance.ScrollCameraToPosition(HexGrid.s_Instance.GetTurretSpawnpoints()[0], 1, false);
        ScaleTile();
        
    }

    void ScaleTile()
    {
        Sequence tileScaleSequence = DOTween.Sequence();
        tileScaleSequence.Append(HexGrid.s_Instance.GetTurretSpawnpoints()[0].transform.DOScale(1.4f, 0.5f)).SetLoops(2);
        tileScaleSequence.OnComplete(() => m_TileTutorialFinished = true);

    }


	// Update is called once per frame
	void Update () {
		
	}
}
