using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    void Start()
    {
    }

    public void Respawn()
    {
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }
}
