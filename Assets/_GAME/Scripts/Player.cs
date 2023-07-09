using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Indicates whether the weapon is enabled for the player.
    /// </summary>
    [Header("Variables")]
    [SerializeField] bool m_WeaponEnabled;
    public float m_BaseHorizontalSpeed = 10f, m_ActiveHorizontalSpeed;
    public float m_BaseJumpSpeed = 10f, m_ActiveJumpSpeed;
    [SerializeField] Vector2 m_ActiveVelocity;
    [SerializeField] float m_ActiveRotation = -90;
    [SerializeField] bool m_InverseDirection = true;
    [SerializeField] AnimationCurve TurningCurve;
    bool m_CanRun = true;
    public bool CanJump, CanPull, CanSmash;
    float ySpeed;
    int activeStopAngle = 180;

    public Transform GroundCheck;
    public LayerMask GroundLayer;
    public float GroundDistance = 0.2f;

    /// <summary>
    /// Indicates whether the references are enabled for the player.
    /// </summary>
    [Header("References")]
    public bool m_References;
    [SerializeField] Transform m_Scanner;
    [SerializeField] public Animator Animator;
    [SerializeField] Camera m_Camera;

    /// <summary>
    /// transform of the weapon slot
    /// </summary>
    [SerializeField] Transform m_ObjectSlot;

    /// <summary>
    /// Character Controller component attached to the player.
    /// </summary>
    public CharacterController Controller;

    /// <summary>
    /// An instance for the Player class for Singleton.
    /// </summary>
    public static Player Instance;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_ActiveHorizontalSpeed = m_BaseHorizontalSpeed;
        m_ActiveJumpSpeed = m_BaseJumpSpeed;
        if (m_InverseDirection)
        {
            activeStopAngle = 180;

        }
        else
        {
            activeStopAngle = 0;
        }
    }

    /// <summary>
    /// Equips a game object by assigning its transform to the object slot when ItemBox attaches to CharacterBox.
    /// </summary>
    /// <param name="go">The game object to equip.</param>
    public void Equip(GameObject go)
    {
        go.SetActive(true);
        m_ObjectSlot = go.transform;
    }
    public void Unequip(GameObject go)
    {
        go.SetActive(false);
        m_ObjectSlot = null;    
    }
    void Update()
    {
        LookVertical();

        CheckForGround();

        GeneralMove();

        if (Input.GetKey(KeyCode.E)) ObjectInteraction();
    }

    public void LookVertical()
    {
        float lookDirection = Input.GetAxis("Vertical");
    }

    public void GeneralMove()
    {
        m_ActiveVelocity = new Vector2(-Input.GetAxis("Horizontal") * m_ActiveHorizontalSpeed, 0);

        m_ActiveRotation = Input.GetAxis("Horizontal") * 90 + 180;
        //m_ActiveRotation = TurningCurve.Evaluate(Mathf.Abs(Input.GetAxis("Horizontal"))) * 90;

        //m_ActiveRotation += Input.GetAxis("Horizontal") * 90;
        //m_ActiveRotation += TurningCurve.Evaluate(Mathf.Abs(Input.GetAxis("Horizontal")));

        Mathf.Clamp(m_ActiveRotation, -90, 90);
        if (m_InverseDirection) m_ActiveRotation *= -1;
        transform.rotation = Quaternion.Euler(0, m_ActiveRotation, 0);

        if (!Controller.isGrounded)
            ySpeed += Physics.gravity.y * Time.deltaTime;

        if (Controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            ySpeed = m_ActiveJumpSpeed;
            Animator.SetBool("IsOnGround", false);
            SetRunning(false);
            Debug.Log("Jump");
        }
        m_ActiveVelocity.y = ySpeed;
        if (m_CanRun)
            Controller.Move(m_ActiveVelocity * Time.deltaTime);
        Animator.SetFloat("HorizontalSpeed", Mathf.Abs(m_ActiveVelocity.x));
    }

    public void ObjectInteraction()
    {
        RaycastHit hit;
        //Search right
        if (Physics.Raycast(m_Scanner.transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Player")
            {

            }
            else return;
        }
        //Search left
        if (Physics.Raycast(m_Scanner.transform.position, transform.TransformDirection(Vector3.left), out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Player")
            {

            }
            else return;
        }
    }


    public void SetRunning(bool newVal)
    {
        m_CanRun = newVal;
        Animator.SetBool("CanRun", newVal);
    }

    private void CheckForGround()
    {
        if (!Controller.isGrounded) return;
        Animator.SetBool("IsOnGround", true);
        SetRunning(true);
    }

    bool IsPlayerGrounded()
    {
        // Perform a raycast from the groundCheck position downward
        RaycastHit hit;
        bool grounded = Physics.Raycast(GroundCheck.position, Vector3.down, out hit, GroundDistance, GroundLayer);

        // Return true if the raycast hits the ground
        return grounded;
    }
}
