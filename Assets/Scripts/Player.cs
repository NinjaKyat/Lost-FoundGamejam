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

        public void AddItem(Item item)
        {
            if (item != null)
                equippedItems.Add(item);
        }

        public Item RemoveItem()
        {
            if (equippedItems.Count > 0)
            {
                int lastItem = equippedItems.Count - 1;
                Item toReturn = equippedItems[lastItem];
                equippedItems.RemoveAt(lastItem);
                return toReturn;
            }
            else return null;

        }
    }
    
    Dictionary<Common.CharacterItemSlots, EquipmentSlot> CharacterEquipment = new Dictionary<Common.CharacterItemSlots,EquipmentSlot>();

    // Start is called before the first frame update
    void Start()
    {
        SetupPlayerStats();
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
        
    }
    
    public void AddItem(Common.CharacterItemSlots targetSlot, Item item)
    {
        EquipmentSlot slot = CharacterEquipment[targetSlot];
        slot.AddItem(item);
    }

    public Item RemoveItem(Common.CharacterItemSlots targetSlot)
    {
        EquipmentSlot slot = CharacterEquipment[targetSlot];
        return slot.RemoveItem();
    }
}
