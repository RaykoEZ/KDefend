using System.Collections.Generic;
using UnityEngine;
// The container of one spawn wave, consists of spawn on multiple locations
public class SpawnCollection : ScriptableObject
{
    [SerializeField] List<SpawnGroupDetail> m_enemyGroups = default;
    public static Dictionary<int, SpawnContainer> CreateDictionary(List<SpawnGroupDetail> enemyGroup)
    {
        Dictionary<int, SpawnContainer> ret = new Dictionary<int, SpawnContainer>();
        SpawnContainer c;
        foreach (var item in enemyGroup)
        {
            c = item.GroupsToSpawn;
            if (!ret.TryGetValue(item.SpawnLocationIndex, out SpawnContainer result))
            {
                ret.Add(item.SpawnLocationIndex, c);
            }
            else 
            {
                result?.AddRange(c.Items);
            }
        }
        return ret;
    }
    public static Dictionary<int, SpawnContainer> CreateCollections(List<SpawnCollection> collection)
    {
        List<SpawnGroupDetail> toAdd = new List<SpawnGroupDetail>();
        foreach (var item in collection)
        {
            toAdd.AddRange(item.m_enemyGroups);
        }
        Dictionary<int, SpawnContainer> ret = CreateDictionary(toAdd);
        return ret;
    }
}