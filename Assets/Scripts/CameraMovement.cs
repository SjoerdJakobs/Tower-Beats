using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour {

    [SerializeField] private float m_MinX;
    [SerializeField] private float m_MaxX;
    [SerializeField] private float m_MinY;
    [SerializeField] private float m_MaxY;
    [SerializeField] private float m_LerpSpeed; // Lower value is faster
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        MoveCamera();
	}

    void MoveCamera()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movePos = new Vector3(transform.position.x + x, transform.position.y + y,-10);
        if (movePos.x < m_MaxX && movePos.x > m_MinX )
        {
            
        }

        if (movePos.x > m_MaxX)
        {
            movePos.x = m_MaxX;
        }
        else if(movePos.x < m_MinX)
        {
            movePos.x = m_MinX;
        }

        if(movePos.y > m_MaxY)
        {
            movePos.y = m_MaxY;
        }
        else if(movePos.y < m_MinY)
        {
            movePos.y = m_MinY;
        }

        transform.position = Vector3.Lerp(transform.position, movePos, m_LerpSpeed);
    }
}
