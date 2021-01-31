using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Plane : MonoBehaviour
{
    private bool hasPropeller = false;
    private bool hasEngine = false;
    private bool hasWing = false;
    private bool hasWheel = false;

    public Transform PropellerLocation;
    public Transform EngineLocation;
    public Transform WingLocation;
    public Transform WheelLocation;

    private bool winnedTheGame = false;
    private Transform brokedPlane;
    private Transform fixedPlane;
    
    // Start is called before the first frame update
    void Start()
    {
        brokedPlane = transform.GetChild(0);
        fixedPlane = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPropeller && hasEngine && hasWing && hasWheel)
            if (!winnedTheGame)
            {
                WinTheGame();
                winnedTheGame = true;
            }
    }

    void WinTheGame()
    {
        EventUI.instance.DisplayDialog("You win!", "You found a way to escape the jungle!", "win", null);
        brokedPlane.gameObject.SetActive(false);
        fixedPlane.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null)
            return;
        
        List<Item> items = player.GetItemsInSlot(Common.CharacterItemSlots.Back);
        if (items.Count < 1)
            return;
        
        List<Item> itemsCopy = new List<Item>();
        
        for (int i = 0; i < items.Count; i++)
        {
            itemsCopy.Add(items[i]);
        }
        
        player.RemoveItem(Common.CharacterItemSlots.Back);
        if (itemsCopy[0].name.Contains("Engine"))
        {
            hasEngine = true;
            PlaceItem(itemsCopy[0], EngineLocation);
            return;
        }

        if (itemsCopy[0].name.Contains("Propeller"))
        {
            hasPropeller = true;
            PlaceItem(itemsCopy[0], PropellerLocation);
            return;
        }
        if (itemsCopy[0].name.Contains("Wing"))
        {
            hasWing = true;
            PlaceItem(itemsCopy[0], WingLocation);
            return;
        }
        if (itemsCopy[0].name.Contains("Wheel"))
        {
            hasWheel = true;
            PlaceItem(itemsCopy[0], WheelLocation);
            return;
        }
    }

    void PlaceItem(Item item, Transform location)
    {
        Destroy(item.GetComponent<Collider2D>());
        Destroy(item.GetComponent<Item>());
        Destroy(item.GetComponent<ItemAnimations>());
        item.transform.parent = location;
        item.transform.localPosition = Vector3.zero;
    }
}
