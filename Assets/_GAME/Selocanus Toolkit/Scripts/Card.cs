using Selocanus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ObjectType { Type1, Type2, Type3, Type4 }
public class Card : MonoBehaviour
{
    public ObjectType CardType;
    public int ObjectLevel = 1;
    public float Price;
    public float Value;
    public ObjectSlot Slot;
    public Vector3 StartPos;
    public Collider Collider;
    public bool PickedUp = false;
    public Sprite ObjectIcon;
    public int ObjectIndex;

    private void Start()
    {
        if (Slot == null)
            StartPos = transform.position;
    }
}
