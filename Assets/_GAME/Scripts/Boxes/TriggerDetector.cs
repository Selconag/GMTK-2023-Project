using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public string ActivatorTag;
    public List<Collider> TriggerList = new List<Collider>();
    public GameObject parentObject;
    public List<Collider> GetList()
    {
        return TriggerList;
    }

    public void ClearCollection()
    {
        TriggerList.Clear();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != ActivatorTag) return;
        AddCollidersRecursive(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag != ActivatorTag) return;
        RemoveCollidersRecursive(other);
    }

    private void AddCollidersRecursive(Collider collider)
    {
        if (collider != parentObject.GetComponent<Collider>() && !TriggerList.Contains(collider))
        {
            TriggerList.Add(collider);
        }

        // Recursively check for collisions in child colliders
        Collider[] childColliders = collider.GetComponentsInChildren<Collider>();
        foreach (Collider childCollider in childColliders)
        {
            if (childCollider != parentObject.GetComponent<Collider>() && !TriggerList.Contains(childCollider))
            {
                TriggerList.Add(childCollider);
            }
        }
    }

    private void RemoveCollidersRecursive(Collider collider)
    {
        if (TriggerList.Contains(collider))
        {
            TriggerList.Remove(collider);
        }

        // Recursively remove colliders from the list
        Collider[] childColliders = collider.GetComponentsInChildren<Collider>();
        foreach (Collider childCollider in childColliders)
        {
            if (TriggerList.Contains(childCollider))
            {
                TriggerList.Remove(childCollider);
            }
        }
    }
}
