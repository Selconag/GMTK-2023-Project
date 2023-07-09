using SelocanusToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UselessBox : Box, IPlayBoxEvents
{
    private void OnTriggerEnter(Collider other)
    {
        PlayParticleEffect();
        PlaySoundEffect();
        PlayAnimationEffect();
        PlayLightEffect();
    }


    public void PlayParticleEffect()
    {
        if (m_ParticleSystem == null) return;
        m_ParticleSystem.gameObject.SetActive(true);
        m_ParticleSystem.Play();
    }

    public void PlaySoundEffect()
    {
        if (m_AudioClip == null) return;
        AudioManager.Instance.PlayCustomSoundSound(m_AudioClip);
    }

    public void PlayAnimationEffect()
    {
        if (m_AnimClip == null) return;
        Player.Instance.Animator.Play(m_AnimClip.ToString());
    }

    public void PlayLightEffect()
    {
        if (m_ChangeLight)
        {
            m_ChangeLight.intensity = 0.5f;
        }

    }
}
