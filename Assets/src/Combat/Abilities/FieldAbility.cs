using UnityEngine;
public class FieldAbility : ActiveAbility
{
    [SerializeField] FieldEffect m_knockbackField = default;
    protected override void Effect_Internal()
    {
        m_knockbackField?.TriggerInRange();
    }
}