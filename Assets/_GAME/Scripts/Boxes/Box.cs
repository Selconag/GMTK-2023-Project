using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] public bool IsBreakable = true;
    [SerializeField] public bool IsMoveable = true;
    [SerializeField] public bool IsPhysicsEnabled = true;

    [Header("References")]
    [SerializeField] protected AnimationClip m_AnimClip;
    [SerializeField] protected AudioClip m_AudioClip;
    [SerializeField] protected ParticleSystem m_ParticleSystem;
    [SerializeField] protected Light m_ChangeLight;
    [SerializeField] protected SpriteRenderer m_Sprite;
    [SerializeField] protected Sprite m_NormalImage, m_ActivatedImage;

    //private void Start()
    //{
    //    m_Sprite.sprite = m_NormalImage;
    //}

    public virtual void ApplyBoxEffect()
    {

    }
}
