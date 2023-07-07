using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject FOV;
    //public BasicPatrolRotate Patrol;
    //public PathMovement Path;
    //public MovingPlatform Movement;

    private Transform player = null;
    private Vector3 PlayerTarget;
    public float Speed = 0.5f;
    public float DistanceDifference = 0.5f;

    public Animator Animator;
    public string ActiveAnimationName;
    private bool AnimationSeal;

    //Clocknest Items
    //private void Start()
    //{
    //    EndZone.LevelEndInfo += DisableFOV;
    //}

    //private void OnDestroy()
    //{
    //    EndZone.LevelEndInfo -= DisableFOV;
    //}

    private void DisableFOV()
    {
        FOV.SetActive(false);
    }




    void Update()
    {
        if (player != null && transform.position != player.position)
        {
            AttackTheTarget(player.position);
        }
    }
    private void AttackTheTarget(Vector3 Target)
    {
        if ((Target - transform.position).magnitude <= DistanceDifference)
        {
            if (!AnimationSeal)
            {
                PlayAnimation = "Attack";
                Animator.SetBool("Attack", true);
                player = null;
            }
            return;
        }

        Target.y = 0;
        //transform.LookAt(Target);
        //transform.Translate(Target * Time.deltaTime * Speed);

        transform.position = Vector3.Lerp(transform.position, Target, Speed * Time.deltaTime);
    }

    public void SetPlayerTarget(Transform target)
    {
        //Clocknest Items
        //Patrol.StopPatrol();
        //Destroy(Patrol);
        //FOV.SetActive(false);
        //Destroy(Movement);
        //Destroy(Path);
        player = target;
        PlayerTarget = target.position;
        PlayerTarget.y = 0;
        transform.LookAt(PlayerTarget);
        PlayAnimation = "Running";
        Destroy(GetComponent<CapsuleCollider>());
    }

    //public void PlayAnimation(string AnimationName)
    //{
    //    Animator.Play(AnimationName, -1);
    //}
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
