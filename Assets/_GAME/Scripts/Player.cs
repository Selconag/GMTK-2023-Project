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
    [SerializeField] float m_ActiveRotation = -90;
    [SerializeField] bool m_InverseDirection = true;
    [SerializeField] AnimationCurve TurningCurve;
    bool m_CanRun = true;

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
        if (m_Controller.isGrounded)
        {
            m_Animator.ResetTrigger("Jump");
            m_Animator.SetBool("IsOnGround", true);
            SetRunning(true);
        }

        Vector2 move = new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        m_ActiveRotation = Input.GetAxis("Horizontal") * 90;
        //m_ActiveRotation = TurningCurve.Evaluate(Mathf.Abs(Input.GetAxis("Horizontal"))) * 90;

        //m_ActiveRotation += Input.GetAxis("Horizontal") * 90;
        //m_ActiveRotation += TurningCurve.Evaluate(Mathf.Abs(Input.GetAxis("Horizontal")));

        Mathf.Clamp(m_ActiveRotation, -90, 90);
        if (m_InverseDirection) m_ActiveRotation *= -1;
        transform.rotation = Quaternion.Euler(0,m_ActiveRotation,0);

        if(!m_Controller.isGrounded)
            ySpeed += Physics.gravity.y * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && m_Controller.isGrounded)
        {
            ySpeed = m_ActiveJumpSpeed;
            m_Animator.SetTrigger("Jump");
            m_Animator.SetBool("IsOnGround", false);
            SetRunning(false);
        }

        m_ActiveVelocity = move;
        m_ActiveVelocity.y = ySpeed;
        if(m_CanRun)
            m_Controller.Move(m_ActiveVelocity * Time.deltaTime);
        m_Animator.SetFloat("HorizontalSpeed", Mathf.Abs(m_ActiveVelocity.x));
    }

    public void SetRunning(bool newVal)
    {
        m_CanRun = newVal;
        m_Animator.SetBool("CanRun", newVal);
    }
}
