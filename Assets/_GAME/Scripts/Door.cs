using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float m_OpeningSpeed = 10f;
    [SerializeField] bool m_IsOpen;
    [SerializeField] bool m_OpeningDown = true;
    [SerializeField] Vector3 m_StartPosition, m_TargetPosition;
    [SerializeField] AnimationCurve m_OpeningCurve;
    float timeElapsed;
    [SerializeField] float m_DoorTimer = 1;

    private void Start()
    {
        m_StartPosition = transform.position;
        if(m_TargetPosition == Vector3.zero)
        {
            if(m_OpeningDown)
                m_TargetPosition = m_StartPosition + new Vector3(0,-transform.lossyScale.y * 2,0);
            else
                m_TargetPosition = m_StartPosition + new Vector3(0, transform.lossyScale.y * 2, 0);

        }
        //ToggleDoor();
    }

    void Update()
    {
        if (timeElapsed < m_DoorTimer)
        {
            if (m_IsOpen && transform.position != m_TargetPosition)
            {
                transform.position = Vector3.Lerp(m_StartPosition, m_TargetPosition, timeElapsed / m_DoorTimer);
            }
            else if (!m_IsOpen && transform.position != m_StartPosition)
            {
                transform.position = Vector3.Lerp(m_TargetPosition, m_StartPosition, timeElapsed / m_DoorTimer);
            }
            timeElapsed += Time.deltaTime;
        }
    }

    public void ToggleDoor()
    {
        m_IsOpen = !m_IsOpen;
        timeElapsed = 0;
    }

}
