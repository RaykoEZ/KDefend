using UnityEngine;
using UnityEngine.Events;

public abstract class Building : MonoBehaviour 
{
    public virtual void OnPlayerInteract() 
    {
        Interact_Internal();
    }
    protected abstract void Interact_Internal();
}