using System.Collections.Generic;
using UnityEngine;
//enemy agent behaviour
[RequireComponent(typeof(Enemy))]
public class Agent : MonoBehaviour
{
    public virtual void CommandGrunts(BaseEntity toCommand) 
    {
        if (toCommand is Enemy ally) 
        {
            Enemy self = GetComponent<Enemy>();
            ally?.Init(self.TargetsOfInterest as List<BaseEntity>, self.CurrentTarget);
        }     
    }
}