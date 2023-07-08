using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] protected bool m_IsBreakable = true;
    [SerializeField] protected bool m_IsMoveable = true;
    [SerializeField] protected bool m_IsPhysicsEnabled = true;

    [Header("References")]
    [SerializeField] protected AnimationClip m_AnimClip;
    [SerializeField] protected AudioClip m_AudioClip;
    [SerializeField] protected ParticleSystem m_ParticleSystem;
    [SerializeField] protected Light m_ChangeLight;

    public virtual void ApplyBoxEffect()
    {

    }
}
