using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Transform playerBody;
    private PlayerMovement playerMove;
    private Vector3 originalPlayerPosition;
    private Quaternion originalPlayerRotation;
    void Start()
    {
        playerMove = FindObjectOfType<PlayerMovement>();
        //Player object has all body parts in the child object
        playerBody = playerMove.transform.GetChild(0);
        originalPlayerPosition = playerBody.localPosition;
        originalPlayerRotation = playerBody.localRotation;
    }
    
    void Update()
    {
        if (!playerMove.isMoving)
        {
            playerBody.localPosition = originalPlayerPosition;
            playerBody.localRotation = originalPlayerRotation;
        }
        else
        {
            Debug.Log("Animating");
            Vector3 position = playerBody.localPosition;
            playerBody.localPosition +=  new Vector3(position.x, position.y + Mathf.Sin(Time.realtimeSinceStartup * 5), position.z);
        }
    }
}
