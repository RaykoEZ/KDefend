using UnityEngine;
// Dash for player
[RequireComponent(typeof(Player), typeof(Rigidbody2D), typeof(IMovement))]
public class PlayerDash : Dash 
{
    [SerializeField] float m_invincibleDuration = default;
    [SerializeField] RegenerateCounter m_dashCounter = default;
    Player PlayerRef => GetComponent<Player>();
    Coroutine iFrame;
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
        if (iFrame != null)
        {
            StopCoroutine(iFrame);
            PlayerRef.IsInvincible = false;
        }
        // decrement dash counter
        if (m_dashCounter.TryUseCounter()) 
        {
            // "* 100f" as base multiplier
            RB?.AddForce(m_direction * m_dashStrength * 100f * Time.fixedDeltaTime, ForceMode2D.Impulse);
            PlayerRef.IsInvincible = true;
            iFrame = StartCoroutine(GameUtil.Cooldown(0.1f, () => { PlayerRef.IsInvincible = false; }));
        }      
    }
}
