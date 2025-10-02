using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Toggle))]
public class ShopOptionUI: OptionUI 
{
    [SerializeField] TextMeshProUGUI m_buyLimitText = default;
    [SerializeField] TextMeshProUGUI m_cost = default;
    int m_buyLimit;
    bool m_locked = false;
    Animator m_anim;
    Toggle m_toggle;
    public event ItemOptionEvent OnUnchosen;
    public int BuyLimit { get => m_buyLimit; }
    public bool Locked { get => m_locked;}

    void OnEnable()
    {
        m_anim = GetComponent<Animator>();
        m_toggle = GetComponent<Toggle>();
    }
    public void UpdateBuyLimit(int newLimit)
    {
        m_buyLimit = newLimit;
        m_buyLimitText.text = $"x {m_buyLimit.ToString()} Left";
        SetLocked(newLimit == 0);
    }
    public override void Init(ItemAsset item)
    {
        base.Init(item);
        m_cost.text = $"Cost:{item.Property.ItemValue.ToString()}";
        UpdateBuyLimit(item.Property.ShopBuyLimit);
        // disable option is locked
    }
    public void SetLocked(bool locked)
    {
        m_locked = locked;
        m_toggle.interactable = !locked;
        m_toggle.isOn = !locked;
        if (locked)
        {
            m_anim.SetTrigger("Disabled");
            m_description.text = "Locked";
        }
        else
        {
            m_anim.SetTrigger("Normal");
        }
    }
    public void OnItemToggled(bool isOn)
    {
        if (isOn)
        {
            OnChosen?.Invoke(this);
        }
        else 
        {
            OnUnchosen?.Invoke(this);
        }
    }
}
