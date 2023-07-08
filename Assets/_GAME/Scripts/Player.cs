using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] bool m_WeaponEnabled;
    [SerializeField] float m_BaseHorizontalSpeed = 10f, m_ActiveHorizontalSpeed;
    [SerializeField] float m_BaseJumpSpeed = 10f, m_ActiveJumpSpeed;
    [SerializeField] Vector2 m_ActiveVelocity;
    float ySpeed;

    [Header("References")]
    public bool m_References;
    [SerializeField] Transform m_ObjectSlot;
    public CharacterController m_Controller;
    [SerializeField] Animator m_Animator;
    void Start()
    {
        m_ActiveHorizontalSpeed = m_BaseHorizontalSpeed;
        m_ActiveJumpSpeed = m_BaseJumpSpeed;
    }

    void Update()
    {

        Vector2 move = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(!m_Controller.isGrounded)
            ySpeed += Physics.gravity.y * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && m_Controller.isGrounded)
        {
            ySpeed = m_ActiveJumpSpeed;
        }
        m_ActiveVelocity = move;
        m_ActiveVelocity.y = ySpeed;
        m_Controller.Move(m_ActiveVelocity * Time.deltaTime);
        m_Animator.SetFloat("HorizontalSpeed", m_ActiveVelocity.x);
    }
}
