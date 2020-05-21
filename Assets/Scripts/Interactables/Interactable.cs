using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact();    // JRH v0.1.9: Base method for all classes implementing IInteractable
}

public abstract class Interactable : MonoBehaviour, IInteractable
{
    public abstract void Interact();    // JRH v0.1.9: Implementation of the base method from IInteractable
}