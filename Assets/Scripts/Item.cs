using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractible
{
    public Common.CharacterItemSlots[] targetSlots;
    public Vector3 EquipOffset;
    private SpriteRenderer rend;
    public bool equipped = false;
    public Dictionary<string, int> AppliedStats = new Dictionary<string, int>();
    public Collider2D Collider => _collider;
    Collider2D _collider;
    private ItemAnimations animations;
    [System.Serializable]
    public struct ItemStats
    {
        public string statName;
        public int statValue;
    }
    
    public ItemStats[] itemStats;
    // Start is called before the first frame update
    void Start()
    {
        animations = GetComponent<ItemAnimations>();
        rend = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();

        for (int i = 0; i < itemStats.Length; i++)
        {
            AppliedStats[itemStats[i].statName] = itemStats[i].statValue;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact(Player interactingPlayer)
    {
        if (!equipped)
        {
            if (interactingPlayer.OnClickItemEquip(this))
            {
                equipped = true;
                ItemPickedUp();
            }
        }
        else
        {
            Item returned = interactingPlayer.OnClickItemUnequip(targetSlots, this);
            if (returned != null)
            {
                returned.equipped = false;
                returned.ItemDropped(interactingPlayer);
            }
        }
        if (interactingPlayer.transform.localScale.x >= 0)
            rend.flipX = false;
        else rend.flipX = true;
    }

    public void ItemDropped(Player interactingPlayer)
    {
        animations.ItemDropped(interactingPlayer.transform.position);
    }

    public void ItemPickedUp()
    {
        animations.PickedUp();
    }
    
}
