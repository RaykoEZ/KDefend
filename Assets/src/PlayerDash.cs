using UnityEngine;
// Dash for player
[RequireComponent(typeof(Player), typeof(Rigidbody2D), typeof(IMovement))]
public class PlayerDash : Dash 
{
    [SerializeField] RegenerateCounter m_dashCounter = default;
    void FixedUpdate()
    {
        TrackPlayerCursor();
    }
    public override bool CanUse()
    {
        return base.CanUse() && m_dashCounter.IsFree;
    }
    protected override void Effect_Internal()
    {
        // decrement dash counter
        if (m_dashCounter.TryUseCounter()) 
        {
            // "* 100f" as base multiplier
            RB?.AddForce(m_direction * m_dashStrength * 100f * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }      
    }
}
