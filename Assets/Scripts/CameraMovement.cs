using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour {

    public static CameraMovement s_Instance;

    [SerializeField] private float m_MinX;
    [SerializeField] private float m_MaxX;
    [SerializeField] private float m_MinY;
    [SerializeField] private float m_MaxY;
    [SerializeField] private float m_LerpSpeed; // Lower value is faster

    private void Awake()
    {
        if(s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update () {
        MoveCamera();
	}

    /// <summary>
    /// Allows the player to move the camera.
    /// Movement is clamped between X and Y values to make sure the player stays in the map
    /// </summary>
    void MoveCamera()
    {
        float currentX = transform.position.x;
        float currentY = transform.position.y;
        currentX = Mathf.Clamp(currentX, m_MinX, m_MaxX);
        currentY = Mathf.Clamp(currentY, m_MinY, m_MaxY);
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movePos = new Vector3(currentX + x, currentY + y,-10);
        transform.position = Vector3.Lerp(transform.position, movePos, m_LerpSpeed);
    }

    public void Screenshake()
    {
        transform.DOShakePosition(0.2f,2,30, 30, false, true);
    }
}