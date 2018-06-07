using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance;
    private int m_PreparationTime = 5;

    public delegate void PreparationTimeUpdated(int time);
    public static PreparationTimeUpdated s_OnPreparationTimeUpdated;

    public delegate void GameStarted();
    public static GameStarted s_OnGameStart;

    public delegate void GameStop();
    public static GameStop s_OnGameStop;

    private readonly string m_TutorialKey = "TUTORIAL";

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        StartGame("level1", true);
    }

    private void Awake()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartGame(string mapName, bool animateMap = true)
    {
        Tile.s_OnSetTileClickableState(false);
        MapLoader.s_Instance.LoadMap(mapName, animateMap);

        bool startGameWithTutorial = (mapName.ToUpper() == m_TutorialKey.ToUpper() ? true : false);

        StartCoroutine(StartGameSequence(startGameWithTutorial));
    }

    private IEnumerator StartGameSequence(bool showTutorial = false)
    {
        yield return new WaitUntil(() => HexGrid.s_Instance != null);
        yield return new WaitUntil(() => HexGrid.s_Instance.GridCreated == true);

        CameraMovement.s_Instance.ScrollCameraToPosition(HexGrid.s_Instance.GetMiddlepointTile(), 0f, false);
        CameraMovement.s_Instance.ZoomAnimated(1, 0, DG.Tweening.Ease.Linear);
        yield return new WaitUntil(() => MapLoader.s_Instance != null);
        yield return new WaitUntil(() => MapLoader.s_Instance.MapLoaded);

        bool hasShownTutorial = false;
        if(showTutorial)
        {
            StartCoroutine(ShowTutorial(() => hasShownTutorial = true ));
            yield return new WaitUntil(() =>  hasShownTutorial == true );
        }

        Tile.s_OnSetTileClickableState(true);
        CameraMovement.s_Instance.CanMoveCamera = true;
        StartCoroutine(StartPreparationTime(() => {
            if (s_OnGameStart != null) s_OnGameStart();
            SpawnContinuousWaves();
        }));
    }

    private void SpawnContinuousWaves()
    {
        EnemySpawner.s_Instance.SpawnWave(6, 1.5f, () => { SpawnContinuousWaves(); });
    }

    private IEnumerator StartPreparationTime(System.Action callback)
    {
        for (int i = m_PreparationTime; i >= 0; i--)
        {
            //if(s_OnPreparationTimeUpdated != null) s_OnPreparationTimeUpdated(i);
            NotificationManager.s_Instance.EnqueueNotification((i == 0 ? "Enemies incoming" : i.ToString()), (i == 0 ? 1.5f : 0.5f));
            //print("Time left: " + i);
            yield return new WaitForSeconds(1);
        }
        callback();
    }

    private IEnumerator ShowTutorial(System.Action callback = null)
    {
        //Base tutorial
        yield return new WaitForSeconds(0.5f);
        CameraMovement.s_Instance.ZoomAnimated(0, 1, DG.Tweening.Ease.InOutQuad);
        NotificationManager.s_Instance.EnqueueNotification("Your objective is to defend this tower in Hexagonia.", 2);
        CameraMovement.s_Instance.ScrollCameraToPosition(MapLoader.s_Instance.HeadQuarters, 1, false);
        yield return new WaitForSeconds(2.6f);
        MapLoader.s_Instance.HeadQuarters.transform.DOScale(1.1f, 0.3f).SetLoops(4, LoopType.Yoyo);
        yield return new WaitForSeconds(2f);

        //Enemy spawning tutorial
        NotificationManager.s_Instance.EnqueueNotification("Enemies will spawn from this red tile", 2);
        CameraMovement.s_Instance.ScrollCameraToPosition(MapLoader.s_Instance.Path[0], 1, false);
        yield return new WaitForSeconds(2.6f);
        MapLoader.s_Instance.Path[0].transform.DOScale(1.1f, 0.3f).SetLoops(4, LoopType.Yoyo);
        yield return new WaitForSeconds(2f);

        //Tower tutorial
        NotificationManager.s_Instance.EnqueueNotification("You can place towers by clicking on a white tile",2);
        CameraMovement.s_Instance.ScrollCameraToPosition(HexGrid.s_Instance.GetTurretSpawnpoints()[0],1,false);
        yield return new WaitForSeconds(2.6f);
        HexGrid.s_Instance.GetTurretSpawnpoints()[0].transform.DOScale(1.1f, 0.3f).SetLoops(4, LoopType.Yoyo);
        yield return new WaitForSeconds(2f);
        NotificationManager.s_Instance.EnqueueNotification("Clicking on the tower you bought will open a menu that will allow you to upgrade or sell the tower.", 6f);
        yield return new WaitForSeconds(6f);

        //Beat meter tutorial
        NotificationManager.s_Instance.EnqueueNotification("Towers react to the music, each tower responds to a different sound.", 5);
        yield return new WaitForSeconds(5f);
        NotificationManager.s_Instance.EnqueueNotification("In the top left corner you can see which tower reacts to which sound.",5);
        yield return new WaitForSeconds(6f);
        if(RMSSliders.s_HighlightSlider != null)
        {
            RMSSliders.s_HighlightSlider();
        }
        yield return new WaitForSeconds(2f);
        NotificationManager.s_Instance.EnqueueNotification("When the value of a sound reaches a threshold (white stripe in the bar) the matching towers will start shooting",6f);
        yield return new WaitForSeconds(6f);

        //Finish tutorial
        CameraMovement.s_Instance.CanMoveCamera = true;
        if (callback != null) callback();
    }
}
