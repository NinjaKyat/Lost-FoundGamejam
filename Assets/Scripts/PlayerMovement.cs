using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    [SerializeField]
    private float equipDistance = 0.5f;
    private IInteractibe targetInteractible;
    public bool isMoving = false;

    Vector2Int previousGridPosition;

    private LayerMask interactablesLayer = 1 << 6;

    CameraTarget cameraTarget;

    void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
        cameraTarget = GetComponent<CameraTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        if (distanceToTarget > 0.2f) 
            isMoving = true;
            else isMoving = false;
        
        if (movementInput.magnitude > 0)
        {
            targetPosition = new Vector2(transform.position.x, transform.position.y) + movementInput.normalized * 0.1f;
            targetInteractible = null;
            isMoving = true;
        }

        if (Input.GetMouseButton(0))
        {
            targetPosition = point;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(point, interactablesLayer);
            
            if (hit != null)
            {
                var interactible = hit.GetComponent<IInteractibe>();
                if (interactible != null)
                {
                    if (interactible is Item item)
                    {
                        if (!item.equipped)    //Set item to be equipped once we reach distance
                        {
                            targetInteractible = item;
                        }
                        else    //UN-EQUIP ITEM
                        {
                            item.Interact();
                        }
                    } else
                    {
                        targetInteractible = interactible;
                    }
                }
            }
        }
        CheckToPickUp();
    }

    private Vector2 previousPosition = Vector2.zero;
    private void FixedUpdate()
    {
        float directionX = transform.position.x - previousPosition.x;
        float directionY = transform.position.y - previousPosition.y;
        previousPosition = transform.position;
        int moveSpeed = player.playerStats.GetStat("movementSpeed");
        rb.MovePosition(Vector2.MoveTowards(this.transform.position, targetPosition, moveSpeed * Time.deltaTime));
        if (Mathf.Abs(directionX) + Mathf.Abs(directionY) > 0.05f)
        {
            transform.localScale = new Vector3(Mathf.Sign(directionX), 1, 1);
        }
        CheckIfWarpNeeded();
        CheckIfSteppedOnEvent();
    }

    void CheckIfWarpNeeded()
    {
        if (GameGrid.instance == null)
            return;

        var position = (Vector2)transform.position;
        if (GameGrid.instance.WrapAround(ref position))
        {
            var jumpOffset = position - (Vector2)transform.position;
            transform.position = position;
            if (cameraTarget != null)
            {
                cameraTarget.JumpedBy(jumpOffset);
            }
        }
    }

    void CheckIfSteppedOnEvent()
    {
        var gridPosition = GameGrid.instance.WrappedPosition(transform.position);
        if (gridPosition != previousGridPosition)
        {
            var gameEvent = GameGrid.instance.GetTile(gridPosition).GetTopEventIfAvailable(player);
            if (gameEvent != null)
            {
                EventUI.instance.DisplayEvent(gameEvent);
            }
        }

        previousGridPosition = gridPosition;
    }

    void CheckToPickUp()
    {
        if (targetInteractible == null)
            return;

        float distance = Vector3.Distance(transform.position, targetInteractible.Transform.position);
        if (distance < equipDistance)
        {
            targetInteractible.Interact();
            targetInteractible = null;
        }
    }
}
