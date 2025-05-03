using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.AddressableAssets;
using Newtonsoft.Json.Linq;
public class AssetReferenceJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return typeof(AssetReference).IsAssignableFrom(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string assetGuid = (string)reader.Value;
        AssetReference assetRef = new AssetReference(assetGuid);
        return assetRef;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        AssetReference assetRef = value as AssetReference;
        writer.WriteValue(assetRef.AssetGUID);
    }
}
public class AssetReferenceListJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(List<string>);
    }
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        List<AssetReference> ret = new List<AssetReference>();
        JArray obj = JArray.Load(reader);
        foreach (var guid in obj)
        {
            ret.Add(new AssetReference(guid.Value<string>()));
        }
        return ret;
    }
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var assetRefs = value as List<AssetReference>;
        writer.WriteStartArray();
        foreach (var player in assetRefs)
        {
            writer.WriteValue(JsonConvert.SerializeObject(player.AssetGUID));
        }
        writer.WriteEndArray();
    }
}
