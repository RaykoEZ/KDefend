using UnityEngine;
using UnityEngine.Playables;

public class CanvasGroupBehaviour : PlayableBehaviour 
{
    [Range(0f, 1f)]
    public float Alpha;
    public bool Interactable;
    public bool BlockRaycast;
    public bool IgnoreParentGroup;
}
