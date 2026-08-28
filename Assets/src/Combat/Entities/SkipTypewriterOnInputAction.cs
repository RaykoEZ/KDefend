using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkipTypewriterOnInputAction : MonoBehaviour
{
    public AbstractTypewriterEffect typewriter; //<-- Assign in inspector.

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame) typewriter.Stop();
    }
}
