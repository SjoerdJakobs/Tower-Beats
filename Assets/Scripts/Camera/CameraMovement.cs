using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour {

    [SerializeField] private bool m_UseBoundaries = true;
    [Space(10f)]
    [SerializeField] private float m_LerpSpeed; // Lower value is faster
    [SerializeField] private float m_ScreenOffset = 10;
    [SerializeField] private float m_MoveSpeed = 40;
    [Space(20f)]
    [SerializeField] private bool m_UseMouseInput = true;
    [SerializeField] private bool m_UseKeyInput = true;
    [SerializeField] private bool m_UseTouchInput = false;

    private float minOrthographicSize = 5f;
    private float maxOrthographicSize = 8.5f;

    public static CameraMovement s_Instance;

    private float m_MinX, m_MaxX, m_MinY, m_MaxY;

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

        if (!m_UseBoundaries)
            CanMoveCamera = true;
    }

    private void Start()
    {
        SetCameraBounderies();
    }

    private void Update()
    {
        if (CanMoveCamera)
        {
            GetInput();
            SetCameraBounderies();
            MoveCamera();
        }

    }

    /// <summary>
    /// Zoom the camera
    /// </summary>
    /// <param name="value">0 is fully zoomed in, 1 is fully zoomed out</param>
    /// <param name="duration">Duration in seconds</param>
    public void Zoom(int value, int duration, Ease easing = Ease.Linear)
    {
        float mappedOrtohraphicSize = (value * (maxOrthographicSize - minOrthographicSize) / 1 + minOrthographicSize);

        if (duration == 0)
            Camera.main.orthographicSize = mappedOrtohraphicSize;
        else
            Camera.main.DOOrthoSize(mappedOrtohraphicSize, duration).SetEase(easing);
    }

    /// <summary>
    /// Set the camera bounderies to the size of the grid
    /// </summary>
    public void SetCameraBounderies()
    {
        Tile minTile = HexGrid.s_Instance.Grid[0, 0];
        Tile maxTile = HexGrid.s_Instance.Grid[HexGrid.s_Instance.GridSize.x - 1, HexGrid.s_Instance.GridSize.y - 1];

        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        // Calculations assume map is position at the origin
        m_MinX = minTile.transform.position.x + horzExtent;
        m_MaxX = maxTile.transform.position.x - horzExtent;
        m_MinY = (minTile.transform.position.y + vertExtent) - 0.2f;
        m_MaxY = (maxTile.transform.position.y - vertExtent) + 0.2f;
    }

    /// <summary>
    /// Convert a world position to an acceptable position within the bounderies
    /// </summary>
    /// <param name="position">Position to convert</param>
    /// <returns>A world position to an acceptable position within the bounderies</returns>
    public Vector3 GetPositionWithinBounderies(Vector3 position)
    {
        return new Vector3(Mathf.Clamp(position.x, m_MinX, m_MaxX), Mathf.Clamp(position.y, m_MinY, m_MaxY), position.z);
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

        if(m_UseBoundaries)
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

        m_MovePos = new Vector3((m_UseBoundaries ? Mathf.Clamp(m_MovePos.x, m_MinX, m_MaxX) : m_MovePos.x), (m_UseBoundaries ? Mathf.Clamp(m_MovePos.y, m_MinY, m_MaxY) : m_MovePos.y), transform.position.z);   
    }

    /// <summary>
    /// Moves the camera to the latest selected Move Position. (Gets called every frame when the player has control over the camera)
    /// </summary>
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
        Vector3 movePos = GetPositionWithinBounderies(new Vector3(position.x, position.y, transform.position.z));

        if (duration == 0)
        {
            transform.position = movePos;
            if (onComplete != null) onComplete();
            CanMoveCamera = enableMoveCameraOnComplete;
        }
        else
            transform.DOMove(movePos, duration).SetEase(Ease.InOutQuad).OnComplete(delegate { if (onComplete != null) onComplete(); CanMoveCamera = enableMoveCameraOnComplete; });
    }
}