using Curry.Events;
using UnityEngine;
public abstract class KDefenderNpcHandler : MonoBehaviour
{
    public abstract void Init(SaveData saveData);
    // handle game events with event flags
    public abstract void HandleEvent(KDEventInfo info);
}
// Triggers dialogue/actions for tutorial Npc - OriaL2
public class Npc_Tutorial : KDefenderNpcHandler
{
    // unlock hidden tutorial
    public void Unlock()
    {

    }
    public void OnEnterSecretRoom() 
    { 
     
    }
    public override void HandleEvent(KDEventInfo info)
    {
        throw new System.NotImplementedException();
    }

    public override void Init(SaveData saveData)
    {
        throw new System.NotImplementedException();
    }
}
public class Npc_SaveLoad : KDefenderNpcHandler
{
    public override void HandleEvent(KDEventInfo info)
    {
        throw new System.NotImplementedException();
    }
    public void Introduce() 
    { 
        
    }
    public void OnSave()
    {

    }
    public void OnLoad()
    {

    }

    public override void Init(SaveData saveData)
    {
        throw new System.NotImplementedException();
    }
}
public class Npc_Notify : KDefenderNpcHandler
{
    public void Introduce()
    {

    }
    public override void HandleEvent(KDEventInfo info)
    {
        throw new System.NotImplementedException();
    }

    public override void Init(SaveData saveData)
    {
        throw new System.NotImplementedException();
    }
}