using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Selocanus
{
    public class SaveSystems : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }


    [Serializable]
    public class PlayerSlot
    {
        public ObjectType Type;
        public int ObjectLevel = 0;
        public int SlotIndex;
        public int ObjectIndex;
    }

    [Serializable]
    public class FreeObjects
    {
        public ObjectType Type;
        public int ObjectLevel = 0;
        public Vector3 LastPosition;
        public int ObjectIndex;
    }
    [Serializable]
    public class CarStatus
    {
        public int ActiveCarIndex = -1;
        public float ActiveCarHealth = 0;
        public bool IsStartingCar = false;
    }
    //[Serializable]
    //public class BuyMenuStatus
    //{
    //    public List<int> BoughtCars;
    //}
    [Serializable]
    public class BoughtObjectsList
    {
        public List<BoughtObject> BoughtObjects;
    }
    [Serializable]
    public class BoughtObject
    {
        public ObjectType ObjectType;
    }
}

