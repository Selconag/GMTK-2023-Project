using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] protected bool m_IsBreakable = true;
    [SerializeField] protected bool m_IsMoveable = true;
    [SerializeField] protected bool m_IsPhysicsEnabled = true;

    public virtual void ApplyBoxEffect()
    {

    }
}
