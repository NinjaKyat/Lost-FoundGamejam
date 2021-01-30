using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private Vector2 targetPosition;
    private bool queuedToPickUp = false;
    [SerializeField]
    private float equipDistance = 0.5f;
    private Item queuedItem;
    void Start()
    {
        player = GetComponent<Player>();
        targetPosition = new Vector2(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        int moveSpeed = player.playerStats.GetStat("movementSpeed");
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Item")
                {
                    Item item = hit.collider.GetComponent<Item>();
                    if (!item.equipped)
                    {
                        targetPosition = hit.point;
                        queuedItem = item;
                        queuedToPickUp = true;
                    }
                    else
                    {
                        item.OnClick();
                    }
                }
                else targetPosition = hit.point;
            }
        }
        transform.position = Vector2.MoveTowards(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);
        CheckToPickUp();
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
