using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractibe
{
    public Common.CharacterItemSlots[] targetSlots;
    public Vector3 EquipOffset;
    private SpriteRenderer rend;
    private Player player;
    public bool equipped = false;
    public Dictionary<string, int> AppliedStats;
    public Transform Transform => transform;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        animations = FindObjectOfType<ItemAnimations>();
        rend = GetComponent<SpriteRenderer>();
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
                equipped = true;
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
        //animations.ItemDropped(player.transform.position);
    }
    
}
