using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Common.CharacterItemSlots[] targetSlots;
    public Vector3 EquipOffset;
    private Player player;
    public bool equipped = false;
    public Dictionary<string, int> AppliedStats;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (!equipped)
        {
            equipped = true;
            player.OnClickItemEquip(this);
        }
        else
        {
            Item returned = player.OnClickItemUnequip(targetSlots, this);
            if (returned != null)
                returned.equipped = false;
        }
    }
    
}
