using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    Stats playerStats;

    class EquipmentSlot
    {
        private List<Item> equippedItems = new List<Item>();
        public int maxItems;
        public Transform position;
        public void AddItem(Item item)
        {
            if (item != null)
            {
                equippedItems.Add(item);
                item.transform.parent = position;
                item.transform.localPosition = Vector3.zero; //Could set an offset here instead of 0
            }
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
            if ((Common.CharacterItemSlots) i == Common.CharacterItemSlots.Head)
                slot.maxItems = 99;
            else slot.maxItems = 1;
            CharacterEquipment[(Common.CharacterItemSlots) i] = slot;
        }

    }
    void SetupPlayerStats()
    {
        playerStats = new Stats();
        playerStats.AddStat(Stats.healthStat, 10);
        playerStats.AddStat(Stats.moveSpeedStat, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Item")
                {
                    hit.collider.GetComponent<Item>().OnClick();
                }
            }
        }
    }
    
    public bool AddItem(Common.CharacterItemSlots targetSlot, Item item)
    {
        EquipmentSlot slot = CharacterEquipment[targetSlot];
        if (slot.GetEquippedItemCount() < slot.maxItems)
        {
            slot.AddItem(item);
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
        return slot.RemoveItem();
    }

    public void OnClickItem(Item item)
    {
        if (item.targetSlots.Length == 0)
            return;

        for (int i = 0; i < item.targetSlots.Length; i++)
        {
            if (TryToEquip(item.targetSlots[i], item))
                return;
        }

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
