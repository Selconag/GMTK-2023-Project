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
    [SerializeField] protected ParticleSystem m_ParticleSystem;

    public virtual void ApplyBoxEffect()
    {

    }
}
