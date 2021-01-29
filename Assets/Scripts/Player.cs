using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Stats playerStats;

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
    
    public void AddItem(Common.CharacterItemSlots slot, Item item)
    {
        
    }

    public void RemoveItem(Item item)
    {
        
    }
}
