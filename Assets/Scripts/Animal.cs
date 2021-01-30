using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Animal : MonoBehaviour, IInteractible
{
    public Collider2D Collider => _collider;
    private Collider2D _collider;

    private float patrolTimer = 0;
    private float patrolTimerMax = 5f;
    
    [SerializeField]
    private float patrolBounds = 3;
    [SerializeField]
    private float moveSpeed = 0;
    
    private Vector2 targetPosition;
    private Rigidbody2D rb;

    private float checkDistance = 4;
    public Transform closestTarget;

    public bool predator = false;
    private bool _isFleeing = false;
    public bool isFleeing
    {
        get { return _isFleeing;}
        set
        {
            if (value != _isFleeing)
            {
                GetNewLocation();
            }

            _isFleeing = value;
        }
    }
    
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        targetPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        patrolTimer -= Time.deltaTime;
        if (patrolTimer < 0)
        {
            GetNewLocation();
            patrolTimer = patrolTimerMax + Random.Range(-2, 2);
        }

        CheckForTargets();

        if (!predator && closestTarget != null)
            isFleeing = true;
        else isFleeing = false;
    }

    public void GetNewLocation()
    {
        Vector2 randomVector = new Vector2(Random.Range(-patrolBounds, patrolBounds), Random.Range(-patrolBounds, patrolBounds));
        targetPosition = new Vector2(transform.position.x, transform.position.y) + randomVector;
    }

    private void FixedUpdate()
    {
        if (closestTarget != null)
        {
            if (predator)
                rb.MovePosition(Vector2.MoveTowards(this.transform.position, closestTarget.position,
                moveSpeed * Time.deltaTime));
            else
            {
                Vector2 awayDirection = transform.position - closestTarget.position;
                rb.MovePosition(Vector2.MoveTowards(this.transform.position, new Vector2(transform.position.x, transform.position.y) + awayDirection,
                    moveSpeed * Time.deltaTime));
            }
        }
        else rb.MovePosition(Vector2.MoveTowards(this.transform.position, targetPosition, moveSpeed * Time.deltaTime));
    }

    public void Interact()
    {
        Debug.Log("Omg it's an animal");
    }

    private void CheckForTargets()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, checkDistance);
        if (targets.Length == 0)
            return;
        
        for (int i = 0; i < targets.Length; i++)
        {
            Player player = targets[i].GetComponent<Player>();
            Animal animal = targets[i].GetComponent<Animal>();

            if (player == null && animal == null || animal == this)
                continue;
            if (player != null)
            {
                closestTarget = player.GetComponent<Transform>();
            }
            else
            {
                if (!predator && animal.predator)
                    closestTarget = animal.GetComponent<Transform>();
                if (predator && !animal.predator)
                    closestTarget = animal.GetComponent<Transform>();
            }
        }

        if (closestTarget != null)
            if (Vector2.Distance(transform.position, closestTarget.position) > checkDistance * 1.5f)
            {
                closestTarget = null;
            } 
    }

}
