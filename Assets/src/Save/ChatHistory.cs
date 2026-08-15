using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
// Stores previous an NPC's conversatons in the chat room
[Serializable]
public class ChatHistory
{
    // whose chat history to assign this to
    public string Username = default;
    [JsonConverter(typeof(AssetReferenceListJsonConverter))]
    [AssetReferenceUILabelRestriction("dialogue")]
    // reference to each dialogue scriptableobject, used to save and load game state
    public List<AssetReference> ChatLogAssets = default;
    public ChatHistory(string name, List<AssetReference> assetRefs)
    {
        Username = name;
        ChatLogAssets = assetRefs;
    }
}