using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using UnityEngine.AddressableAssets;

[Serializable]
public enum AriaPosition
{
    Outside = 0,
    InsideCafe_Counter = 1,
    InsideCafe_Sit = 2,
    // Use Closeup for room interaction
    InRoom = 3,
    None = -1
}
[Serializable]
public class AriaState
{
    public bool IsMissing;
    [JsonConverter(typeof(StringEnumConverter))]   
    public AriaPosition CurrentLocation;
    [JsonConverter(typeof(AssetReferenceJsonConverter))]
    public AssetReference CurrentTalkingPoint;
    public static AriaState Default = new AriaState(false, AriaPosition.None, null);
    public AriaState(
        bool isMissing, 
        AriaPosition currentLocation,
        AssetReference assetRef)
    {
        IsMissing = isMissing;
        CurrentLocation = currentLocation;
        CurrentTalkingPoint = assetRef;
    }
    public AriaState(AriaState copy)
    {
        IsMissing = copy.IsMissing;
        CurrentLocation = copy.CurrentLocation;
        CurrentTalkingPoint = copy.CurrentTalkingPoint;
    }
}