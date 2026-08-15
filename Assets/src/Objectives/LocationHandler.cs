using System.Collections.Generic;
using UnityEngine;
// Stores a list of locations of interest
public class LocationHandler : MonoBehaviour
{
    [SerializeField] List<Transform> m_locations = default;
    public List<Transform> Locations { get => m_locations;}
    public Transform GetLocation(int index) 
    {
        if (index < 0 || index >= m_locations.Count) return null;

        return Locations[index];
    }
}