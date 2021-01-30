using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractible
{
    public Common.CharacterItemSlots[] targetSlots;
    public Vector3 EquipOffset;
    private SpriteRenderer rend;
    private Player player;
    public bool equipped = false;
    public Dictionary<string, int> AppliedStats;
    public Collider2D Collider => collider;
    Collider2D collider;
    private ItemAnimations animations;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        animations = GetComponent<ItemAnimations>();
        rend = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        if (!equipped)
        {
            if (player.OnClickItemEquip(this))
            {
                equipped = true;
                ItemPickedUp();
            }
        }
        else
        {
            Item returned = player.OnClickItemUnequip(targetSlots, this);
            if (returned != null)
            {
                returned.equipped = false;
                returned.ItemDropped();
            }
        }
        if (player.transform.localScale.x >= 0)
            rend.flipX = false;
        else rend.flipX = true;
    }

    public void ItemDropped()
    {
        animations.ItemDropped(player.transform.position);
    }

    public void ItemPickedUp()
    {
        animations.PickedUp();
    }
    
}
