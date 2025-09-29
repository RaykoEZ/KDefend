using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public struct ItemProperty : IEquatable<ItemProperty>
{
    public string Name;
    public string Description;
    //value used for shop exchange cost and sorting
    public int ItemValue;
    public bool Equals(ItemProperty other)
    {
        return other.Name == Name && other.Description == Description;
    }
    public override int GetHashCode()
    {
        return ($"{Name}/{Description}").GetHashCode();
    }
}
public interface IItem
{
    ItemProperty Property { get; }
    public void OnPickup();
}
[RequireComponent(typeof(Collider2D))]
// class to contain item property and interaction triggers
public class Item : MonoBehaviour , IItem
{
    [SerializeField] protected bool m_pickupImmediately = default;
    [SerializeField] protected UnityEvent<Player> m_onUse = default;
    [SerializeField] protected UnityEvent<Player> m_onPickup = default;
    [SerializeField] protected TemporaryInputAction m_pickUpCommand = default;
    [SerializeField] private Image m_cardArt = default;
    protected ItemProperty m_property;
    protected bool m_isEffectActive = false;
    protected Player m_user;
    public ItemProperty Property => m_property;
    public Image CardArt { get => m_cardArt; }
    // instantiate and initialize an item
    public static Item SpawnItem(ItemAsset asset, Transform parent, Vector2 localposition) 
    {
        if (asset == null) return null;
        Item instance = GameUtil.SpawnObject(asset.PrefabRef, localposition, parent);
        instance?.Init(asset);
        return instance;
    }
    // Pickup trigger
    void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.attachedRigidbody == null) return;
        bool compExist = col.attachedRigidbody.TryGetComponent(out Player result);
        m_user = result;
        // when projectile hit this body, trigger on hit effects from projectile
        if (m_pickupImmediately && compExist)
        {
            OnPickup();
        }
        else if (!m_pickupImmediately && compExist)
        {
            m_pickUpCommand?.Enable();
        }
    }
    void OnTriggerExit2D() 
    {
        m_pickUpCommand?.Disable();
    }
    public void Init(ItemAsset asset)
    {
        m_property = asset.Property;
        m_cardArt.sprite = asset.CardArt;
    }
    // when player presses pickup for weapons
    public virtual void PickupDrop(InputAction.CallbackContext _) 
    {
        OnPickup();
    }
    public virtual void OnPickup()
    {
        m_onPickup?.Invoke(m_user);
        m_pickUpCommand?.Disable();
        Destroy(gameObject);
    }
    public void OnPickup(Player player) 
    {
        if (player == null) return;
        m_user = player;
        OnPickup();
    }
    public virtual void UseItem()
    {
        m_isEffectActive = true;
        m_onUse?.Invoke(m_user);
    }
}
