using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour {

    [SerializeField] private bool m_UseBounderies = true;
    [Space(10f)]
    [SerializeField] private float m_MinX;
    [SerializeField] private float m_MaxX;
    [SerializeField] private float m_MinY;
    [SerializeField] private float m_MaxY;
    [SerializeField] private float m_LerpSpeed; // Lower value is faster
    [SerializeField] private float m_ScreenOffset = 10;
    [SerializeField] private float m_MoveSpeed = 40;
    [Space(20f)]
    [SerializeField] private bool m_UseMouseInput = true;
    [SerializeField] private bool m_UseKeyInput = true;
    [SerializeField] private bool m_UseTouchInput = false;

    public static CameraMovement s_Instance;

    private Vector3 m_MovePos;

    private bool m_GotMouseInput;

    public bool CanMoveCamera { get; set; }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (s_Instance == null)
            s_Instance = this;
        else
            Destroy(gameObject);

        if(!m_UseBounderies)
            CanMoveCamera = true;
    }

    private void Update()
    {
        if (CanMoveCamera)
        {
            GetInput();
            MoveCamera();
        }

    }

    /// <summary>
    /// Allows the player to move the camera.
    /// Movement is clamped between X and Y values to make sure the player stays in the map
    /// </summary>
    void GetInput()
    {
        m_GotMouseInput = false;

        float currentX = transform.position.x;
        float currentY = transform.position.y;

        if(m_UseBounderies)
        {
            currentX = Mathf.Clamp(currentX, m_MinX, m_MaxX);
            currentY = Mathf.Clamp(currentY, m_MinY, m_MaxY);
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        m_MovePos = transform.position;

        if (m_UseMouseInput)
        {
            if (Input.mousePosition.x > Screen.width - m_ScreenOffset)
            {
                // Move Right
                m_MovePos += Vector3.right;
                m_GotMouseInput = true;
            }
            if (Input.mousePosition.x < m_ScreenOffset)
            {
                // Move Left
                m_MovePos += Vector3.left;
                m_GotMouseInput = true;
            }
            if (Input.mousePosition.y > Screen.height - m_ScreenOffset)
            {
                // Move Down
                m_MovePos -= Vector3.down;
                m_GotMouseInput = true;
            }
            if (Input.mousePosition.y < m_ScreenOffset)
            {
                // Move Up
                m_MovePos -= Vector3.up;
                m_GotMouseInput = true;
            }
        }

        if (m_UseKeyInput)
        {
            // Use Key movement
            if (!m_GotMouseInput)
                m_MovePos = new Vector3(m_MovePos.x + x, m_MovePos.y + y, transform.position.z);
        }

        // NEEDS TO BE TESTED ON MOBILE
        if (m_UseTouchInput)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // Get movement of the finger since last frame
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                // Set movePos
                m_MovePos = touchDeltaPosition;
            }
        }

        m_MovePos = new Vector3((m_UseBounderies ? Mathf.Clamp(m_MovePos.x, m_MinX, m_MaxX) : m_MovePos.x), (m_UseBounderies ? Mathf.Clamp(m_MovePos.y, m_MinY, m_MaxY) : m_MovePos.y), transform.position.z);   
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, m_MovePos, m_LerpSpeed * m_MoveSpeed * Time.deltaTime);
    }

    public void ScrollCameraToPosition(Tile tile, float duration, bool enableMoveCameraOnComplete, System.Action onComplete = null)
    {
        ScrollCameraToPosition(tile.transform, duration, enableMoveCameraOnComplete, onComplete);
    }

    public void ScrollCameraToPosition(Transform transform, float duration, bool enableMoveCameraOnComplete, System.Action onComplete = null)
    {
        ScrollCameraToPosition(transform.position, duration, enableMoveCameraOnComplete, onComplete);
    }

    public void ScrollCameraToPosition(Vector2 position, float duration, bool enableMoveCameraOnComplete, System.Action onComplete = null)
    {
        CanMoveCamera = false;
        transform.DOMove(new Vector3(position.x, position.y, transform.position.z), duration).SetEase(Ease.InOutQuad).OnComplete(delegate { if(onComplete != null) onComplete(); CanMoveCamera = enableMoveCameraOnComplete; });
    }
}