using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour, IInteractible
{
    // Start is called before the first frame update
    void Start()
    {
        
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
