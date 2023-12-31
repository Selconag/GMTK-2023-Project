using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Player : MonoBehaviour
{

    /// <summary>
    /// Indicates whether the weapon is enabled for the player.
    /// </summary>
    [Header("Variables")]
    [SerializeField] bool m_WeaponEnabled;
    public float m_BaseHorizontalSpeed = 10f, m_ActiveHorizontalSpeed;
    public float m_BaseJumpSpeed = 10f, m_ActiveJumpSpeed;
    [SerializeField] Vector3 m_ActiveVelocity;
    [SerializeField] float m_ActiveRotation = -90;
    [SerializeField] bool m_InverseDirection = true;
    [SerializeField] AnimationCurve TurningCurve;
    bool m_CanRun = true;
    public bool CanJump, CanPull, CanSmash;
    float ySpeed;
    int activeStopAngle = 180;
    public float ClimbCoefficient = 100;
    public int PushForceMultiplier = 100;
    public bool IsWeaponEnabled;
	public float JumpCoefficient = 1;
    public Transform GroundCheck;
    public LayerMask GroundLayer;
    public float GroundDistance = 0.0f;

    private int m_CollisionCount = 0;
    public bool IsColliding { get { return m_CollisionCount > 0; } }
    /// <summary>
    /// Indicates whether the references are enabled for the player.
    /// </summary>
    [Header("References")]
    public bool m_References;
    [SerializeField] Transform m_Scanner;
    [SerializeField] public Animator Animator;
    [SerializeField] Camera m_Camera;
    [SerializeField] Rigidbody m_Rigid;

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
        this.Animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        m_ActiveHorizontalSpeed = m_BaseHorizontalSpeed;
        m_ActiveJumpSpeed = m_BaseJumpSpeed * JumpCoefficient;
        if (m_InverseDirection)
        {
            activeStopAngle = 180;

        }
        else
        {
            activeStopAngle = 0;
        }
    }

    //Wall Climb system
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Box" || other.transform.tag == "Door")
            Climb();
    }

    private void OnCollisionEnter(Collision collision)
    {
   		m_CollisionCount++;
        if(IsWeaponEnabled)
            if(collision.collider.tag == "Enemy")
            {
                Animator.Play("Attack1");
                Destroy(collision.collider.gameObject);
                
            }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
            ObjectInteraction();
        }
        else
        {
            if (collision.collider.transform.tag == "Box" || collision.collider.transform.tag == "Door")
                Climb();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        m_Rigid.useGravity = true;
        m_CollisionCount--;
    }

    public void Climb()
    {

        m_ActiveVelocity = new Vector2(0, Input.GetAxis("Vertical"));
        if(m_ActiveVelocity.magnitude > 0)
        {
            m_Rigid.useGravity = false;
            Debug.Log(m_ActiveVelocity.y);
            //Controller.Move(m_ActiveVelocity * Time.deltaTime);
            m_Rigid.MovePosition(transform.position + m_ActiveVelocity * Time.deltaTime * m_BaseJumpSpeed * ClimbCoefficient);
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

        //if (!Controller.isGrounded)
        //    ySpeed += Physics.gravity.y * Time.deltaTime;

        //if (Controller.isGrounded && Input.GetButtonDown("Jump"))
        //{
        //    ySpeed = m_ActiveJumpSpeed;
        //    Animator.SetBool("IsOnGround", false);
        //    SetRunning(false);
        //    Debug.Log("Jump");
        //}

            if (Input.GetButtonDown("Jump"))
            {

                    ySpeed = m_ActiveJumpSpeed;
                    Vector3 jumpForce = new Vector3(0f, ySpeed, 0f);
                    m_Rigid.AddForce(jumpForce, ForceMode.VelocityChange);
                    Animator.SetBool("IsOnGround", false);
                    SetRunning(false);
                    Debug.Log("Jump");
            }

        //m_ActiveVelocity.y = ySpeed;
        if (m_CanRun)
            //Controller.Move(m_ActiveVelocity * Time.deltaTime);
        m_Rigid.MovePosition(transform.position + m_ActiveVelocity * Time.deltaTime);
        Animator.SetFloat("HorizontalSpeed", Mathf.Abs(m_ActiveVelocity.x));
    }

    public void ObjectInteraction()
    {
        RaycastHit hit;
        //Search rightw
        if (Physics.Raycast(m_Scanner.transform.position, transform.TransformDirection(Vector3.forward), out hit, 1f))
        {
            if (hit.transform.tag == "Box" && hit.transform.GetComponent<Box>().IsMoveable)
            {
                ObjectPush(hit.transform);
            }
            else return;
        }
    }
    //Apply force in the moving direction if we are still pressing the button
    public void ObjectPull()
    {

    }

    //Apply force in the moving direction if we are still colliding
    public void ObjectPush(Transform targetObject)
    {
        Rigidbody targetRigid = targetObject.GetComponent<Rigidbody>();
        Vector3 forceDirection = targetObject.position - transform.position;
        forceDirection.Normalize();
        targetRigid.AddForceAtPosition(forceDirection * PushForceMultiplier, transform.position, ForceMode.Impulse);
    }

    public void SetRunning(bool newVal)
    {
        m_CanRun = newVal;
        Animator.SetBool("CanRun", newVal);
    }

    private void CheckForGround()
    {
        //if (!Controller.isGrounded) return;
        Animator.SetBool("IsOnGround", true);
        SetRunning(true);
    }

    public float GroundCheckRadius;
    bool IsOnGround(out Vector3 groundNormal)
    {
        groundNormal = Vector3.zero;

        if (!IsColliding) return false; // If we're not colliding with anything, we're not grounded.

        // Physics "contacts" can actually have some minor separation, so let's add a margin.
        float margin = Physics.defaultContactOffset;

        // Start your check from a little above the bottom of your capsule.
        // I like to set up my colliders so "foot position" is always transform.position,
        // but your setup may vary. I like to make my ground check radius 1 margin smaller
        // than my capsule radius, to make sure my check never starts already in-contact
        // with an obstacle.
        Vector3 checkOrigin = GroundCheck.position
                            + new Vector3(0, GroundCheckRadius + margin, 0);

        // Check for a collision under our feet.
        if (Physics.SphereCast(
                 checkOrigin,        // Start just above the bottom of our capsule,
                 GroundCheckRadius,  // with a sphere slightly smaller than our capsule.
                 Vector3.down,       // Fire it downwards,
                 out RaycastHit hit, // and record what it hits,
                 2 * margin,         // going just far enough to catch a touching contact,
                 GroundLayer        // with anything marked "ground".
        ) == false) return false;    // If we didn't hit anything, we're not grounded.

       

        // Otherwise, we are grounded. Extract the ground normal and return true.
        groundNormal = hit.normal;
        return true;
    }


    bool IsPlayerGrounded()
    {
        // Perform a raycast from the groundCheck position downward
        RaycastHit hit;
        bool grounded = Physics.Raycast(GroundCheck.position, Vector3.down, out hit, GroundDistance, GroundLayer);

        if (grounded)
        {
            Debug.Log("You are grounded");
        }
        else
        {
            Debug.Log("Your Ground Distance:" + GroundDistance);
            Debug.Log("You are in the air");
        }

        // Return true if the raycast hits the ground
        return grounded;
    }

    public void ImmediateJump()
    {
        m_ActiveJumpSpeed = m_BaseJumpSpeed * 5;
        Vector3 jumpForce = new Vector3(0f, m_ActiveJumpSpeed, 0f);
        m_Rigid.AddForce(jumpForce, ForceMode.VelocityChange);
    }
}
