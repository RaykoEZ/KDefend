public class EnterField : FieldEffect
{
    protected override void OnEnable()
    {
        base.OnEnable();
        m_rangeRef.OnEnter += Enter;
    }
    void OnDisable()
    {
        m_rangeRef.OnEnter -= Enter;
    }
    public virtual void Enter(BaseEntity target)
    {
        Activate(target);
    }
}
