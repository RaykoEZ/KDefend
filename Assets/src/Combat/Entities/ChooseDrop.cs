using Curry.Events;
using System.Collections.Generic;
using UnityEngine;
// Select 1 from 3 random pool options to obtain/activate
public class ChooseDrop : MonoBehaviour 
{
    [SerializeField] ItemDropList m_dropList = default;
    public void PickupDrop()
    {
        KDefenderStateManager.ItemDropEvent(this, m_dropList);
    }
}