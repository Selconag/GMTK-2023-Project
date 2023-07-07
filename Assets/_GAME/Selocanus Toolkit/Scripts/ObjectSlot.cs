using Selocanus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Selocanus
{
    public class ObjectSlot : MonoBehaviour
    {
        public Transform SlotTransform;
        public GameObject SlotObject;
        public ObjectType SlotType;
        public int ObjectLevel = 0;
        public int SlotIndex;
        public bool SlotIsEmpty = true;
    }
}

