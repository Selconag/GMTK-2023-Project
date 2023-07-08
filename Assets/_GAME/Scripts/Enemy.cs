using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Enemy : MonoBehaviour
{
    [Header("Variables")]
    private Vector3 PlayerTarget;
    public float Speed = 0.5f;
    public float DistanceDifference = 0.5f;
    public string ActiveAnimationName;
    private bool AnimationSeal;
    public Vector2 PatrolDistance;
    public Vector2 StartingPos;
    private Vector3 m_NextTargetPos;

    [Header("References")]
    [SerializeField] private Transform m_Player = null;
    [SerializeField] private Transform m_Scanner;
    [SerializeField] Animator Animator;

    private void Start()
    {
        BasicPatrol();
    }


    void Update()
    {
        LookForPlayer();
    }

    public void BasicPatrol()
    {
        if (transform.position != m_NextTargetPos) return;
        //Do move between -PatrolDistance.X and PatrolDistance.X based on StartingPoint


    }

    public void LookForPlayer()
    {
        RaycastHit hit;
        //Search right
        if (Physics.Raycast(m_Scanner.transform.position, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Player")
            {
                AttackTheTarget(hit.transform);
            }
            else return;
        }
        //Search left
        if (Physics.Raycast(m_Scanner.transform.position, transform.TransformDirection(Vector3.left), out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Player")
            {
                AttackTheTarget(hit.transform);
            }
            else return;
        }
    }


    private void AttackTheTarget(Transform Target)
    {
        Debug.Log("Enemy Found");
        transform.position = Vector3.Lerp(transform.position, new Vector3(Target.position.x, transform.position.y, 0), Speed * Time.deltaTime);
    }

    public void SetPlayerTarget(Transform target)
    {
        m_Player = target;
        PlayerTarget = target.position;
        PlayerTarget.y = 0;
        transform.LookAt(PlayerTarget);
        PlayAnimation = "Running";
        Destroy(GetComponent<CapsuleCollider>());
    }
    public string PlayAnimation
    {
        get
        {
            return ActiveAnimationName;
        }
        set
        {
            ActiveAnimationName = value;
            Animator.Play(value);
        }
    }
}
