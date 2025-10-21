using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public interface IItem
{
    public ItemProperty Property { get; }
    public void OnPickup();
}
// for stacking items
[Serializable]
public class ItemRankStack
{
    // for implementing item ranks when stacking collectibles,
    // 0: Base Rank effects
    // n: max rank effects
    [SerializeField] List<UnityEvent> m_rankEffects = default;
    protected int m_currentRank = 0;
    // current rank index
    public int CurrentStack { get => m_currentRank; protected set => Mathf.Clamp(value, 0, MaxRankStack); }
    public int MaxRankStack => m_rankEffects.Count - 1;
    public int UpdateRank(int rankDelta, bool triggerEffect = false)
    {
        CurrentStack = CurrentStack + rankDelta;
        // trigger effect on rank up/down?
        if (triggerEffect)
        {
            m_rankEffects[CurrentStack]?.Invoke();
        }
        return CurrentStack;
    }
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
    // for handling item stacks or ranks
    protected ItemRankStack m_rank = new ItemRankStack();
    public ItemProperty Property => m_property;
    public int CurrentStack { get => m_rank.CurrentStack; }
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
