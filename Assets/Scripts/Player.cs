using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Stats playerStats;

    class EquipmentSlot
    {
        public List<Item> equippedItems = new List<Item>();
        public Vector3 AdditionalItemOffset = Vector3.zero;
        private Vector3 OffsetToAdd = new Vector3(0,0.1f);
        public int maxItems;
        public Transform position;
        public bool flip;
        public void AddItem(Item item)
        {
            if (item != null)
            {
                equippedItems.Add(item);
                item.transform.parent = position;
                item.transform.localEulerAngles = Vector3.zero;
                item.transform.localPosition = item.EquipOffset + AdditionalItemOffset;
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
                toReturn.transform.localEulerAngles = Vector3.zero;
                Vector3 localScale = toReturn.transform.localScale;
                if (localScale.x < 0)
                {
                    toReturn.transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
                    toReturn.GetComponent<SpriteRenderer>().flipX = false;
                }
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
        StartCoroutine(HungerLoop());
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
            CharacterEquipment[(Common.CharacterItemSlots) i] = slot;
        }

    }
    void SetupPlayerStats()
    {
        playerStats = new Stats();
        playerStats.SetStat(Stats.healthStat, 10);
        playerStats.SetStat(Stats.moveSpeedStat, 4);
        playerStats.SetStat(Stats.attackStat, 1);
        playerStats.SetStat(Stats.foodStat, 5);
    }

    IEnumerator HungerLoop()
    {
        while(playerStats.GetStat(Stats.healthStat) > 0)
        {
            yield return new WaitForSeconds(30);
            var currentFood = playerStats.GetStat(Stats.foodStat);
            if (currentFood > 0)
            {
                playerStats.AddStat(Stats.foodStat, -1);
            }
            else
            {
                playerStats.AddStat(Stats.healthStat, -1);
            }
        }
    }

    public List<Item> GetItemsInSlot(Common.CharacterItemSlots slot)
    {
        return CharacterEquipment[slot].equippedItems;
    }
    
    void Update()
    {
        var currentHealth = playerStats.GetStat(Stats.healthStat);
        if (currentHealth < 1)
        {
            Debug.Log("YOU ARE DEAD");
        }
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

        //Debug.Log($"Can't add item {item.name}. {slot.position.name}Slot is full");
        return false;
    }

    public Item RemoveItem(Common.CharacterItemSlots targetSlot)
    {
        EquipmentSlot slot = CharacterEquipment[targetSlot];
        Item temp = slot.RemoveItem();
        temp.equipped = false;
        temp.ItemDropped(this);
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

    public bool OnClickItemEquip(Item item)
    {
        if (item.targetSlots.Length == 0)
            return false;

        for (int i = 0; i < item.targetSlots.Length; i++)
        {
            if (TryToEquip(item.targetSlots[i], item))
                return true;
        }

        RemoveItem(item.targetSlots[0]);
        return OnClickItemEquip(item);
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
