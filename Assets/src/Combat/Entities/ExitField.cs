public class ExitField : FieldEffect
{
    protected override void OnEnable()
    {
        base.OnEnable();
        m_rangeRef.OnExit += Exit;
    }
    void OnDisable()
    {
        m_rangeRef.OnExit -= Exit;
    }
    public virtual void Exit(BaseEntity target)
    {
        Activate(target);
    }
}
