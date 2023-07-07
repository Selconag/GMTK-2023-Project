using System.Collections.Generic;
using UnityEngine;

public class ConnectableObject : Card
{
    public Transform ParentRef, ChildPoint;
    public Vector3 PosChange;
    public Transform ObjectPlace;
    private void Start()
    {
        if (Slot == null)
            StartPos = transform.position;
    }
}