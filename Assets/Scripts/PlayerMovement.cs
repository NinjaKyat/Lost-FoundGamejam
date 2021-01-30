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
    void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        targetPosition = new Vector2(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(point);
            
            if (hit != null)
            {
                Item item = hit.GetComponent<Item>();
                if (item != null)
                {
                    if (!item.equipped)
                    {
                        targetPosition = point;
                        queuedItem = item;
                        queuedToPickUp = true;
                    }
                    else
                    {
                        item.OnClick();
                    }
                }
                else targetPosition = point;
            }
        }
        CheckToPickUp();
    }

    private Vector2 previousPosition = Vector2.zero;
    private void FixedUpdate()
    {
        float direction = transform.position.x - previousPosition.x;
        previousPosition = transform.position;
        int moveSpeed = player.playerStats.GetStat("movementSpeed");
        rb.MovePosition(Vector2.MoveTowards(this.transform.position, targetPosition, moveSpeed * Time.deltaTime));
        if (Mathf.Abs(direction) > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        }
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
