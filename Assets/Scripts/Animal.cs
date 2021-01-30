using UnityEngine;

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
            Vector2 randomVector = new Vector2(Random.Range(-patrolBounds, patrolBounds), Random.Range(-patrolBounds, patrolBounds));
            targetPosition = new Vector2(transform.position.x, transform.position.y) + randomVector;
            patrolTimer = patrolTimerMax;
        }
        
        rb.MovePosition(Vector2.MoveTowards(this.transform.position, targetPosition, moveSpeed * Time.deltaTime));
    }

    public void Interact()
    {
        Debug.Log("Omg it's an animal");
    }


}
