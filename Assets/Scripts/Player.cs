using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Stats playerStats;

    class EquipmentSlot
    {
        private List<Item> equippedItems = new List<Item>();
        public Vector3 AdditionalItemOffset = Vector3.zero;
        private Vector3 OffsetToAdd = new Vector3(0,0.15f);
        public int maxItems;
        public Transform position;
        public bool flip;
        public void AddItem(Item item)
        {
            if (item != null)
            {
                equippedItems.Add(item);
                item.transform.parent = position;
                if (!flip)
                {
                    item.transform.localPosition = item.EquipOffset + AdditionalItemOffset;
                }
                else
                {
                    item.GetComponent<SpriteRenderer>().flipX = flip;
                    item.transform.localPosition = new Vector3(-item.EquipOffset.x, item.EquipOffset.y, item.EquipOffset.z) + AdditionalItemOffset; 
                }

                AdditionalItemOffset += OffsetToAdd;
            }
        }

        public bool CheckIfItemIsEquipped(Item item)
        {
            for (int i = 0; i < equippedItems.Count; i++)
            {
                if (equippedItems[i].name == item.name)
                    return true;
            }

            return false;
        }

        public int GetEquippedItemCount()
        {
            return equippedItems.Count;
        }
        public Item RemoveItem()
        {
            if (equippedItems.Count > 0)
            {
                int lastItem = equippedItems.Count - 1;
                Item toReturn = equippedItems[lastItem];
                toReturn.transform.parent = null;
                if (flip) toReturn.GetComponent<SpriteRenderer>().flipX = !flip;
                AdditionalItemOffset -= OffsetToAdd;
                equippedItems.RemoveAt(lastItem);
                return toReturn;
            }
            else return null;
        }
    }
    
    Dictionary<Common.CharacterItemSlots, EquipmentSlot> CharacterEquipment = new Dictionary<Common.CharacterItemSlots,EquipmentSlot>();
    public Transform[] equipmentSlotPositions;
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
        SetupPlayerEquipmentSlots();
        SetupPlayerStats();
    }

    void SetupPlayerEquipmentSlots()
    {
        for (int i = 0; i < equipmentSlotPositions.Length; i++)
        {
            EquipmentSlot slot = new EquipmentSlot();
            slot.position = equipmentSlotPositions[i];
            Common.CharacterItemSlots currentSlot = (Common.CharacterItemSlots) i;
            if (currentSlot == Common.CharacterItemSlots.Head)
                slot.maxItems = 99;
            else slot.maxItems = 1;
            if (currentSlot == Common.CharacterItemSlots.LeftHand)
                slot.flip = true;
            else slot.flip = false;
            CharacterEquipment[(Common.CharacterItemSlots) i] = slot;
        }

    }
    void SetupPlayerStats()
    {
        playerStats = new Stats();
        playerStats.AddStat(Stats.healthStat, 10);
        playerStats.AddStat(Stats.moveSpeedStat, 10);
    }
    
    void Update()
    {

    }
    
    public bool AddItem(Common.CharacterItemSlots targetSlot, Item item)
    {
        EquipmentSlot slot = CharacterEquipment[targetSlot];
        if (slot.GetEquippedItemCount() < slot.maxItems)
        {
            slot.AddItem(item);
            ApplyItemStats(item);
            return true;
        }
        else
        {
            Debug.Log($"Can't add item {item.name}. {slot.position.name}Slot is full");
            return false;
        }
    }

    public Item RemoveItem(Common.CharacterItemSlots targetSlot)
    {
        EquipmentSlot slot = CharacterEquipment[targetSlot];
        Item temp = slot.RemoveItem();
        RemoveItemStats(temp);
        return temp;
    }

    void ApplyItemStats(Item item)
    {
        if (item.AppliedStats == null)
            return;
        foreach(KeyValuePair<string, int> entry in item.AppliedStats)
        {
            playerStats.AddStat(entry.Key, entry.Value);
        }
    }

    void RemoveItemStats(Item item)
    {
        if (item.AppliedStats == null)
            return;
        foreach(KeyValuePair<string, int> entry in item.AppliedStats)
        {
            playerStats.AddStat(entry.Key, -entry.Value);
        }
    }

    public void OnClickItemEquip(Item item)
    {
        if (item.targetSlots.Length == 0)
            return;

        for (int i = 0; i < item.targetSlots.Length; i++)
        {
            if (TryToEquip(item.targetSlots[i], item))
                return;
        }
    }

    public Item OnClickItemUnequip(Common.CharacterItemSlots[] targetSlots, Item item)
    {
        for (int i = 0; i < targetSlots.Length; i++)
        {
            if (CharacterEquipment[targetSlots[i]].CheckIfItemIsEquipped(item))
                return RemoveItem(targetSlots[i]);
        }

        return null;
    }

    bool TryToEquip(Common.CharacterItemSlots targetSlot, Item item)
    {
        if (targetSlot == Common.CharacterItemSlots.Head)
        {
            if (AddItem(Common.CharacterItemSlots.Head, item))
                return true;
        }
        if (targetSlot == Common.CharacterItemSlots.RightHand)
        {
            if (AddItem(Common.CharacterItemSlots.RightHand, item))
                return true;
        }
        if (targetSlot == Common.CharacterItemSlots.LeftHand)
        {
            if (AddItem(Common.CharacterItemSlots.LeftHand, item))
                return true;
        }
        if (targetSlot == Common.CharacterItemSlots.Back)
        {
            if (AddItem(Common.CharacterItemSlots.Back, item))
                return true;
        }

        return false;
    }
}
