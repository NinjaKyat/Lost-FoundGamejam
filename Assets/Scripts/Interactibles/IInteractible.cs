using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    void Interact();
    Collider2D Collider { get; }
}
