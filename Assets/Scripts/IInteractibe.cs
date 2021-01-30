using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractibe
{
    void Interact();
    Transform Transform { get; }
}
