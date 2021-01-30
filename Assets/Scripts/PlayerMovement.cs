using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    Collider2D collider;
    private Vector2 targetPosition;
    [SerializeField]
    private float interactDistance = 0.5f;
    private IInteractible targetInteractible;
    public bool isMoving = false;

    Vector2Int previousGridPosition;

    CameraTarget cameraTarget;

    void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        targetPosition = transform.position;
        cameraTarget = GetComponent<CameraTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventUI.instance && EventUI.instance.IsShowingUI())
        {
            isMoving = false;
            targetInteractible = null;
            targetPosition = transform.position;
            return;
        }

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
            Collider2D hit = Physics2D.OverlapPoint(point);
            
            if (hit != null)
            {
                var interactible = hit.GetComponent<IInteractible>();
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
        if (GameGrid.instance == null)
            return;

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

        float distance = collider.Distance(targetInteractible.Collider).distance;
        if (distance < interactDistance)
        {
            targetInteractible.Interact();
            targetInteractible = null;
        }
    }
}
