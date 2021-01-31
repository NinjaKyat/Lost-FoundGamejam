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

    public GameObject deathParticles;
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

    public string eventTag = "chicken";

    public bool isMoving = false;

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
            isMoving = true;
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
        else
        {
            if (isMoving)
                rb.MovePosition(Vector2.MoveTowards(this.transform.position, targetPosition, moveSpeed * Time.deltaTime));
            if (Vector3.Distance(transform.position, targetPosition) < 0.2f)
            {
                targetPosition = transform.position;
                isMoving = false;
            }
            else
            {
                isMoving = true;
            }
        }
    }

    public void Interact(Player interactingPlayer)
    {
        EventUI.instance.DisplayEvent(EventMeister.GetRandomEvent(interactingPlayer.playerStats, eventTag));
        Destroy(gameObject);
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
                if (!predator && animal.predator)        //If you're not a predator and see a predator you target it as a flee target
                    closestTarget = animal.GetComponent<Transform>();
                if (predator && !animal.predator)        //If you're a predator and see non predator you target it as a pursue target
                    closestTarget = animal.GetComponent<Transform>();
            }
        }

        if (closestTarget != null)
            if (Vector2.Distance(transform.position, closestTarget.position) > checkDistance * 1.5f)
            {
                closestTarget = null;
            } 
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Player player = other.collider.GetComponent<Player>();
        if (player != null)
            Interact(player);
        Animal otherAnimal = other.collider.GetComponent<Animal>();
        if (otherAnimal == null)
            return;
        if (!predator && otherAnimal.predator)
        {
            if (deathParticles != null)Instantiate(deathParticles, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
