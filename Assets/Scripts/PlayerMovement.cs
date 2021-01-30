using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool queuedToPickUp = false;
    [SerializeField]
    private float equipDistance = 0.5f;
    private Item queuedItem;
    public bool isMoving = false;

    private LayerMask interactablesLayer = 1 << 6;
    void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (movementInput.magnitude > 0)
        {
            targetPosition = new Vector2(transform.position.x, transform.position.y) + movementInput.normalized * 0.1f;
            queuedToPickUp = false;
            queuedItem = null;
        }

        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            targetPosition = point;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(point, interactablesLayer);
            
            if (hit != null)
            {
                Item item = hit.GetComponent<Item>();
                if (item != null)
                {
                    if (!item.equipped)    //Set item to be equipped once we reach distance
                    {
                        queuedItem = item;
                        queuedToPickUp = true;
                    }
                    else    //UN-EQUIP ITEM
                    {
                        item.OnClick();
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
            isMoving = true;
            transform.localScale = new Vector3(Mathf.Sign(directionX), 1, 1);
        }

        isMoving = false;
    }

    void CheckToPickUp()
    {
        if (!queuedToPickUp)
            return;

        float distance = Vector3.Distance(transform.position,queuedItem.transform.position);
        if (distance < equipDistance)
        {
            queuedToPickUp = false;
            queuedItem.OnClick();
            queuedItem = null;
        }
    }
}
