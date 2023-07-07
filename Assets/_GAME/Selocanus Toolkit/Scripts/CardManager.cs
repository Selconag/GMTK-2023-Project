using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Selocanus
{
    public class CardManager : MonoBehaviour
    {
        [Header("References")]
        public Transform SpawnZone;
        public ParticleSystem MergeParticle;
        //Implement pool system here

        [Header("Variables")]
        public float SpawnRangeX, SpawnRangeZ;
        public int IncreasePriceGapPerTimes = 6;
        public int ActiveObjectIndex = 0;
        public float MaxMergingScale, MergingScaleTimer;
        public int ActiveLoopAmount;

        [Header("Object Lists")]
        public List<GameObject> Wheels, Workers;
        public List<ObjectSlot> SpawnSlots;
        public List<Card> ActiveObjects = null;
        public List<FreeObjects> FreeObjects;

        //private void Start()
        //{
        //    InsertObjectsToSlots();
        //    InsertObjectsToFreeWorld();
        //}


        #region Adding
        public bool AddNewCard(ObjectType type)
        {
            //Get Spawn Position
            //Spawn object
            Card Object;
            switch (type)
            {
                case ObjectType.Type4:
                    Object = Instantiate(Workers[0]).GetComponent<Card>();
                    Object.transform.position = SpawnZone.position;
                    Object.transform.SetParent(SpawnZone);
                    Object.transform.localPosition = new Vector3(Random.Range(-SpawnRangeX, SpawnRangeX), 0F, Random.Range(-SpawnRangeZ, SpawnRangeZ));
                    Object.transform.parent = null;
                    Object.StartPos = transform.position;
                    //AddToActiveObjects(Object);
                    //PoolManager.Instance.CallSpawnPool(Object.transform.position);
                    break;
            }
            return true;
        }

        public bool AddNewCard(GameObject Object)
        {
            //Get Spawn Position
            //Spawn object
            Object = Instantiate(Object);
            Object.transform.position = SpawnZone.position;
            Object.transform.SetParent(SpawnZone);
            Object.transform.localPosition = new Vector3(Random.Range(-SpawnRangeX, SpawnRangeX), 0F, Random.Range(-SpawnRangeZ, SpawnRangeZ));
            Object.transform.parent = null;
            Object.GetComponent<Card>().StartPos = transform.position;
            Object.tag = "Card";
            Object.name = "Bought Object";
            //AddToActiveObjects(Object.GetComponent<Card>());
            return true;
        }


        #endregion

        #region Removing

        public void ClearAllSlots()
        {
            //FOR SAVE SYSTEM SLOTS
            ClearSlotObjects();
            //ProgressManager.Instance.SaveSlots();
        }

        private void ClearSlotObjects()
        {
            //if (!TutorialSystem.Instance.TutorialCompleted) return;
            foreach (var item in SpawnSlots)
            {
                //if (item.SlotType == CardType.Worker && item.SlotObject != null)
                //{
                //    item.SlotObject.GetComponent<Worker>().RemoveSignatures();
                //}
                if (item.SlotObject != null)
                {
                    Transform temp = item.SlotObject.transform;
                    Destroy(item.SlotObject);
                    item.SlotObject = null;
                    if (temp != null) Destroy(temp.gameObject);
                }
                item.ObjectLevel = 0;
                item.SlotIsEmpty = true;
            }

            List<PlayerSlot> slots = new List<PlayerSlot>();
            //Card card;
            foreach (var item in SpawnSlots)
            {
                //Check if object is available
                if (!item.SlotIsEmpty && item.ObjectLevel > -1)
                {
                    PlayerSlot newSlot = new PlayerSlot();
                    newSlot.Type = item.SlotType;
                    newSlot.ObjectLevel = 0;
                    newSlot.SlotIndex = item.SlotIndex;
                    newSlot.ObjectIndex = item.SlotObject.GetComponent<Card>().ObjectIndex;
                    //if (item.SlotType) continue;

                    slots.Add(newSlot);
                }
            }
            //ProgressManager.Instance.SaveSlots(slots);
            Debug.Log("Done Saving Slots");
        }

        #endregion

        #region Merging & Others

        public bool CheckIfNextLevelAvailable(Card card, int activeLevel)
        {
            switch (card.CardType)
            {
                case ObjectType.Type1:
                    if (Wheels.Count >= activeLevel + 1)
                        return true;
                    break;
                //case CardType.Paint:
                //    if (Modfixes.Count >= activeLevel + 1)
                //        return true;
                //    break;
                //case CardType.ModFix:
                //    if (Colors.Count >= activeLevel + 1)
                //        return true;
                //    break;
                case ObjectType.Type4:
                    if (Workers.Count >= activeLevel + 1)
                        return true;
                    break;
            }
            return false;
        }

        public bool MergeCards(Card source, Card target)
        {
            if (source.CardType == target.CardType && source.ObjectLevel == target.ObjectLevel)
            {
                if (CheckIfNextLevelAvailable(source, source.ObjectLevel))
                {
                    if (source.CardType == ObjectType.Type1 || source.CardType == ObjectType.Type1)
                    {
                        Debug.Log("Enters");
                        Vector3 spawnPosition = target.transform.position;
                        int newLevel = target.ObjectLevel + 1;
                        if (source.Slot != null)
                            source.Slot.SlotIsEmpty = true;
                        //RemoveFromActiveObjects(source);
                        Destroy(source.gameObject);
                        var tempSlot = target.Slot;
                        //If target's slot is null
                        if (tempSlot == null)
                        {
                            Vector3 tempPos = target.transform.position;
                            //RemoveFromActiveObjects(target);
                            Destroy(target.gameObject);
                            //var spawnIndex = CheckSpawnPlaces();

                            if (source.ObjectLevel > 0)
                            {
                                switch (target.CardType)
                                {
                                    case ObjectType.Type1:
                                        target = Instantiate(Wheels[newLevel - 1]).GetComponent<Card>();
                                        break;
                                    case ObjectType.Type4:
                                        target = Instantiate(Workers[newLevel - 1]).GetComponent<Card>();
                                        break;
                                }
                                target.transform.position = tempPos;
                                target.StartPos = tempPos;
                                //target.transform.DOScale(Vector3.one * MaxMergingScale, MergingScaleTimer).OnComplete(() => target.transform.DOScale(Vector3.one, MergingScaleTimer));
                                //Destroy(Instantiate(MergeParticle, target.transform), 3);
                                //HapticManager.Instance.HapticOnMergeEvent();
                                //PoolManager.Instance.CallMergePool(target.transform.position);
                                //AddToActiveObjects(target);
                                //SaveSlotObjects();
                            }
                            return true;

                        }
                        //If target has a slot
                        else
                        {
                            //RemoveFromActiveObjects(target);
                            Destroy(target.gameObject);
                            //var spawnIndex = CheckSpawnPlaces();
                            var spawnIndex = target.Slot.SlotIndex;
                            if (source.ObjectLevel > 0)
                            {
                                switch (target.CardType)
                                {
                                    case ObjectType.Type1:
                                        target = Instantiate(Wheels[newLevel - 1]).GetComponent<Card>();
                                        break;
                                    case ObjectType.Type4:
                                        target = Instantiate(Workers[newLevel - 1]).GetComponent<Card>();
                                        break;
                                }
                                //Clear source slot
                                if (source.Slot != null)
                                {
                                    source.Slot.SlotIsEmpty = true;
                                    source.Slot.SlotObject = null;
                                    source.Slot.ObjectLevel = 0;
                                    source.Slot = null;
                                }

                                SpawnSlots[spawnIndex].ObjectLevel = -1;

                                target.transform.position = tempSlot.SlotTransform.position;
                                target.StartPos = SpawnSlots[spawnIndex].SlotTransform.position;
                                target.Slot = tempSlot;

                                tempSlot.SlotObject = target.gameObject;
                                target.Slot.ObjectLevel = target.ObjectLevel;
                                target.transform.SetParent(tempSlot.SlotTransform);
                                //target.transform.DOScale(Vector3.one * MaxMergingScale, MergingScaleTimer).OnComplete(() => target.transform.DOScale(Vector3.one, MergingScaleTimer));
                                //Destroy(Instantiate(MergeParticle, target.transform), 3);
                                //HapticManager.Instance.HapticOnMergeEvent();
                                //PoolManager.Instance.CallMergePool(target.transform.position);
                                //AddToActiveObjects(target);
                            }
                            //SaveSlotObjects();
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            //Not same types, return false
            return false;
        }

        //TO DO Add functionality to connecting cards
        //Required functionality
        //Store child infos, when added to object slot, activate all child objects abilities and values
        public bool ConnectCards(ConnectableObject source, ConnectableObject target, bool saveStatus = true)
        {
            //Basically connect source to target
            source.ParentRef = target.transform;
            source.transform.SetParent(target.ChildPoint.transform);

            //Only target will be available to move and source is docked to target
            source.transform.localPosition = Vector3.zero;
            source.transform.localEulerAngles = Vector3.zero;
            source.transform.localPosition += target.PosChange;


            //destroy source collider from now on
            Destroy(source.Collider);

            source.tag = "Stacked";
            //PoolManager.Instance.CallItemEffectPool(target.transform.position);
            //RemoveFromActiveObjects(source);
            //if (saveStatus)
            //    SaveSlotObjects();
            return true;
        }

        public void SwapCards(Card source, Card target)
        {
            //Basically swap source with target
            //Save index
            var SlotIndex = target.Slot.SlotIndex;
            //Send start pos info
            target.StartPos = source.StartPos;
            target.transform.SetParent(null);
            source.transform.SetParent(target.Slot.SlotTransform);
            //Update slot infos of objects
            source.Slot = target.Slot;
            target.Slot = null;
            source.Slot.SlotObject = source.gameObject;
            source.Slot.ObjectLevel = source.ObjectLevel;
            source.Slot.GetComponent<Collider>().enabled = false;
            //Change places of objects
            target.transform.position = target.StartPos;
            source.transform.localPosition = Vector3.zero;
            //target.transform.DOScale(Vector3.one * MaxMergingScale, MergingScaleTimer).OnComplete(() => target.transform.DOScale(Vector3.one, MergingScaleTimer));
            //HapticManager.Instance.HapticOnItemSwap();
            //RemoveFromActiveObjects(source);
            //AddToActiveObjects(target);
            //SaveSlotObjects();
            //SaveFreeObjects();
        }

        public void SwapCardsInSlots(Card source, Card target)
        {
            //Basically swap source with target
            //Save index
            var TargetPos = target.StartPos;
            var SourcePos = source.StartPos;
            var TargetSlot = target.Slot;
            var SourceSlot = source.Slot;


            //Send source to target
            source.StartPos = TargetPos;
            source.transform.SetParent(TargetSlot.SlotTransform);
            source.Slot = TargetSlot;
            source.Slot.SlotObject = source.gameObject;
            source.transform.localPosition = Vector3.zero;
            source.Slot.ObjectLevel = source.ObjectLevel;

            //Send target to source
            target.StartPos = SourcePos;
            target.transform.SetParent(SourceSlot.SlotTransform);
            target.Slot = SourceSlot;
            target.Slot.SlotObject = target.gameObject;
            target.transform.localPosition = Vector3.zero;
            target.Slot.ObjectLevel = target.ObjectLevel;

            //target.transform.DOScale(Vector3.one * MaxMergingScale, MergingScaleTimer).OnComplete(() => target.transform.DOScale(Vector3.one, MergingScaleTimer));
            //HapticManager.Instance.HapticOnItemSwap();
            //SaveSlotObjects();
        }


        public int CheckSpawnPlaces()
        {
            for (int i = 0; i < SpawnSlots.Count; i++)
            {
                if (SpawnSlots[i].SlotIsEmpty)
                {
                    return i;
                }
            }
            return 9;
        }

        public int CalculateNextLevelPrice()
        {
            int Coefficient = ActiveObjectIndex / IncreasePriceGapPerTimes;
            int Remains = ActiveObjectIndex % IncreasePriceGapPerTimes;
            int basePrice = 10;
            int activePrice = basePrice;
            for (int i = 0; i < Coefficient; i++)
            {
                activePrice = basePrice * IncreasePriceGapPerTimes;
                basePrice += basePrice;
            }
            activePrice += Remains * basePrice;
            return activePrice;
        }

        #endregion

        ////WARNING: SAVE SYSTEMS REQUIRE CLOCKNEST GAME KIT, SO THEREFORE THIS SYSTEM CAN NOT WORK STANDALONE, FOR NOW.
        //#region Save Systems
        //public void InsertObjectsToFreeWorld()
        //{
        //    if (ProgressManager.Instance.GetFreeObjects().Count <= 0) return;
        //    var freeObjects = ProgressManager.Instance.GetFreeObjects();
        //    if (freeObjects.Count > 0)
        //    {
        //        foreach (var obj in freeObjects)
        //        {
        //            //Check if objects are correct
        //            if (obj.ObjectLevel > -1)
        //            {
        //                Card card = new Card();
        //                //Spawn object
        //                //Place object to appropriate place
        //                if (obj.Type == ObjectType.Type4)
        //                {
        //                    card = Instantiate(BuyMenuManager.Instance.WorkersList[obj.ObjectLevel - 1]).GetComponent<Card>();
        //                }
        //                else
        //                {
        //                    if (obj.Type == ObjectType.Type3)
        //                    {
        //                        card = Instantiate(BuyMenuManager.Instance.GetPartMod(obj.ModType)).GetComponent<Card>();

        //                    }
        //                    else
        //                    {
        //                        card = Instantiate(BuyMenuManager.Instance.FindGameObject(obj.ObjectIndex)).GetComponent<Card>();
        //                    }
        //                }
        //                card.transform.position = obj.LastPosition;
        //                card.StartPos = transform.position;
        //                AddToActiveObjects(card);
        //            }
        //        }
        //    }
        //}

        //public void SaveFreeObjects()
        //{
        //    ProgressManager.Instance.SaveFreeObjects(ActiveObjects);
        //    Debug.Log("Done Saving Slots");
        //}

        //public void AddToActiveObjects(Card card)
        //{
        //    if (ActiveObjects == null)
        //        ActiveObjects = new List<Card>();
        //    ActiveObjects.Add(card);
        //    SaveFreeObjects();
        //}

        //public void RemoveFromActiveObjects(Card card)
        //{
        //    if (ActiveObjects != null && ActiveObjects.Count > 0)
        //    {
        //        if (ActiveObjects.Contains(card))
        //            ActiveObjects.Remove(card);
        //        SaveFreeObjects();
        //    }
        //}


        //public void InsertObjectsToSlots()
        //{
        //    if (ProgressManager.Instance.GetSlots().Count <= 0) return;
        //    var newSlots = ProgressManager.Instance.GetSlots();
        //    if (newSlots.Count > 0)
        //    {
        //        foreach (var slot in newSlots)
        //        {
        //            //Check if objects are correct
        //            if (slot.ObjectLevel > -1)
        //            {
        //                Card card = new Card();
        //                //Spawn object
        //                //Place object to appropriate place
        //                if (slot.Type == ObjectType.Type4)
        //                {
        //                    card = Instantiate(BuyMenuManager.Instance.WorkersList[slot.ObjectLevel - 1]).GetComponent<Card>();
        //                }
        //                else
        //                {
        //                    if (slot.Type == ObjectType.Type3)
        //                    {
        //                        card = Instantiate(BuyMenuManager.Instance.GetPartMod(slot.ModType)).GetComponent<Card>();

        //                    }
        //                    else
        //                    {
        //                        card = Instantiate(BuyMenuManager.Instance.FindGameObject(slot.ObjectIndex)).GetComponent<Card>();
        //                    }
        //                }
        //                //Set its variables
        //                SpawnSlots[slot.SlotIndex].SlotObject = card.gameObject;
        //                SpawnSlots[slot.SlotIndex].ObjectLevel = card.ObjectLevel;
        //                SpawnSlots[slot.SlotIndex].SlotIsEmpty = false;
        //                card.transform.position = SpawnSlots[slot.SlotIndex].SlotTransform.position;
        //                card.transform.SetParent(SpawnSlots[slot.SlotIndex].SlotTransform);
        //                card.Slot = SpawnSlots[slot.SlotIndex];
        //                card.StartPos = SpawnSlots[slot.SlotIndex].SlotTransform.position;
        //            }
        //        }
        //        SaveSlotObjects();
        //    }

        //}


        //public void SaveSlotObjects()
        //{
        //    //if (!TutorialSystem.Instance.TutorialCompleted) return;

        //    List<PlayerSlot> slots = new List<PlayerSlot>();
        //    //Card card;
        //    foreach (var item in SpawnSlots)
        //    {
        //        //Check if object is available
        //        if (!item.SlotIsEmpty && item.ObjectLevel > -1)
        //        {
        //            PlayerSlot newSlot = new PlayerSlot();
        //            newSlot.Type = item.SlotType;
        //            newSlot.ObjectLevel = item.ObjectLevel;
        //            newSlot.SlotIndex = item.SlotIndex;
        //            newSlot.ObjectIndex = item.SlotObject.GetComponent<Card>().ObjectIndex;
        //            //item.SlotObject.StartPos = transform.position;
        //            slots.Add(newSlot);
        //        }
        //    }
        //    ProgressManager.Instance.SaveSlots(slots);
        //    Debug.Log("Done Saving Slots");
        //}
        //#endregion
    }
}

