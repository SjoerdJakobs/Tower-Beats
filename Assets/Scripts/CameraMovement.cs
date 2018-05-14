using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour {

    [SerializeField] private float m_MinX;
    [SerializeField] private float m_MaxX;
    [SerializeField] private float m_MinY;
    [SerializeField] private float m_MaxY;
    [SerializeField] private float m_LerpSpeed; // Lower value is faster
    [SerializeField] private float m_ScreenOffset = 0;
    [SerializeField] private float m_MoveSpeed = 1;
    [Space(20f)]
    [SerializeField] private bool m_UseMouseInput = true;
    [SerializeField] private bool m_UseKeyInput = true;
    [SerializeField] private bool m_UseTouchInput = false;

    public static CameraMovement s_Instance;

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
    }

    void LateUpdate()
    {
        if (CanMoveCamera)
            MoveCamera();
    }

    /// <summary>
    /// Allows the player to move the camera.
    /// Movement is clamped between X and Y values to make sure the player stays in the map
    /// </summary>
    void MoveCamera()
    {
        m_GotMouseInput = false;

        float currentX = transform.position.x;
        float currentY = transform.position.y;

        currentX = Mathf.Clamp(currentX, m_MinX, m_MaxX);
        currentY = Mathf.Clamp(currentY, m_MinY, m_MaxY);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movePos = transform.position;

        if (m_UseMouseInput)
        {
            if (Input.mousePosition.x > Screen.width - m_ScreenOffset)
            {
                // Move Right
                movePos += Vector3.right;
                m_GotMouseInput = true;
            }
            if (Input.mousePosition.x < m_ScreenOffset)
            {
                // Move Left
                movePos += Vector3.left;
                m_GotMouseInput = true;
            }
            if (Input.mousePosition.y > Screen.height - m_ScreenOffset)
            {
                // Move Down
                movePos -= Vector3.down;
                m_GotMouseInput = true;
            }
            if (Input.mousePosition.y < m_ScreenOffset)
            {
                // Move Up
                movePos -= Vector3.up;
                m_GotMouseInput = true;
            }
        }

        if (m_UseKeyInput)
        {
            // Use Key movement
            if (!m_GotMouseInput)
                movePos = new Vector3(movePos.x + x, movePos.y + y, transform.position.z);
        }

        // NEEDS TO BE TESTED ON MOBILE
        if (m_UseTouchInput)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // Get movement of the finger since last frame
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                // Set movePos
                movePos = touchDeltaPosition;
            }
        }

        movePos = new Vector3(Mathf.Clamp(movePos.x, m_MinX, m_MaxX), Mathf.Clamp(movePos.y, m_MinY, m_MaxY), transform.position.z);

        transform.position = Vector3.Lerp(transform.position, movePos, m_LerpSpeed);
    }

    public void ScrollCameraToPosition(Tile tile, float duration, System.Action onComplete)
    {
        ScrollCameraToPosition(tile.transform, duration, onComplete);
    }

    public void ScrollCameraToPosition(Transform transform, float duration, System.Action onComplete)
    {
        ScrollCameraToPosition(transform.position, duration, onComplete);
    }

    public void ScrollCameraToPosition(Vector2 position, float duration, System.Action onComplete)
    {
        CanMoveCamera = false;
        transform.DOMove(new Vector3(position.x, position.y, transform.position.z), duration).SetEase(Ease.InOutQuad).OnComplete(delegate { onComplete(); CanMoveCamera = true; });
    }
}