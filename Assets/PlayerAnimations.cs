using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Transform playerBody;
    private PlayerMovement playerMove;
    private Vector3 originalPlayerPosition;
    private Quaternion originalPlayerRotation;
    Vector3 velocity = Vector3.zero;
    float smoothTime = 0.15F;
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
            playerBody.localPosition = Vector3.SmoothDamp(playerBody.localPosition, originalPlayerPosition, ref velocity, smoothTime);
            playerBody.localRotation = originalPlayerRotation;
        }
        else
        {
            Vector3 position = playerBody.localPosition;
            float yOffset = (Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * 10)) / 2.5f);
            playerBody.localPosition =  new Vector3(position.x, yOffset , position.z);

            Vector3 rotation = playerBody.localRotation.eulerAngles;
            float zOffset = Mathf.Pow(Mathf.Sin(Time.realtimeSinceStartup * 10) * 3, 3);
            playerBody.localEulerAngles = new Vector3(rotation.x, rotation.y, zOffset);
        }
    }
}
