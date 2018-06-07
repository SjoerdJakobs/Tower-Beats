using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour {

    [SerializeField] private bool m_UseBoundaries = true;
    [Space(10f)]
    [SerializeField] private float m_PanSpeed = 40;
    [SerializeField] private float m_ZoomSpeed = 10;

    private float minOrthographicSize = 5f;
    private float maxOrthographicSize = 8.5f;

    public static CameraMovement s_Instance;

    private float m_MinX, m_MaxX, m_MinY, m_MaxY;

    private Camera m_MainCamera;
    [Space]
    [SerializeField] private Camera m_UICamera;

    public bool CanMoveCamera { get; set; }

    private Vector3 m_LastPanPosition;
    private int m_PanFingerID;
    private bool m_WasZoomingLastFrame;
    private Vector2[] m_LastZoomPositions;

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

        m_MainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        SetCameraBoundaries();
    }

    private void Update()
    {
        if (CanMoveCamera)
        {
            if (!IsPointerOverUIObject())
            {
                if (Input.touchSupported && Application.isMobilePlatform)
                    HandleTouch();
                else
                    HandleMouse();

                if(m_UseBoundaries)
                    SetCameraBoundaries();
            }
        }
    }

    private void HandleTouch()
    {
        switch(Input.touchCount)
        {
            case 1:
                m_WasZoomingLastFrame = false;

                Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began)
                {
                    m_LastPanPosition = touch.position;
                    m_PanFingerID = touch.fingerId;
                    DOTween.Kill(transform);
                }
                else if(touch.fingerId == m_PanFingerID && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2:
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!m_WasZoomingLastFrame)
                {
                    m_LastZoomPositions = newPositions;
                    m_WasZoomingLastFrame = true;
                }
                else
                {
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(m_LastZoomPositions[0], m_LastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, m_ZoomSpeed / 1000);

                    m_LastZoomPositions = newPositions;
                }
                break;

            default:
                m_WasZoomingLastFrame = false;
                break;
        }
    }

    private void HandleMouse()
    {
        Vector3 movePosition = transform.position;
        if(Input.GetMouseButtonDown(0))
        {
            m_LastPanPosition = Input.mousePosition;
            DOTween.Kill(transform);
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, m_ZoomSpeed);
    }

    private void PanCamera(Vector3 newPanPosition)
    {
        Vector3 offset = m_MainCamera.ScreenToViewportPoint(m_LastPanPosition - newPanPosition);
        Vector2 move = new Vector2(offset.x * m_PanSpeed, offset.y * m_PanSpeed);

        transform.Translate(move, Space.World);

        Vector3 fixedPosition = GetPositionWithinBoundaries(transform.position);

        if(m_UseBoundaries)
            transform.position = fixedPosition;

        m_LastPanPosition = newPanPosition;
    }

    private void ZoomCamera(float offset, float speed)
    {
        if (offset == 0) return;

        float calcOrthographicSize = Mathf.Clamp(m_MainCamera.orthographicSize - (offset * speed), minOrthographicSize, maxOrthographicSize);
        m_UICamera.orthographicSize = calcOrthographicSize;
        m_MainCamera.orthographicSize = calcOrthographicSize;

        if (m_UseBoundaries)
            transform.position = GetPositionWithinBoundaries(transform.position);
    }

    /// <summary>
    /// Zoom the camera
    /// </summary>
    /// <param name="value">0 is fully zoomed in, 1 is fully zoomed out</param>
    /// <param name="duration">Duration in seconds</param>
    public void ZoomAnimated(int value, float duration, Ease easing = Ease.Linear)
    {
        float mappedOrtohraphicSize = MappedOrthographicSize(value);

        if (duration == 0)
            m_MainCamera.orthographicSize = mappedOrtohraphicSize;
        else
            m_MainCamera.DOOrthoSize(mappedOrtohraphicSize, duration).SetEase(easing);
    }

    private float MappedOrthographicSize(float value)
    {
        return (value * (maxOrthographicSize - minOrthographicSize) / 1 + minOrthographicSize);
    }

    /// <summary>
    /// Set the camera boundaries to the size of the grid
    /// </summary>
    public void SetCameraBoundaries()
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
    /// Convert a world position to an acceptable position within the boundaries
    /// </summary>
    /// <param name="position">Position to convert</param>
    /// <returns>A world position to an acceptable position within the boundaries</returns>
    public Vector3 GetPositionWithinBoundaries(Vector3 position)
    {
        return new Vector3(Mathf.Clamp(position.x, m_MinX, m_MaxX), Mathf.Clamp(position.y, m_MinY, m_MaxY), position.z);
    }

    public bool IsPositionOutOfBounds(Vector3 position)
    {
        if (position.x > m_MaxX || position.x < m_MinX)
            return true;
        else if (position.y > m_MaxY || position.y < m_MinY)
            return true;

        return false;
    }

    public void ScrollCameraToPosition(Tile tile, float duration, bool enableMoveCameraOnComplete, bool overwriteCanMoveCamera = false, System.Action onComplete = null)
    {
        ScrollCameraToPosition(tile.transform, duration, enableMoveCameraOnComplete, overwriteCanMoveCamera, onComplete);
    }

    public void ScrollCameraToPosition(Transform transform, float duration, bool enableMoveCameraOnComplete, bool overwriteCanMoveCamera = false, System.Action onComplete = null)
    {
        ScrollCameraToPosition(transform.position, duration, enableMoveCameraOnComplete, overwriteCanMoveCamera, onComplete);
    }

    public void ScrollCameraToPosition(Vector2 position, float duration, bool enableMoveCameraOnComplete, bool overwriteCanMoveCamera = false, System.Action onComplete = null)
    {
        CanMoveCamera = overwriteCanMoveCamera;
        Vector3 movePos = GetPositionWithinBoundaries(new Vector3(position.x, position.y, transform.position.z));

        if (duration == 0)
        {
            transform.position = movePos;
            if (onComplete != null) onComplete();
            CanMoveCamera = enableMoveCameraOnComplete;
        }
        else
        {
            transform.DOMove(movePos, duration).SetEase(Ease.InOutQuad).OnComplete(delegate { if (onComplete != null) onComplete(); CanMoveCamera = enableMoveCameraOnComplete; });
        }
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}