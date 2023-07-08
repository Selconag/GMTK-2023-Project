using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBox : Box
{
    /// <summary>
    /// List of boxes currently attached to this character box.
    /// </summary>
    public static List<IBoxes> attachedBoxes = new();

    /// <summary>
    /// Attaches a box to this character box.
    /// </summary>
    /// <param name="box">The box to attach.</param>
    public void AttachBox(IBoxes box)
    {
        if (box == null || attachedBoxes.Contains(box)) return;
        
        attachedBoxes.Add(box);
        box.onAttach();
    }
    //When boxes connect together, apply effect here
    public override void ApplyBoxEffect()
    {
       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("sgdsgsd")){
            AttachBox(other.GetComponent<IBoxes>()); 
        }
    }
}
