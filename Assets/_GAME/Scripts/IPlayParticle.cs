using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayParticle
{
    public void PlayParticleEffect(ParticleSystem targetParticle)
    {
        targetParticle.gameObject.SetActive(true);
        targetParticle.Play();
    }
}
