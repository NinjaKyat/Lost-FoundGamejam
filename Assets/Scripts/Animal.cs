using UnityEngine;

public class Animal : MonoBehaviour, IInteractible
{
    public Collider2D Collider => collider;
    private Collider2D collider;
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Omg it's an animal");
    }


}
