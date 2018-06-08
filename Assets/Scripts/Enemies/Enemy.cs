using System.Collections;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;

public class Enemy : PoolObj
{
    [SerializeField] private string m_EnemyString;
    public string EnemyString
    {
        get { return m_EnemyString; }
    }

    [SerializeField]private ParticleSystem m_DeathParticle;
    public delegate void DestroyEvent(Enemy enemy);
    public static DestroyEvent s_OnDestroyEnemy;

    [Header("Stats")]
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_CoinsToGive;
    public float CurrentHealth { get; set; }
    public bool IsAlive { get; set; }

    [Header("Object and animations")]
    [SerializeField]private SkeletonAnimation m_SkeletonAnims;
    public SkeletonAnimation SkeletonAnims
    {
        get { return m_SkeletonAnims; }
        set { m_SkeletonAnims = value; }
    }
    private int m_WaypointIndex;
    public int WaypointIndex
    {
        get { return m_WaypointIndex; }
    }
    private AnimationState m_Anim;
    private EnemyHealthbar m_EnemyHealthbar;
    private MeshRenderer m_Renderer;
    [SerializeField] private Transform m_EnemyTransform;

    private int m_CurrentNodeIndex;

    private void Awake()
    {
        CurrentHealth = m_MaxHealth;
        m_Renderer = GetComponentInChildren<MeshRenderer>();//GetComponent<MeshRenderer>();

        PauseCheck.Pause += TogglePause;

        m_EnemyHealthbar = GetComponent<EnemyHealthbar>();
    }

    private void OnEnable()
    {
        GameManager.s_OnGameStop += Death;
    }

    public void SetMaxHealth(int NewMaxInt)
    {
        m_MaxHealth = NewMaxInt;
    }

    public void RestoreHealth()
    {
        CurrentHealth = m_MaxHealth;
        m_EnemyHealthbar.ChangeEnemyHealthUI(CurrentHealth / m_MaxHealth);
    }

    public void TakeDamage(float damage, string towerType)
    {
        if (IsAlive)
        {
            CurrentHealth -= damage;
            m_EnemyHealthbar.ChangeEnemyHealthUI(CurrentHealth / m_MaxHealth);

            if (CurrentHealth <= 0)
            {
                if(gameObject.activeSelf)
                    StartCoroutine(Death(true));
            }
            else if (CurrentHealth > 0)
            {
                switch (towerType)
                {
                    case "Bass":
                        m_SkeletonAnims.AnimationState.SetAnimation(0, m_EnemyString + "HIT_Electricity", false);
                        m_SkeletonAnims.AnimationState.AddAnimation(0, m_EnemyString + "MOVE", true, 0);
                        break;
                    case "Drum":
                        EffectsManager.s_Instance.SpawnEffect(EffectType.ENEMY_HIT, false, new Vector2(transform.position.x, transform.position.y + 0.5f));
                        break;
                }
            }
        }
    }

    /// <summary>
    /// This gets added to the s_OnPlayListComplete delegate and won't give the player any coins for enemies that died this way.
    /// </summary>
    public void Death()
    {
        StartCoroutine(Death(false));
    }

    public IEnumerator Death(bool killedByPlayer)
    {
        DOTween.Kill(this);

        if (s_OnDestroyEnemy != null)
        {
            s_OnDestroyEnemy(this);
        }
        IsAlive = false;

        //If player kills the enemy
        if (killedByPlayer)
        {
            //Give coins
            PlayerData.s_Instance.ChangeCoinAmount(m_CoinsToGive);
        }

        m_DeathParticle.gameObject.SetActive(true);
        m_DeathParticle.Play();

        m_SkeletonAnims.AnimationState.SetAnimation(0, m_EnemyString + "DEATH", false).OnComplete();

        float animTime = m_SkeletonAnims.skeleton.data.FindAnimation(m_EnemyString + "DEATH").duration;

        yield return new WaitForSeconds(animTime);

        DeathCallback();
    }

    private void DeathCallback()
    {
        m_DeathParticle.gameObject.SetActive(false);
        ReturnToPool();
    }

    public void DamageObjective()
    {
        Effects.s_Screenshake(0.2f, 20);

        if (PlayerData.s_Instance.Lives > 0)
        {
            PlayerData.s_Instance.ChangeLivesAmount(-1);

            DOTween.Kill(this);
            ReturnToPool();
        }
    }

    public void Move(Vector3 startPos)
    {
        if (IsAlive)
        {
            DOTween.Kill(this);
            transform.position = startPos;
            Vector3[] pathArray = MapLoader.s_Instance.GetWaypointsFromPath();
            transform.DOPath(pathArray, pathArray.Length / m_MoveSpeed, PathType.CatmullRom).SetEase(Ease.Linear).SetId(this).OnComplete(() => DamageObjective()).OnWaypointChange(OnWaypointChange);
        }
    }

    /// <summary>
    /// Gets called every time the enemy lands on a new path tile
    /// </summary>
    /// <param name="waypointIndex">Index of the path position</param>
    private void OnWaypointChange(int waypointIndex)
    {
        m_WaypointIndex = waypointIndex;
        StartCoroutine(Callback());
        UpdateEnemyRotation(waypointIndex);
        UpdateEnemyLayering(waypointIndex);
    }

    /// <summary>
    /// Updates the enemy's sorting order based on the enemy's position in the grid
    /// </summary>
    /// <param name="waypointIndex">Index of the path position</param>
    private void UpdateEnemyLayering(int waypointIndex)
    {
        m_Renderer.sortingOrder = HexGrid.s_Instance.GridSize.y - MapLoader.s_Instance.Path[waypointIndex].PositionInGrid.y;
    }

    /// <summary>
    /// Rotates the enemy to the right direction
    /// </summary>
    /// <param name="waypointIndex"></param>
    private void UpdateEnemyRotation(int waypointIndex)
    {
        float currentX = MapLoader.s_Instance.Path[waypointIndex].transform.position.x;
        float nextX = MapLoader.s_Instance.Path[waypointIndex + 1].transform.position.x;

        //Checks if the X position of the next node in the path is higher or lower and rotates the enemy in the right direction
        if(nextX < currentX)
        {
            m_EnemyTransform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if(nextX > currentX)
        {
            m_EnemyTransform.rotation = new Quaternion(0, 180,0,0);
        }

    }


    public void TogglePause(bool pause)
    {
        if (pause)
        {
            DOTween.Pause(this);
        }
        else
        {
            DOTween.Play(this);
        }
    }

    private void OnDisable()
    {
        GameManager.s_OnGameStop -= Death;
    }
    
    IEnumerator Callback()
    {
        if(m_EnemyString == "Enemy2_")
        {
            int RNG = Random.Range(0, 101);

            if(RNG < 10)
            {
                float animTime = m_SkeletonAnims.skeleton.data.FindAnimation(m_EnemyString + "MOVE2_DAB").duration;

                m_SkeletonAnims.AnimationState.AddAnimation(0, m_EnemyString + "MOVE2_DAB", true, 0);

                yield return new WaitForSeconds(animTime);

                m_SkeletonAnims.AnimationState.AddAnimation(0, m_EnemyString + "MOVE", true, 0);
            }
        }        
    }
}