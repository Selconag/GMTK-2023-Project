using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpZone : MonoBehaviour
{
    //Needs an animator to play animations
    public Animator Animator;
    //The particle system will be required too
    public ParticleSystem SellParticle;
    public float Timer, TimeUntilClose;
    private bool opened, closed;

    void Update()
    {
        if (opened || closed)
        {
            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
            }
            else
            {
                //1 Repair done
                if (opened)
                {
                    Timer = TimeUntilClose;
                    PlayClosing();
                    Debug.Log("Closed >:(");
                    opened = false;
                    closed = false;
                }

            }
        }

    }

    public void PlayOpeningOnce()
    {
        if (opened) return;
        opened = true;
        closed = false;
        Debug.Log("Dumping2");
        Animator.SetBool("StillOpen", true);
        Animator.Play("Opening");
    }

    public void PlayClosing()
    {
        if (closed) return;
        Animator.SetBool("StillOpen", false);
        closed = true;
        opened = false;
        Animator.Play("Closing");

    }

    public void PlayPartice()
    {
        SellParticle.Play();
    }

}
