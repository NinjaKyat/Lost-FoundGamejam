using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private Player player;
    void Start()
    {
        player = FindObjectOfType<Player>();
    }
    
    void Update()
    {
        
    }
}
