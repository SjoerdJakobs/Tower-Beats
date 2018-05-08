using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField] private float m_MinX;
    [SerializeField] private float m_MaxX;
    [SerializeField] private float m_MinY;
    [SerializeField] private float m_MaxY;
    [SerializeField] private float m_LerpSpeed; // Lower value is faster
    [SerializeField] private float m_ScreenOffset = 0;
    [SerializeField] private float m_MoveSpeed = 1;
    private bool m_UsingMouseInput;

    void LateUpdate () {

        MoveCamera();
	}

    /// <summary>
    /// Allows the player to move the camera.
    /// Movement is clamped between X and Y values to make sure the player stays in the map
    /// </summary>
    void MoveCamera()
    {
        m_UsingMouseInput = false;

        float currentX = transform.position.x;
        float currentY = transform.position.y;

        currentX = Mathf.Clamp(currentX, m_MinX, m_MaxX);
        currentY = Mathf.Clamp(currentY, m_MinY, m_MaxY);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 movePos = transform.position;

        if (Input.mousePosition.x > Screen.width - m_ScreenOffset)
        {
            // Move Right
            movePos += Vector3.right;
            m_UsingMouseInput = true;
        }
        if (Input.mousePosition.x < m_ScreenOffset)
        {
            // Move Left
            movePos += Vector3.left;
            m_UsingMouseInput = true;
        }
        if (Input.mousePosition.y > Screen.height - m_ScreenOffset)
        {
            // Move Down
            movePos += Vector3.up;
            m_UsingMouseInput = true;
        }
        if (Input.mousePosition.y < m_ScreenOffset)
        {
            // Move Up
            movePos += Vector3.down;
            m_UsingMouseInput = true;
        }

        // Use Key movement
        if(!m_UsingMouseInput)
            movePos = new Vector3(movePos.x + x, movePos.y + y, transform.position.z);

        movePos = new Vector3(Mathf.Clamp(movePos.x, m_MinX, m_MaxX), Mathf.Clamp(movePos.y, m_MinY, m_MaxY), transform.position.z);

        transform.position = Vector3.Lerp(transform.position, movePos, m_LerpSpeed);
    }
}