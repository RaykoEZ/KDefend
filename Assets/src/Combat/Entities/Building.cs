public abstract class Building : BaseEntity 
{
    public abstract void Interact();
}
public class PlayerBase : Building
{
    public override void Interact()
    {

    }
}
public class Shop : Building 
{
    public override void Interact()
    {

    }
}
public class EnemyBase : Building 
{
    public override void Interact()
    {

    }
}