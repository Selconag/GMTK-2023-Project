using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterBox : Box
{
    /// <summary>
    /// List of boxes currently attached to this character box.
    /// </summary>

    [SerializeField] public List<IBoxes> attachedBoxes = new();
    
    /// <summary>
    /// Attaches a box to this character box.
    /// </summary>
    /// <param name="box">The box to attach.</param>
    public void AttachBox(IBoxes box)
    {
        if (box == null)
        {
            Debug.Log("Box is null");
            return;
        }
        
        attachedBoxes.Add(box);
        box.onAttach();
        Debug.Log("Amount of attached boxes: "+ attachedBoxes.Count);
    }
    public void DetachBox(IBoxes box)
    {
        attachedBoxes.Remove(box);
        box.onDetach();
        Debug.Log("Amount of attached boxes: " + attachedBoxes.Count);
    }
    //When boxes connect together, apply effect here
    public override void ApplyBoxEffect()
    {
       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CollectableBox")){
            AttachBox(other.GetComponent<IBoxes>());
            Debug.Log("We have attached"+ other.name.ToString()+ "with the Character Object!");
            // Calculate the position offset based on the direction from which the other object comes
            // Calculate the position offset based on the direction from which the other object comes
            Vector3 positionOffset = Vector3.zero;
            Vector3 characterSize = GetComponent<Collider>().bounds.size;
            Vector3 otherSize = other.bounds.size;

            Vector3 direction = other.transform.position - transform.position;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                // Attach on the left or right side
                positionOffset.x = (characterSize.x + otherSize.x) / 2f * Mathf.Sign(direction.x);
            }
            else
            {
                // Attach on the up or down side
                positionOffset.y = (characterSize.y + otherSize.y) / 2f * Mathf.Sign(direction.y);
            }

            // Snap the other object adjacent to the character object
            other.transform.position = transform.position + positionOffset;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CollectableBox"))
        {
            DetachBox(other.GetComponent<IBoxes>());
            Debug.Log("We have detached" + other.name.ToString() + "from the Character Object!");
            other.transform.parent = null;
        }
    }
}
